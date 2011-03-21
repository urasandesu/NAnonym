using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;
using SR = System.Reflection;
using NUnit.Framework;

namespace Test.Urasandesu.NAnonym.Cecil.ILTools
{
    [TestFixture]
    public class ReflectiveMethodDesigner2Test
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [SetUp]
        public void SetUp()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        [TearDown]
        public void TearDown()
        {
            TestHelper.TryDeleteFiles(".", "tmp*.dll");
            TestHelper.TryDeleteFiles(".", "tmp*.log");
        }

        const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
        const SR::MethodAttributes PublicHideBySig = SR::MethodAttributes.Public | SR::MethodAttributes.HideBySig;

        [Test]
        public void EmitTest01()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest01Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest01");

                emitTest01Gen.AddDefaultConstructor();

                var func1Parameters2 = emitTest01Gen.AddMethod("Func1Parameters2", PublicHideBySig, typeof(int), new Type[] { typeof(int) });
                func1Parameters2.ExpressBody2(
                gen =>
                {
                    var value = default(int);
                    var objValue = default(object);
                    var value2 = default(int?);
                    gen.Eval(() => Dsl.Allocate(value).As(Dsl.LoadArgument<int>(1)));
                    gen.Eval(() => Dsl.If(value != 20 && value != 30 && value != 40 && value != 50));
                    {
                        gen.Eval(() => Dsl.Allocate(objValue).As(value));
                        gen.Eval(() => Dsl.If(Dsl.Allocate(value2).As(objValue as int?) != null && value2 < 10));
                        {
                            gen.Eval(() => Dsl.Return(value - value * value + (int)value2));
                        }
                        gen.Eval(() => Dsl.Else());
                        {
                            gen.Eval(() => Dsl.Return(value + value * value * value));
                        }
                        gen.Eval(() => Dsl.EndIf());
                    }
                    gen.Eval(() => Dsl.ElseIf(value == 20));
                    {
                        gen.Eval(() => Dsl.Return(value));
                    }
                    gen.Eval(() => Dsl.ElseIf(value == 40));
                    {
                        gen.Eval(() => Dsl.Return(value ^ value ^ value));
                    }
                    gen.Eval(() => Dsl.Else());
                    {
                        gen.Eval(() => Dsl.Return(value == 30 ? value + value : value * value));
                    }
                    gen.Eval(() => Dsl.EndIf());
                });

