using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    [TestFixture]
    public class GlobalClassTest
    {
        static GlobalClassTest()
        {
            DependencyUtil.RollbackGlobal();

            DependencyUtil.RegisterGlobal<GlobalClass1>();
            DependencyUtil.RegisterGlobal<GlobalClass2>();
            DependencyUtil.LoadGlobal();
        }

        [Test]
        public void PrintTest01()
        {
            var class1 = new Class1();
            var class2 = new Class2();
            string value = "aiueo";
            
            Assert.AreEqual(
                "Modified prefix at Class1.Print" +
                "Modified prefix at Class2.Print" +
                value +
                "Modified suffix at Class2.Print" +
                "Modified suffix at Class1.Print",
                class1.Print(value));

            Assert.AreEqual(
                "Modified prefix at Class2.Print" +
                value +
                "Modified suffix at Class2.Print",
                class2.Print(value));
        }
    }

    [TestFixture]
    public class GlobalClass3_1Test
    {
        static GlobalClass3_1Test()
        {
            DependencyUtil.RollbackGlobal();

            DependencyUtil.RegisterGlobal<GlobalClass3_1>();
            DependencyUtil.LoadGlobal();
        }

        [Test]
        public void AddTest01()
        {
            var class3 = new Class3();

            Assert.AreEqual(4, class3.Add(1, 1));
        }
    }

    [TestFixture]
    public class GlobalClass3_2Test
    {
        static GlobalClass3_2Test()
        {
            DependencyUtil.RollbackGlobal();

            DependencyUtil.RegisterGlobal<GlobalClass3_2>();
            DependencyUtil.LoadGlobal();
        }

        [Test]
        public void AddTest01()
        {
            var class3 = new Class3();

            Assert.AreEqual(2, class3.Add(1, 1));
            Assert.AreEqual(4, class3.Add(1, 1));
            Assert.AreEqual(6, class3.Add(1, 1));
        }
    }

}
