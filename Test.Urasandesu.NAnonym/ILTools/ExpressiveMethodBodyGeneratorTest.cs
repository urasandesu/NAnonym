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

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public class ExpressiveMethodBodyGeneratorTest
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
                    gen.Eval(_ => _.Base());
                    gen.Eval(_ => Console.WriteLine("Hello, dynamic assembly !!"));
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
                    gen.Eval(_ => TestHelper.ThrowException("aiueo"));
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
                    gen.Eval(_ => _.Base());
                });

                var sample1 = default(ISample1);
                var executeMethodBuilder = 
                    emitTest03TypeBuilder.DefineMethod(
                        TypeSavable.GetMethodName<string>(() => sample1.Execute), 
                        SR::MethodAttributes.Public | 
                        SR::MethodAttributes.HideBySig | 
                        SR::MethodAttributes.NewSlot | 
                        SR::MethodAttributes.Virtual | 
                        SR::MethodAttributes.Final, 
                        CallingConventions.HasThis, 
                        typeof(void), 
                        TypeSavable.GetMethodParameterTypes<string>(() => sample1.Execute));
                string valueParameterName = TypeSavable.GetMethodParameterNames<string>(() => sample1.Execute)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.St(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.Append("Hello, World!! "));
                    gen.Eval(_ => stringBuilder.Append(_.Ld<string>(_.X(valueParameterName))));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
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
                    gen.Eval(_ => _.Base());
                });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest04TypeBuilder.DefineMethod(
                        TypeSavable.GetMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.St(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.Append(_.Ld<string>(_.X(valueParameterName))));
                    gen.Eval(_ => stringBuilder.Append(_.Ld<string>(_.X(valueParameterName))));
                    gen.Eval(_ => stringBuilder.Append(_.Ld<string>(_.X(valueParameterName))));
                    gen.Eval(_ => _.Return(stringBuilder.ToString()));
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
                var candidateCallingCurrentMethods = typeof(ExpressiveMethodBodyGeneratorTest).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
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
                    gen.Eval(_ => _.Base());
                });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest05TypeBuilder.DefineMethod(
                        TypeSavable.GetMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.St(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", _.Ld<string>(_.X(valueParameterName))));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Name: {0}\r\n", _.X(cacheField.Name)));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Type: {0}\r\n", _.X(cacheField.FieldType.FullName)));
                    gen.Eval(_ => _.Return(stringBuilder.ToString()));
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
                    gen.Eval(_ => _.Base());
                    gen.Eval(_ => _.St(fieldValue).As(10));
                },
                new FieldBuilder[] { fieldValueFieldBuilder });

                var sample2 = default(ISample2);
                var executeMethodBuilder =
                    emitTest06TypeBuilder.DefineMethod(
                        TypeSavable.GetMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterBuilder = executeMethodBuilder.DefineParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodBuilder.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.St(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter = {0}, FieldValue = {1}", _.Ld<string>(_.X(valueParameterName)), fieldValue));
                    gen.Eval(_ => _.Return(stringBuilder.ToString()));
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
                    gen.Eval(_ => _.Base());
                    gen.Eval(_ => _.St(fieldValue).As(10));
                });


                var sample2 = default(ISample2);

                var executeMethodGen = 
                    emitTest07TypeGen.AddMethod(
                        TypeSavable.GetMethodName<string, string>(() => sample2.Print),
                        SR::MethodAttributes.Public |
                        SR::MethodAttributes.HideBySig |
                        SR::MethodAttributes.NewSlot |
                        SR::MethodAttributes.Virtual |
                        SR::MethodAttributes.Final,
                        CallingConventions.HasThis,
                        typeof(string),
                        TypeSavable.GetMethodParameterTypes<string, string>(() => sample2.Print));
                string valueParameterName = TypeSavable.GetMethodParameterNames<string, string>(() => sample2.Print)[0];
                var valueParameterGen = executeMethodGen.AddParameter(1, ParameterAttributes.In, valueParameterName);
                executeMethodGen.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.St(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter = {0}, FieldValue = {1}", _.Ld<string>(_.X(valueParameterName)), fieldValue));
                    gen.Eval(_ => _.Return(stringBuilder.ToString()));
                });

                var emitTest07 = ((SRTypeGeneratorImpl)emitTest07TypeGen).Source.CreateType();
                var instance = (ISample2)Activator.CreateInstance(emitTest07);

                string message = instance.Print("aiueo");
                Assert.AreEqual("Parameter = aiueo, FieldValue = 10", message);
            });
        }
    }
}
