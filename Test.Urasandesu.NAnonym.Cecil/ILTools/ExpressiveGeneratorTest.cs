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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Cecil;
using NUnit.Framework;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.System;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;
using MC = Mono.Cecil;
using OpCodes = Urasandesu.NAnonym.ILTools.OpCodes;
using SR = System.Reflection;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;

namespace Test.Urasandesu.NAnonym.Cecil.ILTools
{
    [NewDomainTestFixture]
    public class ExpressiveGeneratorTest : NewDomainTestBase
    {
        [NewDomainTestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [NewDomainTestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [NewDomainSetUp]
        public void SetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [NewDomainTearDown]
        public void TearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
        }

        [NewDomainTest]
        public void EmitTest01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => TestHelper.ThrowException("Hello, World!!"));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(i).As(100));
                    gen.Eval(_ => TestHelper.ThrowException("i.ToString() = {0}", i.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);


                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest03()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(s).As(new string('a', 10)));
                    gen.Eval(_ => TestHelper.ThrowException("s.ToString() = {0}", s.Substring(0, 5)));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest04()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(s).As(new string('a', 10)));
                    gen.Eval(_ => TestHelper.ThrowException("s.ToString() = {0}", s.ToUpper()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest05()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => TestHelper.ThrowException(SR::Emit.OpCodes.Brtrue));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest06()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(one).As(1));
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("1 + 1 = {0}", one + 1)));
                    int i = default(int);
                    gen.Eval(_ => _.Alloc(i).As(default(int)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("++i = {0}", _.AddOneDup(i))));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("i++ = {0}", _.DupAddOne(i))));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("--i = {0}", _.SubOneDup(i))));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest07()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod("dynamicMethod", typeof(string), new Type[] { typeof(int) }, true)));
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Name = {0}", dynamicMethod.Name)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Return Type = {0}", dynamicMethod.ReturnType)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter Length = {0}", dynamicMethod.GetParameters().Length)));
                    gen.Eval(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter[0] Type = {0}", dynamicMethod.GetParameters()[0])));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest08()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest09()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(d).As(0d));
                    gen.Eval(_ => _.Return(value + value * (int)d));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);
                
                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest10()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => TestHelper.ThrowException("GetValue(10) = {0}", TestHelper.GetValue(10).ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        public void EmitTest11()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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
                    gen.Eval(_ => _.Alloc(ctor3).As(typeof(System.Func<,>).MakeGenericType(typeof(String), typeof(String)).GetConstructor(
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                        null,
                                                        new Type[] 
                                                        { 
                                                            typeof(Object), 
                                                            typeof(IntPtr) 
                                                        }, null)));
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Name = {0}\r\n", ctor3.Name));
                    gen.Eval(_ => stringBuilder.AppendFormat("IsPublic = {0}\r\n", ctor3.IsPublic));
                    var parameterInfos = default(ParameterInfo[]);
                    gen.Eval(_ => _.Alloc(parameterInfos).As(ctor3.GetParameters()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter Count = {0}\r\n", parameterInfos.Length));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter[0] = {0}\r\n", parameterInfos[0]));
                    gen.Eval(_ => stringBuilder.AppendFormat("Parameter[1] = {0}\r\n", parameterInfos[1]));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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




        [NewDomainTest]
        [Ignore("Occurred for statement rule conflict.")]
        public void EmitTest12()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef().DuplicateWithoutMember();
                tempAssemblyDef.MainModule.Types.Add(methodTestClassDef);

                var action2LocalVariableDef10 =
                    typeof(MethodTestClass1).ToTypeDef().GetMethod(
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
                    gen.Eval(_ => TestHelper.ThrowException(i + (int)d));
                });

                tempAssemblyDef.Write(tempFileName);

                var scope = action2LocalVariableDef10.CarryPortableScope();
                scope.SetValue(() => i, i);
                scope.SetValue(() => d, d);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef10.Name;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            ((NewDomainTestInfoWithScope)target.TestInfo).Scope.DockWith(target.Instance);
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




        [NewDomainTest]
        [Ignore("Occurred for statement rule conflict.")]
        public void EmitTest13()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef().DuplicateWithoutMember();
                tempAssemblyDef.MainModule.Types.Add(methodTestClassDef);

                var action2LocalVariableDef11 =
                    typeof(MethodTestClass1).ToTypeDef().GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef11.Name = "Action2LocalVariable11";
                var a = new KeyValuePair<int, string>(1, "aiueo");
                methodTestClassDef.Methods.Add(action2LocalVariableDef11);
                action2LocalVariableDef11.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => TestHelper.ThrowException(a));
                });

                tempAssemblyDef.Write(tempFileName);

                var scope = action2LocalVariableDef11.CarryPortableScope();
                scope.SetValue(() => a, a);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef11.Name;
                testInfo.Scope = scope;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            ((NewDomainTestInfoWithScope)target.TestInfo).Scope.DockWith(target.Instance);
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




        [NewDomainTest]
        [Ignore("Occurred for statement rule conflict.")]
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
                    var il = gen.ILOperator;
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
                    gen.Eval(_ => TestHelper.ThrowException(i * (int)d));
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



        [NewDomainTest]
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
                    gen.Eval(_ => TestHelper.ThrowException(_.X(s)));   // Expand により、この場で式ツリーが展開される。
                    //gen.Eval(_ => TestHelper.ThrowException(_.Expand(() => new { Key = 1, Value = "aiueo" })));    // オブジェクトは展開できない。リテラルとして CIL に埋め込めるものだけ。
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




        [NewDomainTest]
        public void EmitTest18()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var candidateCallingCurrentMethods = typeof(ExpressiveGeneratorTest).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
                var callingCurrentMethod = candidateCallingCurrentMethods.FirstOrDefault(method => method.Name.StartsWith("<EmitTest18>"));
                var cacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(callingCurrentMethod);

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
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Name: {0}\r\n", _.X(cacheField.Name)));
                    gen.Eval(_ => stringBuilder.AppendFormat("Cached Field Type: {0}\r\n", _.X(cacheField.FieldType.FullName)));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                // 展開してしまうので、別 AppDomain でも問題なし。
                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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
                        catch (TargetInvocationException e)
                        {
                            var match = Regex.Match(e.InnerException.Message, @"[^\r\n].*[^\r\n]");
                            Assert.IsTrue(match.Success);
                            Assert.IsTrue(match.Value.IndexOf("Cached Field Name: CS$<>9__CachedAnonymousMethodDelegate") == 0);
                            match = match.NextMatch();
                            Assert.IsTrue(match.Success);
                            Assert.AreEqual("Cached Field Type: System.Action`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", match.Value);
                        }
                    };

                return testInfo;
            }));
        }




        [NewDomainTest]
        [Ignore("Occurred for statement rule conflict.")]
        public void EmitTest19()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
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

                    gen.Eval(_ => TestHelper.ThrowException(string.Format("{0}, {1}", a, b.ToString("yyyy/MM/dd"))));

                    scope.SetValue(() => a, a);
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
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
                            ((NewDomainTestInfoWithScope)target.TestInfo).Scope.SetValue(() => b, b);
                            ((NewDomainTestInfoWithScope)target.TestInfo).Scope.DockWith(target.Instance);
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("[1, aiueo], 2010/08/31", e.InnerException.Message);
                        }

                        Assert.AreEqual(2, ((NewDomainTestInfoWithScope)target.TestInfo).Scope.Items.Count);
                        ((NewDomainTestInfoWithScope)target.TestInfo).Scope.Items.ForEach(
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

                        {
                            var a = default(KeyValuePair<int, string>);
                            Assert.IsTrue(((NewDomainTestInfoWithScope)target.TestInfo).Scope.Contains(() => a));
                            Assert.AreEqual(new KeyValuePair<int, string>(1, "aiueo"), ((NewDomainTestInfoWithScope)target.TestInfo).Scope.GetValue(() => a));
                        }
                    };

                return testInfo;
            }));
        }




        [NewDomainTest]
        public void EmitTest20()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var action2LocalVariableDef20 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef20.Name = "Action2LocalVariable20";
                methodTestClassDef.Methods.Add(action2LocalVariableDef20);
                action2LocalVariableDef20.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    var methodToCall = default(Func<string, string>);
                    gen.Eval(_ => _.Alloc(methodToCall).As(null));
                    gen.Eval(_ => stringBuilder.AppendFormat("methodToCall == null = {0}", methodToCall == null));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef20.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("methodToCall == null = True", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [NewDomainTest]
        public void EmitTest21()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                methodTestClassDef.Module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(tempFileName);

                var methodToCall20 = default(Func<string, string>);
                var methodToCall20Def = 
                    new FieldDefinition(
                        TypeSavable.GetName(() => methodToCall20), 
                        MC::FieldAttributes.Private, 
                        methodTestClassDef.Module.Import(typeof(Func<string, string>)));
                methodTestClassDef.Fields.Add(methodToCall20Def);

                var action2LocalVariableDef21 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef21.Name = "Action2LocalVariable21";
                methodTestClassDef.Methods.Add(action2LocalVariableDef21);
                action2LocalVariableDef21.ExpressBody(
                gen =>
                {
                    var stringBuilder = default(StringBuilder);
                    gen.Eval(_ => _.Alloc(stringBuilder).As(new StringBuilder()));
                    gen.Eval(_ => _.If(methodToCall20 == null));
                    var methodToCall = default(Func<string, string>);
                    gen.Eval(_ => _.Alloc(methodToCall).As(null));
                    gen.Eval(_ => _.Alloc(methodToCall).As(new Func<string, string>(TestHelper.GetValue)));
                    gen.Eval(_ => _.Alloc(methodToCall20).As(methodToCall));
                    gen.Eval(_ => _.EndIf());
                    gen.Eval(_ => stringBuilder.AppendFormat("methodToCall(\"aiueo\") = {0}", methodToCall20("aiueo")));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                });

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = methodTestClassDef.FullName;
                testInfo.MethodName = action2LocalVariableDef21.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        try
                        {
                            target.Method.Invoke(target.Instance, null);
                            Assert.Fail();
                        }
                        catch (TargetInvocationException e)
                        {
                            Assert.AreEqual("methodToCall(\"aiueo\") = aiueo", e.InnerException.Message);
                        }
                    };

                return testInfo;
            }));
        }




        [NewDomainTest]
        [Ignore("Occurred for statement rule conflict.")]
        public void EmitTest22()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef =
                    new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef =
                    AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);

                var emitTest22Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest22");

                var ctorGen = 
                    emitTest22Gen.AddConstructor(
                        SR::MethodAttributes.Public | 
                        SR::MethodAttributes.HideBySig | 
                        SR::MethodAttributes.SpecialName | 
                        SR::MethodAttributes.RTSpecialName, 
                        CallingConventions.HasThis, 
                        Type.EmptyTypes);
                ctorGen.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });

                var action2SameDomain1Gen = 
                    emitTest22Gen.AddMethod(
                        "Action2SameDomain1", 
                        SR::MethodAttributes.Public | 
                        SR::MethodAttributes.HideBySig, 
                        typeof(void), 
                        Type.EmptyTypes);
                int i = 10;
                double d = 10.0d;
                action2SameDomain1Gen.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => TestHelper.ThrowException(i * (int)d));
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest22 = assembly.GetType(emitTest22Gen.FullName);
                var instance = Activator.CreateInstance(emitTest22);
                var action2SameDomain1 = emitTest22.GetMethod(action2SameDomain1Gen.Name);
                var scope = action2SameDomain1Gen.CarryPortableScope();
                scope.SetValue(() => i, i);
                scope.SetValue(() => d, d);
                scope.DockWith(instance);
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




        [NewDomainTest]
        public void EmitTest23()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest23Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest23");


                var ctorDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig | 
                                        SR::MethodAttributes.SpecialName | SR::MethodAttributes.RTSpecialName;

                var ctorGen = emitTest23Gen.AddConstructor(ctorDefaultAttr, CallingConventions.HasThis, Type.EmptyTypes);
                ctorGen.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });



                var methodDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig;

                var generativeEmit1Gen = emitTest23Gen.AddMethod("GenerativeEmit1", methodDefaultAttr, typeof(void), Type.EmptyTypes);
                generativeEmit1Gen.ExpressBody(
                gen =>
                {
                    var dynamicMethod = default(DynamicMethod);
                    gen.Eval(_ => _.Alloc(dynamicMethod).As(new DynamicMethod("DynamicMethod", null, null)));

                    var il = default(ILGenerator);
                    gen.Eval(_ => _.Alloc(il).As(dynamicMethod.GetILGenerator()));

                    gen.ExpressEmit(() => il,
                    _gen =>
                    {
                        _gen.Eval(_ => TestHelper.ThrowException("testtest"));
                    });

                    var action = default(Action);
                    gen.Eval(_ => _.Alloc(action).As((Action)dynamicMethod.CreateDelegate(typeof(Action))));
                    gen.Eval(_ => action());
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest23 = assembly.GetType(emitTest23Gen.FullName);
                var instance = Activator.CreateInstance(emitTest23);
                var generativeEmit1 = emitTest23.GetMethod(generativeEmit1Gen.Name);
                try
                {
                    generativeEmit1.Invoke(instance, null);
                    Assert.Fail();
                }
                catch (TargetInvocationException e)
                {
                    Assert.AreEqual("testtest", e.InnerException.Message);
                }
            });
        }




        [NewDomainTest]
        public void EmitTest24()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest24Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest24");


                var ctorDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig |
                                        SR::MethodAttributes.SpecialName | SR::MethodAttributes.RTSpecialName;

                var ctorGen = emitTest24Gen.AddConstructor(ctorDefaultAttr, CallingConventions.HasThis, Type.EmptyTypes);
                ctorGen.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });



                var methodDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig;

                var reflectiveDesigner1Gen = emitTest24Gen.AddMethod("ReflectiveDesign1", methodDefaultAttr, typeof(void), Type.EmptyTypes);
                reflectiveDesigner1Gen.ExpressBody(
                gen =>
                {
                    gen.ExpressReflection(
                    _gen =>
                    {
                        var throwExceptionInfo = typeof(TestHelper).GetMethod("ThrowException",
                                                                              BindingFlags.Public | BindingFlags.Static,
                                                                              null,
                                                                              new Type[] 
                                                                              { 
                                                                                  typeof(string) 
                                                                              },
                                                                              null);

                        _gen.Eval(_ => throwExceptionInfo.Invoke(null, new object[] { "testtest" }));
                    });
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest24 = assembly.GetType(emitTest24Gen.FullName);
                var instance = Activator.CreateInstance(emitTest24);
                var generativeEmit1 = emitTest24.GetMethod(reflectiveDesigner1Gen.Name);
                try
                {
                    generativeEmit1.Invoke(instance, null);
                    Assert.Fail();
                }
                catch (TargetInvocationException e)
                {
                    Assert.AreEqual("testtest", e.InnerException.Message);
                }
            });
        }




        [NewDomainTest]
        public void EmitTest25()
        {
            TestHelper.UsingTempFile(tempFileName =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest25Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest25");


                var ctorDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig |
                                        SR::MethodAttributes.SpecialName | SR::MethodAttributes.RTSpecialName;

                var ctorGen = emitTest25Gen.AddConstructor(ctorDefaultAttr, CallingConventions.HasThis, Type.EmptyTypes);
                ctorGen.ExpressBody(
                gen =>
                {
                    gen.Eval(_ => _.Base());
                });



                var methodDefaultAttr = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig;

                var ldStAlloc1Gen = emitTest25Gen.AddMethod("LdStAlloc1", methodDefaultAttr, typeof(void), Type.EmptyTypes);
                ldStAlloc1Gen.ExpressBody(
                gen =>
                {
                    var i = default(int);
                    var name = "i";
                    gen.Eval(_ => _.Alloc(i).As(100));
                    gen.Eval(_ => _.St<int>(name).As(50));
                    gen.Eval(_ => TestHelper.ThrowException(_.Ld<int>(name)));
                });

                tempAssemblyDef.Write(tempFileName);

                var assembly = Assembly.LoadFile(Path.GetFullPath(tempFileName));
                var emitTest25 = assembly.GetType(emitTest25Gen.FullName);
                var instance = Activator.CreateInstance(emitTest25);
                var generativeEmit1 = emitTest25.GetMethod(ldStAlloc1Gen.Name);
                try
                {
                    generativeEmit1.Invoke(instance, null);
                    Assert.Fail();
                }
                catch (TargetInvocationException e)
                {
                    Assert.AreEqual("50", e.InnerException.Message);
                }
            });
        }
    }
}

