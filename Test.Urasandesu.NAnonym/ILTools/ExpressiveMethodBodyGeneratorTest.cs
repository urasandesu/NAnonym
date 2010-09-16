using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;
using SR = System.Reflection;

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
    }
}
