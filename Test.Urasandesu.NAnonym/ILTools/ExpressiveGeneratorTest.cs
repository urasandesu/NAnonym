/* 
 * File: ExpressiveGeneratorTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */


using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;
using SR = System.Reflection;
using Test.Urasandesu.NAnonym.Etc;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using System.Linq.Expressions;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class ExpressiveGeneratorTest
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [Test]
        public void EmitTest01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest01Builder = tempModule.DefineType(tempModule.Name + "." + "EmitTest01");

                var ctorBuilder =
                    emitTest01Builder.DefineConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorBuilder.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                    gen.Eval(() => Console.WriteLine("Hello, dynamic assembly !!"));
                });

                var emitTest01 = emitTest01Builder.CreateType();
                var instance = Activator.CreateInstance(emitTest01);

                Console.WriteLine(instance);
            });
        }





        [Test]
        public void EmitTest02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempDynamicMethod = new DynamicMethod("Temp", null, null);
                tempDynamicMethod.ExpressBody(
                gen =>
                {
                    gen.Eval(() => TestHelper.ThrowException("aiueo"));
                });

                var temp = (Action)tempDynamicMethod.CreateDelegate(typeof(Action));
                try
                {
                    temp();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual("aiueo", e.Message);
                }
            });
        }



        [Test]
        public void EmitTest03()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest03TypeBuilder = tempModule.DefineType(tempModule.Name + "." + MethodBase.GetCurrentMethod().Name);
                emitTest03TypeBuilder.AddInterfaceImplementation(typeof(ISample1));

                var ctorConstructorBuilder =
                    emitTest03TypeBuilder.DefineConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorConstructorBuilder.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var sample1 = default(ISample1);
                var executeMethodBuilder = 
                    emitTest03TypeBuilder.DefineMethod(
                        TypeSavable.GetStaticMethodName<string>(() => sample1.Execute), 
                        SR::MethodAttributes.Public | 
                        SR::MethodAttributes.HideBySig | 
                        SR::MethodAttributes.NewSlot | 
                        SR::MethodAttributes.Virtual | 
                        SR::MethodAttributes.Final, 
                        CallingConventions.HasThis, 
                        typeof(void), 
                        TypeSavable.GetStaticMethodParameterTypes<string>(() => sample1.Execute));
                string valueParameterName = TypeSavable.GetStaticMethodParameterNames<string>(() => sample1.Execute)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(() => Dsl.Allocate(stringBuilder).As(new StringBuilder()));
                    gen.Eval(() => stringBuilder.Append("Hello, World!! "));
                    gen.Eval(() => stringBuilder.Append(Dsl.Load<string>(valueParameterName)));
                    gen.Eval(() => TestHelper.ThrowException(stringBuilder.ToString()));
                },
                new ParameterBuilder[] { valueParameterBuilder });

                var emitTest03 = emitTest03TypeBuilder.CreateType();
                var instance = (ISample1)Activator.CreateInstance(emitTest03);

                try
                {
                    instance.Execute("aiueo");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual("Hello, World!! aiueo", e.Message);
                }
            });
        }



        [Test]
        public void EmitTest04()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest04TypeBuilder = tempModule.DefineType(tempModule.Name + "." + MethodBase.GetCurrentMethod().Name);
                emitTest04TypeBuilder.AddInterfaceImplementation(typeof(ISample2));

                var ctorConstructorBuilder =
                    emitTest04TypeBuilder.DefineConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorConstructorBuilder.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest04TypeBuilder.DefineMethod(
                        TypeSavable.GetStaticMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetStaticMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetStaticMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(() => Dsl.Allocate(stringBuilder).As(new StringBuilder()));
                    gen.Eval(() => stringBuilder.Append(Dsl.Load<string>(valueParameterName)));
                    gen.Eval(() => stringBuilder.Append(Dsl.Load<string>(valueParameterName)));
                    gen.Eval(() => stringBuilder.Append(Dsl.Load<string>(valueParameterName)));
                    gen.Eval(() => Dsl.Return(stringBuilder.ToString()));
                },
                new ParameterBuilder[] { valueParameterBuilder });

                var emitTest04 = emitTest04TypeBuilder.CreateType();
                var instance = (ISample2)Activator.CreateInstance(emitTest04);

                Assert.AreEqual("aiueoaiueoaiueo", instance.Print("aiueo"));
            });
        }



        [Test]
        public void EmitTest05()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var candidateCallingCurrentMethods = typeof(ExpressiveGeneratorTest).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
                var callingCurrentMethod = candidateCallingCurrentMethods.FirstOrDefault(method => method.Name.StartsWith("<EmitTest05>"));
                var cacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(callingCurrentMethod);

                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest05TypeBuilder = tempModule.DefineType(tempModule.Name + "." + MethodBase.GetCurrentMethod().Name);
                emitTest05TypeBuilder.AddInterfaceImplementation(typeof(ISample2));

                var ctorConstructorBuilder =
                    emitTest05TypeBuilder.DefineConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorConstructorBuilder.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest05TypeBuilder.DefineMethod(
                        TypeSavable.GetStaticMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetStaticMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetStaticMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(() => Dsl.Allocate(stringBuilder).As(new StringBuilder()));
                    gen.Eval(() => stringBuilder.AppendFormat("{0}\r\n", Dsl.Load<string>(valueParameterName)));
                    gen.Eval(() => stringBuilder.AppendFormat("Cached Field Name: {0}\r\n", Dsl.Extract(cacheField.Name)));
                    gen.Eval(() => stringBuilder.AppendFormat("Cached Field Type: {0}\r\n", Dsl.Extract(cacheField.FieldType.FullName)));
                    gen.Eval(() => Dsl.Return(stringBuilder.ToString()));
                },
                new ParameterBuilder[] { valueParameterBuilder });

                var emitTest05 = emitTest05TypeBuilder.CreateType();
                var instance = (ISample2)Activator.CreateInstance(emitTest05);

                string message = instance.Print("aiueo");

                var match = Regex.Match(message, @"[^\r\n].*[^\r\n]");
                Assert.IsTrue(match.Success);
                Assert.AreEqual("aiueo", match.Value);
                match = match.NextMatch();
                Assert.IsTrue(match.Success);
                Assert.IsTrue(match.Value.IndexOf("Cached Field Name: CS$<>9__CachedAnonymousMethodDelegate") == 0);
                match = match.NextMatch();
                Assert.IsTrue(match.Success);
                Assert.AreEqual("Cached Field Type: System.Action`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", match.Value);
            });
        }



        [Test]
        public void EmitTest06()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest06TypeBuilder = tempModule.DefineType(tempModule.Name + "." + MethodBase.GetCurrentMethod().Name);
                emitTest06TypeBuilder.AddInterfaceImplementation(typeof(ISample2));

                int fieldValue = default(int);
                var fieldValueFieldBuilder = emitTest06TypeBuilder.DefineField(TypeSavable.GetName(() => fieldValue), typeof(int), FieldAttributes.Private);

                var ctorConstructorBuilder =
                    emitTest06TypeBuilder.DefineConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorConstructorBuilder.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                    gen.Eval(() => Dsl.Allocate(fieldValue).As(10));
                },
                new FieldBuilder[] { fieldValueFieldBuilder });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest06TypeBuilder.DefineMethod(
                        TypeSavable.GetStaticMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetStaticMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetStaticMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(() => Dsl.Allocate(stringBuilder).As(new StringBuilder()));
                    gen.Eval(() => stringBuilder.AppendFormat("Parameter = {0}, FieldValue = {1}", Dsl.Load<string>(valueParameterName), fieldValue));
                    gen.Eval(() => Dsl.Return(stringBuilder.ToString()));
                },
                new ParameterBuilder[] { valueParameterBuilder },
                new FieldBuilder[] { fieldValueFieldBuilder });

                var emitTest06 = emitTest06TypeBuilder.CreateType();
                var instance = (ISample2)Activator.CreateInstance(emitTest06);

                string message = instance.Print("aiueo");
                Assert.AreEqual("Parameter = aiueo, FieldValue = 10", message);
            });
        }



        [Test]
        public void EmitTest07()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest07TypeGen = tempModule.AddType(tempModule.Name + "." + MethodBase.GetCurrentMethod().Name);
                emitTest07TypeGen.AddInterfaceImplementation(typeof(ISample2));

                int fieldValue = default(int);

                var fieldValueFieldGen = emitTest07TypeGen.AddField(TypeSavable.GetName(() => fieldValue), typeof(int), FieldAttributes.Private);

                var ctorConstructorGen = 
                    emitTest07TypeGen.AddConstructor(
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.SpecialName |
                        SR::MethodAttributes.RTSpecialName,
                        CallingConventions.Standard,
                        new Type[] { });
                ctorConstructorGen.ExpressBody(
                gen =>
                {
                    gen.Eval(() => Dsl.Base());
                    gen.Eval(() => Dsl.Allocate(fieldValue).As(10));
                });


                var sample2 = default(ISample2);

                var executeMethodGen = 
                    emitTest07TypeGen.AddMethod(
                        TypeSavable.GetStaticMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetStaticMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetStaticMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterGen = executeMethodGen.AddParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodGen.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(() => Dsl.Allocate(stringBuilder).As(new StringBuilder()));
                    gen.Eval(() => stringBuilder.AppendFormat("Parameter = {0}, FieldValue = {1}", Dsl.Load<string>(valueParameterName), fieldValue));
                    gen.Eval(() => Dsl.Return(stringBuilder.ToString()));
                });

                var emitTest07 = ((SRTypeGeneratorImpl)emitTest07TypeGen).Source.CreateType();
                var instance = (ISample2)Activator.CreateInstance(emitTest07);

                string message = instance.Print("aiueo");
                Assert.AreEqual("Parameter = aiueo, FieldValue = 10", message);
            });
        }
    }
}

