using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
//using Urasandesu.NAnonym.DI;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DI;
//using DependencyUtil = Urasandesu.NAnonym.Cecil.DI.DependencyUtil;

namespace Test.Urasandesu.NAnonym.Cecil.DI
{
    [TestFixture]
    public class GlobalClassTest
    {
        static GlobalClassTest()
        {
            DependencyUtil.Setup<GlobalClass1>();
            DependencyUtil.Setup<GlobalClass2>();
            DependencyUtil.Load();

            // TODO: Commit と Rollback の実装。
            // Commit と Rollback できるようにしておけると理想。
            // こんな風に↓
            // AppDomain.CurrentDomain.BeginEdit();
            // AppDomain.CurrentDomain.Inject<GlobalClass1>();
            // AppDomain.CurrentDomain.Inject<GlobalClass2>();
            // AppDomain.CurrentDomain.AcceptChanges();
            // System.Data.DataRow クラスのメソッドが参考になりそう。
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
}
