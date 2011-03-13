using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Test;
using Mono.Cecil;
using System.IO;
using SR = System.Reflection;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.System;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using Test.Urasandesu.NAnonym.Etc;
using System.Diagnostics;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;

namespace Test.Urasandesu.NAnonym.Cecil.ILTools
{
    [NewDomainTestFixture]
    public class ReflectiveMethodDesigner2Test : NewDomainTestBase
    {
        [NewDomainTestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [NewDomainTestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [NewDomainSetUp]
        public void SetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [NewDomainTearDown]
        public void TearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
        const SR::MethodAttributes PublicHideBySig = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig;

        [NewDomainTest]
        public void EmitTest01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest01Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest01");

                emitTest01Gen.AddDefaultConstructor();

                //var func1Parameters2 = emitTest01Gen.AddMethod("Func1Parameters2", PublicHideBySig, typeof(int), new Type[] { typeof(int) });
                var func1Parameters2 = emitTest01Gen.AddMethod("Func1Parameters2", PublicHideBySig, typeof(int), new Type[] { typeof(int) });
                func1Parameters2.ExpressBody2(
                gen =>
                {
                    var value = default(int);
                    //var objValue = default(object);
                    //var value2 = default(int?);
                    gen.Eval(() => Dsl.Allocate(value).As(Dsl.LoadArgument<int>(1)));
                    //gen.Eval(() => Dsl.If(value != 20 && value != 30 && value != 40 && value != 50));
                    //{
                    //    gen.Eval(() => Dsl.Allocate(objValue).As(value));
                    //    gen.Eval(() => Dsl.If(Dsl.Allocate(value2).As(objValue as int?) != null));
                    //    {
                    //        gen.Eval(() => Dsl.Return(value + value * value + (int)value2));
                    //    }
                    //    gen.Eval(() => Dsl.Else());
                    //    {
                    gen.Eval(() => Dsl.Return(value + value * value * value));
                    //    }
                    //    gen.Eval(() => Dsl.EndIf());
                    //}
                    //gen.Eval(() => Dsl.ElseIf(value == 20));
                    //{
                    //    gen.Eval(() => Dsl.Return(value));
                    //}
                    //gen.Eval(() => Dsl.ElseIf(value == 40));
                    //{
                    //    gen.Eval(() => Dsl.Return(value ^ value ^ value));
                    //}
                    //gen.Eval(() => Dsl.Else());
                    //{
                    //    gen.Eval(() => Dsl.Return(value == 30 ? value + value : value * value));
                    //}
                    //gen.Eval(() => Dsl.EndIf());
                });

                var ms = new MemoryStream();
                //tempAssemblyDef.Write(ms);
                tempAssemblyDef.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                //testInfo.RawAssembly = ms.ToArray();
                testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = emitTest01Gen.FullName;
                testInfo.MethodName = func1Parameters2.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        //Assert.AreEqual("testtest", target.Method.Invoke(target.Instance, new object[] { }));
                        var result = default(int);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 10 });
                        Assert.AreEqual(1010, result);
                        //result = (int)target.Method.Invoke(target.Instance, new object[] { 20 });
                        //Assert.AreEqual(20, result);
                        //result = (int)target.Method.Invoke(target.Instance, new object[] { 30 });
                        //Assert.AreEqual(60, result);
                        //result = (int)target.Method.Invoke(target.Instance, new object[] { 40 });
                        //Assert.AreEqual(40, result);
                        //result = (int)target.Method.Invoke(target.Instance, new object[] { 50 });
                        //Assert.AreEqual(2500, result);
                    };

                return testInfo;
            }));
        }
    }
}
