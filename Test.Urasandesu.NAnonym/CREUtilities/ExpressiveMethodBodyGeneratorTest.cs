using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.CREUtilities;
using Urasandesu.NAnonym.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Urasandesu.NAnonym.Test;
using System.Threading;
using Test.Urasandesu.NAnonym.DI;
using OpCodes = Urasandesu.NAnonym.CREUtilities.OpCodes;

namespace Test.Urasandesu.NAnonym.CREUtilities
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
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action1Def2 =
                    methodTestClassDef.GetMethod(
                        "Action1",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action1Def2.Name = "Action12";
                methodTestClassDef.Methods.Add(action1Def2);
                action1Def2.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException("Hello, World!!"));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action1Def2.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("Hello, World!!", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef2 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef2.Name = "Action2LocalVariable2";
                methodTestClassDef.Methods.Add(action2LocalVariableDef2);
                action2LocalVariableDef2.ExpressBody(
                gen =>
                {
                    int i = default(int);
                    gen.Eval(_ => _.Addloc(i, 100));
                    gen.Eval(_ => ThrowException("i.ToString() = {0}", i.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);


                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef2.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("i.ToString() = 100", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest03()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef3 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef3.Name = "Action2LocalVariable3";
                methodTestClassDef.Methods.Add(action2LocalVariableDef3);
                action2LocalVariableDef3.ExpressBody(
                gen =>
                {
                    string s = default(string);
                    gen.Eval(_ => _.Addloc(s, new string('a', 10)));
                    gen.Eval(_ => ThrowException("s.ToString() = {0}", s.Substring(0, 5)));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef3.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("s.ToString() = aaaaa", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest04()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef4 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef4.Name = "Action2LocalVariable4";
                methodTestClassDef.Methods.Add(action2LocalVariableDef4);
                action2LocalVariableDef4.ExpressBody(
                gen =>
                {
                    string s = new string('a', 10);
                    gen.Eval(_ => _.Addloc(s, new string('a', 10)));
                    gen.Eval(_ => ThrowException("s.ToString() = {0}", s.ToUpper()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef4.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("s.ToString() = AAAAAAAAAA", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest05()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef5 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef5.Name = "Action2LocalVariable5";
                methodTestClassDef.Methods.Add(action2LocalVariableDef5);
                action2LocalVariableDef5.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException(SR::Emit.OpCodes.Brtrue));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef5.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("brtrue", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest06()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef6 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef6.Name = "Action2LocalVariable6";
                methodTestClassDef.Methods.Add(action2LocalVariableDef6);
                action2LocalVariableDef6.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    int one = default(int);
                    gen.Eval(_ => _.Addloc(one, 1));
                    gen.Eval(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("1 + 1 = {0}", one + 1)));
                    int i = default(int);
                    gen.Eval(_ => _.Addloc(i, default(int)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("++i = {0}", _.AddOneDup(i))));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("i++ = {0}", _.DupAddOne(i))));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("--i = {0}", _.SubOneDup(i))));
                    gen.Eval(_ => ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef6.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual(
@"1 + 1 = 2
++i = 1
i++ = 1
--i = 1
"
                                , e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest07()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef7 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef7.Name = "Action2LocalVariable7";
                methodTestClassDef.Methods.Add(action2LocalVariableDef7);
                action2LocalVariableDef7.ExpressBody(
                gen =>
                {
                    var dynamicMethod = default(DynamicMethod);
                    gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod("dynamicMethod", typeof(string), new Type[] { typeof(int) }, true)));
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Name = {0}", dynamicMethod.Name)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Return Type = {0}", dynamicMethod.ReturnType)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter Length = {0}", dynamicMethod.GetParameters().Length)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter[0] Type = {0}", dynamicMethod.GetParameters()[0])));
                    gen.Eval(_ => ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef7.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual(
@"Name = dynamicMethod
Return Type = System.String
Parameter Length = 1
Parameter[0] Type = Int32 
"
                                , e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest08()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var func1Parameters2 =
                    methodTestClassDef.GetMethod(
                        "Func1Parameters",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { typeof(int) }).DuplicateWithoutBody();
                func1Parameters2.Name = "Func1Parameters2";
                methodTestClassDef.Methods.Add(func1Parameters2);
                func1Parameters2.ExpressBody(
                gen =>
                {
                    int value = default(int);
                    gen.Eval(_ => _.Return(value + value * value));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = func1Parameters2.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        object result = target.Method.Invoke(target.Instance, new object[] { 10 });
                        Assert.AreEqual(110, result);
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest09()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var func1Parameters3 =
                    methodTestClassDef.GetMethod(
                        "Func1Parameters",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { typeof(int) }).DuplicateWithoutBody();
                func1Parameters3.Name = "Func1Parameters3";
                methodTestClassDef.Methods.Add(func1Parameters3);
                func1Parameters3.ExpressBody(
                gen =>
                {
                    int value = default(int);
                    double d = default(double);
                    gen.Eval(_ => _.Addloc(d, 0d));
                    gen.Eval(_ => _.Return(value + value * (int)d));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);
                
                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = func1Parameters3.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        object result = target.Method.Invoke(target.Instance, new object[] { 10 });
                        Assert.AreEqual(10, result);
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest10()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef8 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef8.Name = "Action2LocalVariable8";
                methodTestClassDef.Methods.Add(action2LocalVariableDef8);
                action2LocalVariableDef8.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException("GetValue(10) = {0}", GetValue(10).ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef8.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual("GetValue(10) = 10", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest11()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef9 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef9.Name = "Action2LocalVariable9";
                methodTestClassDef.Methods.Add(action2LocalVariableDef9);
                action2LocalVariableDef9.ExpressBody(
                gen =>
                {
                    var ctor3 = default(ConstructorInfo);
                    gen.Eval(_ => _.Addloc(ctor3, typeof(System.Func<,>).MakeGenericType(typeof(String), typeof(String)).GetConstructor(
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                        null,
                                                        new Type[] 
                                                        { 
                                                            typeof(Object), 
                                                            typeof(IntPtr) 
                                                        }, null)));
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Name = {0}\r\n", ctor3.Name));
                    gen.Eval(_ => stringBuilder.AppendFormat("IsPublic = {0}\r\n", ctor3.IsPublic));
                    var parameterInfos = default(ParameterInfo[]);
                    gen.Eval(_ => _.Addloc(parameterInfos, ctor3.GetParameters()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter Count = {0}\r\n", parameterInfos.Length));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter[0] = {0}\r\n", parameterInfos[0]));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter[1] = {0}\r\n", parameterInfos[1]));
                    gen.Eval(_ => ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef9.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual(
@"Name = .ctor
IsPublic = True
Parameter Count = 2
Parameter[0] = System.Object object
Parameter[1] = IntPtr method
"
                                , e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest12()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef10 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef10.Name = "Action2LocalVariable10";
                int i = 100;
                double d = 100d;
                methodTestClassDef.Methods.Add(action2LocalVariableDef10);
                action2LocalVariableDef10.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException(i + (int)d));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var scope = action2LocalVariableDef10.CarryPortableScope();
                scope.SetValue(() => i, i);
                scope.SetValue(() => d, d);
                //scope.Bind(() => i, i);
                //scope.Bind(() => d, d);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef10.Name;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.TestInfo.Scope.DockWith(target.Instance);
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("200", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest13()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef11 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef11.Name = "Action2LocalVariable11";
                var a = new KeyValuePair<int, string>(1, "aiueo");
                methodTestClassDef.Methods.Add(action2LocalVariableDef11);
                action2LocalVariableDef11.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException(a));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                // 書き込んだ時点で参照が変わってない？
                // 同期は取られてるはず…か。
                //var scope = action2LocalVariableDef11.CreateScope();
                var scope = action2LocalVariableDef11.CarryPortableScope();
                scope.SetValue(() => a, a);
                //scope.Bind(() => a, a);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef11.Name;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.TestInfo.Scope.DockWith(target.Instance);
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("[1, aiueo]", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest14()
        {
            // TODO: 同一 AppDomain の場合は SharedScope の明示的な作成は必要ないはず。
            //       AssemblyBuilder からは難しいが、DynamicMethod からならがっつり短くできそう。
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                var emitTest14Def =
                    new TypeDefinition(
                        tempAssemblyNameDef.Name,
                        "EmitTest14",
                        MC::TypeAttributes.AutoClass |
                        MC::TypeAttributes.AnsiClass |
                        MC::TypeAttributes.BeforeFieldInit |
                        MC::TypeAttributes.Public,
                        tempAssemblyDef.MainModule.Import(typeof(object)));
                tempAssemblyDef.MainModule.Types.Add(emitTest14Def);

                var ctorDef =
                    new MethodDefinition(
                        ".ctor",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig |
                        MC::MethodAttributes.SpecialName |
                        MC::MethodAttributes.RTSpecialName,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                emitTest14Def.Methods.Add(ctorDef);
                ctorDef.ExpressBody(
                gen =>
                {
                    var il = gen.GetILOperator();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { }));
                });

                var action2SameDomain1Def =
                    new MethodDefinition(
                        "Action2SameDomain1",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                emitTest14Def.Methods.Add(action2SameDomain1Def);
                int i = 10;
                double d = 10.0d;
                action2SameDomain1Def.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException(i * (int)d));
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest14 = assembly.GetType(emitTest14Def.FullName);
                var instance = Activator.CreateInstance(emitTest14);
                var action2SameDomain1 = emitTest14.GetMethod(action2SameDomain1Def.Name);
                var scope = action2SameDomain1Def.CarryPortableScope();
                scope.SetValue(() => i, i);
                scope.SetValue(() => d, d);
                scope.DockWith(instance);
                //scope.Bind(() => i, i);
                //scope.Bind(() => d, d);
                //scope.Reinitialize(instance);
                try
                {
                    action2SameDomain1.Invoke(instance, null);
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual("100", e.InnerException.Message);
                }
            });
        }




        [Test]
        public void EmitTest15()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(tempFileName));
                var tempAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.Run);
                var tempModule = tempAssemblyBuilder.DefineDynamicModule(tempAssemblyName.Name);

                var emitTest15Builder = tempModule.DefineType(tempModule.Name + "." + "EmitTest15");

                var ctorBuilder =
                    emitTest15Builder.DefineConstructor(
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

                var emitTest15 = emitTest15Builder.CreateType();
                var instance = Activator.CreateInstance(emitTest15);

                Console.WriteLine(instance);
            });
        }



        [Test]
        public void EmitTest16()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                var emitTest16Def =
                    new TypeDefinition(
                        tempAssemblyNameDef.Name,
                        "EmitTest16",
                        MC::TypeAttributes.AutoClass |
                        MC::TypeAttributes.AnsiClass |
                        MC::TypeAttributes.BeforeFieldInit |
                        MC::TypeAttributes.Public,
                        tempAssemblyDef.MainModule.Import(typeof(object)));
                tempAssemblyDef.MainModule.Types.Add(emitTest16Def);

                var ctorDef =
                    new MethodDefinition(
                        ".ctor",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig |
                        MC::MethodAttributes.SpecialName |
                        MC::MethodAttributes.RTSpecialName,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                emitTest16Def.Methods.Add(ctorDef);
                ctorDef.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var action2SameDomain2Def =
                    new MethodDefinition(
                        "Action2SameDomain2",
                        MC::MethodAttributes.Public |
                        MC::MethodAttributes.HideBySig,
                        tempAssemblyDef.MainModule.Import(typeof(void)));
                emitTest16Def.Methods.Add(action2SameDomain2Def);
                string s = "Hello, Dynamic Assmbly!!";
                action2SameDomain2Def.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException(_.Expand(s)));   // Expand により、この場で式ツリーが展開される。
                    //gen.Eval(_ => ThrowException(_.Expand(() => new { Key = 1, Value = "aiueo" })));    // オブジェクトは展開できない。リテラルとして CIL に埋め込めるものだけ。
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest16 = assembly.GetType(emitTest16Def.FullName);
                var instance = Activator.CreateInstance(emitTest16);
                var action2SameDomain2 = emitTest16.GetMethod(action2SameDomain2Def.Name);
                try
                {
                    action2SameDomain2.Invoke(instance, null);
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual("Hello, Dynamic Assmbly!!", e.InnerException.Message);
                }
            });
        }




        [Test]
        public void EmitTest17()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempDynamicMethod = new DynamicMethod("Temp", null, null);
                tempDynamicMethod.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => ThrowException("aiueo"));
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
        public void EmitTest18()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // MEMO: 偶然見つけた。Anonymous メソッド内に外側のクラスのメンバ紛れ込ますと cache されなくなってしまうあ。
                // MEMO: なるほど。cache されない代わりに完全に中身が展開されるらしい。
                // TODO: GlobalClass セットアップクラスでインスタンスメンバ参照させることはまずないと思うけど・・・。一応その場合のパスも考えるべし。

                var candidateCallingCurrentMethods = typeof(ExpressiveMethodBodyGeneratorTest).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
                var callingCurrentMethod = candidateCallingCurrentMethods.FirstOrDefault(method => method.Name.StartsWith("<EmitTest18>"));
                var cacheField = TypeAnalyzer.GetCacheFieldIfAnonymous(callingCurrentMethod);

                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef12 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef12.Name = "Action2LocalVariable12";
                methodTestClassDef.Methods.Add(action2LocalVariableDef12);
                action2LocalVariableDef12.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Name: {0}\r\n", _.Expand(cacheField.Name)));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Type: {0}\r\n", _.Expand(cacheField.FieldType.FullName)));
                    gen.Eval(_ => ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                // 展開してしまうので、別 AppDomain でも問題なし。
                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef12.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (Exception e)
                        {
                            Assert.AreEqual(
@"Cached Field Name: CS$<>9__CachedAnonymousMethodDelegatea2
Cached Field Type: System.Action`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
"
                                , e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [Test]
        public void EmitTest19()
        {
            TestHelper.UsingTempFile(tempFileName =>
            NewDomainTest.Transfer(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef19 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef19.Name = "Action2LocalVariable19";
                methodTestClassDef.Methods.Add(action2LocalVariableDef19);
                var scope = action2LocalVariableDef19.CarryPortableScope();
                action2LocalVariableDef19.ExpressBody(
                gen =>
                {
                    var a = new KeyValuePair<int, string>(1, "aiueo");
                    var b = default(DateTime);                                  // 別の AppDomain で値を作成してみる。

                    gen.Eval(_ => ThrowException(string.Format("{0}, {1}", a, b.ToString("yyyy/MM/dd"))));

                    scope.SetValue(() => a, a);
                });

                // あー、ここは TEMP ファイルじゃだめなのか・・・
                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfo();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef19.Name;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            var b = new DateTime(2010, 8, 31);
                            target.TestInfo.Scope.SetValue(() => b, b);
                            target.TestInfo.Scope.DockWith(target.Instance);
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("[1, aiueo], 2010/08/31", e.InnerException.Message);
                        }

                        // TODO: 対象のスコープにどんな変数が定義してあるかこんな感じで見たい。
                        Assert.AreEqual(2, target.TestInfo.Scope.Items.Count);
                        target.TestInfo.Scope.Items.ForEach(
                        (item, index) =>
                        {
                            switch (index)
                            {
                                case 0:
                                    Assert.AreEqual("a", item.Name);
                                    Assert.AreEqual(new KeyValuePair<int, string>(1, "aiueo"), item.Value);
                                    break;
                                case 1:
                                    Assert.AreEqual("b", item.Name);
                                    Assert.AreEqual(new DateTime(2010, 8, 31), item.Value);
                                    break;
                                default:
                                    Assert.Fail();
                                    break;
                            }
                        });

                        // TODO: 定義してあるかのどうかのチェックと、設定済みの値をこんな感じで見たい。
                        {
                            var a = default(KeyValuePair<int, string>);
                            Assert.IsTrue(target.TestInfo.Scope.Contains(() => a));
                            Assert.AreEqual(new KeyValuePair<int, string>(1, "aiueo"), target.TestInfo.Scope.GetValue(() => a));
                        }
                    };

                return testInfo;
            }));
        }





        public static void ThrowException(string value)
        {
            throw new Exception(value);
        }

        public static void ThrowException(string value, object param)
        {
            throw new Exception(string.Format(value, param));
        }

        public static void ThrowException(object o)
        {
            throw new Exception(string.Format("{0}", o));
        }

        public static int GetValue(int value)
        {
            return value;
        }
    }
}
