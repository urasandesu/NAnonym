using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Assert = Urasandesu.NAnonym.Test.Assert;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym;
using Test.Urasandesu.NAnonym.Etc;
using SR = System.Reflection;

namespace Test.Urasandesu.NAnonym.ILTools
{
    [TestFixture]
    public partial class MixinTest
    {
        [Test]
        public void ToTypeDefTest01()
        {
            var typeC = typeof(Type).ToTypeDef();
            var typeR = typeof(Type);
            Assert.AreEquivalent(typeC, typeR, Mixin.Equivalent);
        }


        [Test]
        public void ToTypeDefTest02()
        {
            var typeC = typeof(Type[]).ToTypeRef();
            var typeR = typeof(Type[]);
            Assert.AreEquivalent(typeC, typeR, Mixin.Equivalent);
        }


        [Test]
        public void GetMethodTest()
        {
            var typeC = typeof(Type).ToTypeDef();
            var typeR = typeof(Type);
            var getTypeFromHandleC =
                typeC.GetMethod(
                    "GetTypeFromHandle",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(RuntimeTypeHandle)
                    });
            var getTypeFromHandleR =
                typeR.GetMethod(
                    "GetTypeFromHandle",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new Type[] {
                        typeof(RuntimeTypeHandle)
                    },
                    null);
            Assert.AreEquivalent(getTypeFromHandleC, getTypeFromHandleR, Mixin.Equivalent);
        }



        [Test]
        public void GetMethodTest2()
        {
            var typeC = typeof(Type).ToTypeDef();
            var typeR = typeof(Type);
            var makeGenericTypeC =
                typeC.GetMethod(
                    "MakeGenericType",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(Type[])
                    });
            var makeGenericTypeR =
                typeR.GetMethod(
                    "MakeGenericType",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new Type[] {
                        typeof(Type[])
                    },
                    null);
            Assert.AreEquivalent(makeGenericTypeC, makeGenericTypeR, Mixin.Equivalent);
        }



        [Test]
        public void GetFieldTest()
        {
            var opCodesC = typeof(SR::Emit.OpCodes).ToTypeDef();
            var opCodesR = typeof(SR::Emit.OpCodes);
            var Ldarg_0C = opCodesC.GetField("Ldarg_0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var Ldarg_0R = opCodesR.GetField("Ldarg_0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            Assert.AreEquivalent(Ldarg_0C, Ldarg_0R, Mixin.Equivalent);
        }



        [Test]
        public void GetConstructorTest()
        {
            var dynamicMethodC = typeof(DynamicMethod).ToTypeDef();
            var dynamicMethodR = typeof(DynamicMethod);
            var dynamicMethodCtorC =
                dynamicMethodC.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[]{
                        typeof(String),
                        typeof(Type),
                        typeof(Type[])
                    }
                );
            var dynamicMethodCtorR =
                dynamicMethodR.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new Type[]{
                        typeof(String),
                        typeof(Type),
                        typeof(Type[])
                    },
                    null
                );
            Assert.AreEquivalent(dynamicMethodCtorC, dynamicMethodCtorR, Mixin.Equivalent);
        }
    }
}