                var ms = new MemoryStream();
                tempAssemblyDef.Write(ms);
                //tempAssemblyDef.Write(tempFileName);
                
                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.RawAssembly = ms.ToArray();
                //testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = emitTest01Gen.FullName;
                testInfo.MethodName = func1Parameters2.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        var result = default(int);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 10 });
                        Assert.AreEqual(1010, result);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 9 });
                        Assert.AreEqual(-63, result);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 20 });
                        Assert.AreEqual(20, result);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 30 });
                        Assert.AreEqual(60, result);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 40 });
                        Assert.AreEqual(40, result);
                        result = (int)target.Method.Invoke(target.Instance, new object[] { 50 });
                        Assert.AreEqual(2500, result);
                    };

                return testInfo;
            }));
        }

        [Test]
        public void EmitTest02()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest02Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest02");

                emitTest02Gen.AddDefaultConstructor();

                var action1Parameter0 = emitTest02Gen.AddMethod("Action1Parameter0", PublicHideBySig, typeof(void), Type.EmptyTypes);
                action1Parameter0.ExpressBody2(
                gen =>
                {
                    var writeLog = typeof(TestHelper).GetMethod("WriteLog", new Type[] { typeof(string), typeof(object[]) });
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "testtest", new object[] { } }));
                    var p1 = default(PropertyTestClass1);
                    var p1Ci = typeof(PropertyTestClass1).GetConstructor(Type.EmptyTypes);
                    gen.Eval(() => Dsl.Allocate(p1).As((PropertyTestClass1)p1Ci.Invoke(null)));
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "{0}", new object[] { p1 } }));
                    var p1ValueProperty = typeof(PropertyTestClass1).GetProperty("ValueProperty");
                    gen.Eval(() => p1ValueProperty.SetValue(p1, 10, null));
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "ValueProperty: {0}", new object[] { (int)p1ValueProperty.GetValue(p1, null) } }));
                    var p1ObjectProperty = typeof(PropertyTestClass1).GetProperty("ObjectProperty");
                    gen.Eval(() => p1ObjectProperty.SetValue(p1, "a", null));
                    gen.Eval(() => p1ObjectProperty.SetValue(p1, p1ObjectProperty.GetValue(p1, null), null));
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "ObjectProperty: {0}", new object[] { p1ObjectProperty.GetValue(p1, null) } }));
                    var f2 = default(FieldTestClass2);
                    gen.Eval(() => Dsl.Allocate(f2).As(new FieldTestClass2()));
                    var f2ValueField = typeof(FieldTestClass2).GetField("ValueField");
                    gen.Eval(() => f2ValueField.SetValue(f2, 30));
                    gen.Eval(() => f2ValueField.SetValue(f2, f2ValueField.GetValue(f2)));
                    gen.Eval(() => TestHelper.WriteLog("ValueField: {0}", f2ValueField.GetValue(f2)));
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "ValueField: {0}", new object[] { f2ValueField.GetValue(f2) } }));
                    var p2 = default(PropertyTestClass2);
                    var p2Ci = typeof(PropertyTestClass2).GetConstructor(new Type[] { typeof(int), typeof(string) });
                    gen.Eval(() => Dsl.Allocate(p2).As((PropertyTestClass2)p2Ci.Invoke(new object[] { p1ValueProperty.GetValue(p1, null), p1ObjectProperty.GetValue(p1, null) })));
                    gen.Eval(() => writeLog.Invoke(null, new object[] { "({0}, {1})", new object[] { p2.ValueProperty, p2.ObjectProperty } }));
                    var getValue = typeof(TestHelper).GetMethod("GetValue", new Type[] { typeof(int) });
                    var value = default(int);
                    gen.Eval(() => Dsl.Allocate(value).As((int)getValue.Invoke(null, new object[] { f2ValueField.GetValue(f2) })));
                });

                var ms = new MemoryStream();
                tempAssemblyDef.Write(ms);
                //tempAssemblyDef.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.RawAssembly = ms.ToArray();
                //testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = emitTest02Gen.FullName;
                testInfo.MethodName = action1Parameter0.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        target.Method.Invoke(target.Instance, new object[] { });
                        Assert.AreEqual(
@"testtest
Test.Urasandesu.NAnonym.Etc.PropertyTestClass1
ValueProperty: 10
ObjectProperty: a
ValueField: 30
ValueField: 30
(10, a)
", TestHelper.ReadLogToEnd());
                    };

                return testInfo;
            }));
        }

        [Test]
        public void EmitTest03()
        {
            TestHelper.UsingTempFile(tempFileName =>
            TestHelper.UsingNewDomain(() =>
            {
                var tempAssemblyNameDef = new AssemblyNameDefinition(Path.GetFileNameWithoutExtension(tempFileName), new Version("1.0.0.0"));
                var tempAssemblyDef = AssemblyDefinition.CreateAssembly(tempAssemblyNameDef, tempAssemblyNameDef.Name, ModuleKind.Dll);
                var emitTest03Gen = tempAssemblyDef.MainModule.AddType(tempAssemblyNameDef.Name + "." + "EmitTest03");

                emitTest03Gen.AddDefaultConstructor();

                var func1Parameter0 = emitTest03Gen.AddMethod("Func1Parameter0", PublicHideBySig, typeof(string), Type.EmptyTypes);
                func1Parameter0.ExpressBody2(
                gen =>
                {
                    var dm = default(DynamicMethod);
                    gen.Eval(() => Dsl.Allocate(dm).As(new DynamicMethod("DynamicMethod", typeof(string), null, true)));

                    var il = default(ILGenerator);
                    gen.Eval(() => Dsl.Allocate(il).As(dm.GetILGenerator()));

                    gen.ExpressInternally(() => il, typeof(string).ToTypeDecl(), null,
                    _gen =>
                    {
                        var f1StaticObjectField = typeof(FieldTestClass1).GetFieldStaticNonPublic("staticObjectField");
                        _gen.Eval(() => f1StaticObjectField.SetValue(null, "testtest"));
                        _gen.Eval(() => Dsl.Return(f1StaticObjectField.GetValue(null)));
                    });

                    var func = default(Func<string>);
                    gen.Eval(() => Dsl.Allocate(func).As((Func<string>)dm.CreateDelegate(typeof(Func<string>))));
                    gen.Eval(() => Dsl.Return(func()));
                });

                var ms = new MemoryStream();
                tempAssemblyDef.Write(ms);
                //tempAssemblyDef.Write(tempFileName);

                var testInfo = new NewDomainTestInfoWithScope(MethodBase.GetCurrentMethod().Name);
                testInfo.RawAssembly = ms.ToArray();
                //testInfo.AssemblyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempFileName);
                testInfo.TypeFullName = emitTest03Gen.FullName;
                testInfo.MethodName = func1Parameter0.Name;
                testInfo.TestVerifier =
                    target =>
                    {
                        var result = (string)target.Method.Invoke(target.Instance, new object[] { });
                        Assert.AreEqual("testtest", result);
                    };

                return testInfo;
            }));
        }
    }
}
