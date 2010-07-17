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

namespace Test.Urasandesu.NAnonym.CREUtilities
{
    [TestFixture]
    public class ExpressiveILProcessorTest
    {
        [Test]
        public void EmitTest01()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();
                var action1Def2 =
                    methodTestClassDef.GetMethod(
                        "Action1",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action1Def2.Name = "Action12";
                methodTestClassDef.Methods.Add(action1Def2);
                {
                    var egen = new ExpressiveILProcessor(action1Def2);
                    egen.Emit(_ => ThrowException("Hello, World!!"));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret); // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action1Def2.Name,
                            typeof(Action12Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest02()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef2 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef2.Name = "Action2LocalVariable2";
                methodTestClassDef.Methods.Add(action2LocalVariableDef2);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef2);
                    int i = default(int);
                    egen.Emit(_ => _.Addloc(i, 100));
                    egen.Emit(_ => ThrowException("i.ToString() = {0}", i.ToString()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef2.Name,
                            typeof(Action2LocalVariable2Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest03()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef3 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef3.Name = "Action2LocalVariable3";
                methodTestClassDef.Methods.Add(action2LocalVariableDef3);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef3);
                    string s = default(string);
                    egen.Emit(_ => _.Addloc(s, new string('a', 10)));
                    egen.Emit(_ => ThrowException("s.ToString() = {0}", s.Substring(0, 5)));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef3.Name,
                            typeof(Action2LocalVariable3Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest04()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef4 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef4.Name = "Action2LocalVariable4";
                methodTestClassDef.Methods.Add(action2LocalVariableDef4);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef4);
                    string s = new string('a', 10);
                    egen.Emit(_ => _.Addloc(s, new string('a', 10)));
                    egen.Emit(_ => ThrowException("s.ToString() = {0}", s.ToUpper()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef4.Name,
                            typeof(Action2LocalVariable4Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest05()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef5 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef5.Name = "Action2LocalVariable5";
                methodTestClassDef.Methods.Add(action2LocalVariableDef5);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef5);
                    egen.Emit(_ => ThrowException(SR.Emit.OpCodes.Brtrue));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef5.Name,
                            typeof(Action2LocalVariable5Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest06()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef6 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef6.Name = "Action2LocalVariable6";
                methodTestClassDef.Methods.Add(action2LocalVariableDef6);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef6);
                    var stringBuilder = default(StringBuilder);
                    int one = default(int);
                    egen.Emit(_ => _.Addloc(one, 1));
                    egen.Emit(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("1 + 1 = {0}", one + 1)));
                    int i = default(int);
                    egen.Emit(_ => _.Addloc(i, default(int)));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("++i = {0}", _.AddOneDup(i))));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("i++ = {0}", _.DupAddOne(i))));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("--i = {0}", _.SubOneDup(i))));
                    egen.Emit(_ => ThrowException(stringBuilder.ToString()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef6.Name,
                            typeof(Action2LocalVariable6Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest07()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef7 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef7.Name = "Action2LocalVariable7";
                methodTestClassDef.Methods.Add(action2LocalVariableDef7);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef7);
                    var dynamicMethod = default(DynamicMethod);
                    egen.Emit(_ => _.Addloc(dynamicMethod, new DynamicMethod("dynamicMethod", typeof(string), new Type[] { typeof(int) }, true)));
                    var stringBuilder = default(StringBuilder);
                    egen.Emit(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Name = {0}", dynamicMethod.Name)));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Return Type = {0}", dynamicMethod.ReturnType)));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter Length = {0}", dynamicMethod.GetParameters().Length)));
                    egen.Emit(_ => stringBuilder.AppendFormat("{0}\r\n", string.Format("Parameter[0] Type = {0}", dynamicMethod.GetParameters()[0])));
                    egen.Emit(_ => ThrowException(stringBuilder.ToString()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef7.Name,
                            typeof(Action2LocalVariable7Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest08()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var func1Parameters2 =
                    methodTestClassDef.GetMethod(
                        "Func1Parameters",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { typeof(int) }).DuplicateWithoutBody();
                func1Parameters2.Name = "Func1Parameters2";
                methodTestClassDef.Methods.Add(func1Parameters2);
                {
                    var egen = new ExpressiveILProcessor(func1Parameters2);
                    int value = default(int);
                    egen.Emit(_ => _.Return(value + value * value));
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            func1Parameters2.Name,
                            typeof(Func1Parameters2Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest09()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var func1Parameters3 =
                    methodTestClassDef.GetMethod(
                        "Func1Parameters",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { typeof(int) }).DuplicateWithoutBody();
                func1Parameters3.Name = "Func1Parameters3";
                methodTestClassDef.Methods.Add(func1Parameters3);
                {
                    var egen = new ExpressiveILProcessor(func1Parameters3);
                    int value = default(int);
                    double d = default(double);
                    egen.Emit(_ => _.Addloc(d, 0d));
                    egen.Emit(_ => _.Return(value + value * (int)d));
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            func1Parameters3.Name,
                            typeof(Func1Parameters3Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest10()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef8 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef8.Name = "Action2LocalVariable8";
                methodTestClassDef.Methods.Add(action2LocalVariableDef8);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef8);
                    egen.Emit(_ => ThrowException("GetValue(10) = {0}", GetValue(10).ToString()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef8.Name,
                            typeof(Action2LocalVariable8Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest11()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef9 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef9.Name = "Action2LocalVariable9";
                methodTestClassDef.Methods.Add(action2LocalVariableDef9);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef9);
                    var ctor3 = default(ConstructorInfo);
                    egen.Emit(_ => _.Addloc(ctor3, typeof(System.Func<,>).MakeGenericType(typeof(String), typeof(String)).GetConstructor(
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                        null,
                                                        new Type[] 
                                                        { 
                                                            typeof(Object), 
                                                            typeof(IntPtr) 
                                                        }, null)));
                    var stringBuilder = default(StringBuilder);
                    egen.Emit(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    egen.Emit(_ => stringBuilder.AppendFormat("Name = {0}\r\n", ctor3.Name));
                    egen.Emit(_ => stringBuilder.AppendFormat("IsPublic = {0}\r\n", ctor3.IsPublic));
                    var parameterInfos = default(ParameterInfo[]);
                    egen.Emit(_ => _.Addloc(parameterInfos, ctor3.GetParameters()));
                    egen.Emit(_ => stringBuilder.AppendFormat("Parameter Count = {0}\r\n", parameterInfos.Length));
                    egen.Emit(_ => stringBuilder.AppendFormat("Parameter[0] = {0}\r\n", parameterInfos[0]));
                    egen.Emit(_ => stringBuilder.AppendFormat("Parameter[1] = {0}\r\n", parameterInfos[1]));
                    egen.Emit(_ => ThrowException(stringBuilder.ToString()));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef9.Name,
                            typeof(Action2LocalVariable9Tester)
                       );
            }));
        }




        [Test]
        public void EmitTest12()
        {
            UsingTempFile(tempFileName =>
            NewAppDomainTester.Using(newDomain =>
            {
                // modify ...
                var methodTestClassDef = typeof(MethodTestClass1).ToTypeDef();

                var action2LocalVariableDef10 =
                    methodTestClassDef.GetMethod(
                        "Action2LocalVariable",
                        BindingFlags.Instance | BindingFlags.Public,
                        new Type[] { }).DuplicateWithoutBody();
                action2LocalVariableDef10.Name = "Action2LocalVariable10";
                methodTestClassDef.Methods.Add(action2LocalVariableDef10);
                {
                    var egen = new ExpressiveILProcessor(action2LocalVariableDef10);
                    int i = 100;
                    egen.Eval(_ => _.Addloc(i), i);
                    egen.Emit(_ => ThrowException(i + i));
                    egen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                }

                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef10.Name,
                            typeof(Action2LocalVariable10Tester)
                       );
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

        static void UsingTempFile(Action<string> action)
        {
            string tempFileName = Path.GetFileName(FileSystem.GetTempFileName());
            try
            {
                action(tempFileName);
            }
            finally
            {
                try
                {
                    File.Delete(tempFileName);
                }
                catch
                {
                    // 2 度とアクセスされることはないため無視。
                }
            }
        }
    }
}
