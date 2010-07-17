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
        public static void ThrowException(string value)
        {
            throw new Exception(value);
        }

        public static void ThrowException(string value, object param)
        {
            throw new Exception(string.Format(value, param));
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
                var action1Def2Gen = new ExpressiveILProcessor(action1Def2);
                action1Def2Gen.Emit(_ => ThrowException("Hello, World!!"));
                action1Def2Gen.Direct.Emit(MC.Cil.OpCodes.Ret); // TODO: 後で移動。
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
                var action2LocalVariableDef2Gen = new ExpressiveILProcessor(action2LocalVariableDef2);
                int i = default(int);
                action2LocalVariableDef2Gen.Emit(_ => _.Addloc(i, 100));
                action2LocalVariableDef2Gen.Emit(_ => ThrowException("i.ToString() = {0}", _.Ldloc(i).ToString()));
                action2LocalVariableDef2Gen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
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
                var action2LocalVariableDef2Gen = new ExpressiveILProcessor(action2LocalVariableDef3);
                string s = default(string);
                action2LocalVariableDef2Gen.Emit(_ => _.Addloc(s, new string('a', 10)));
                action2LocalVariableDef2Gen.Emit(_ => ThrowException("s.ToString() = {0}", _.Ldloc(s).Substring(0, 5)));
                action2LocalVariableDef2Gen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
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
                var action2LocalVariableDef2Gen = new ExpressiveILProcessor(action2LocalVariableDef4);
                string s = new string('a', 10);
                action2LocalVariableDef2Gen.Emit(_ => _.Addloc(s, new string('a', 10)));
                action2LocalVariableDef2Gen.Emit(_ => ThrowException("s.ToString() = {0}", _.Ldloc(s).ToUpper()));
                action2LocalVariableDef2Gen.Direct.Emit(MC.Cil.OpCodes.Ret);    // TODO: 後で移動。
                methodTestClassDef.Module.Assembly.Write(tempFileName);

                return new NewAppDomainTesterParam(
                            Path.Combine(newDomain.BaseDirectory, tempFileName),
                            methodTestClassDef.FullName,
                            action2LocalVariableDef4.Name,
                            typeof(Action2LocalVariable4Tester)
                       );
            }));
        }
    }
}
