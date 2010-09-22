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
                    gen.Eval(_ => _.Addloc(stringBuilder, new StringBuilder()));
                    gen.Eval(_ => stringBuilder.Append("Hello, World!! "));
                    gen.Eval(_ => stringBuilder.Append(_.ExpandAndLdarg<string>(valueParameterName)));
                    gen.Eval(_ => TestHelper.ThrowException(stringBuilder.ToString()));
                },
                valueParameterBuilder);

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
    }
}
