using System;
using NUnit.Framework;
using Test.Urasandesu.NAnonym.DI;
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.DI;

namespace Test
{
    public class GlobalClassTest
    {
        static GlobalClassTest()
        {
            AppDomain.CurrentDomain.Inject<GlobalClass1>();
            AppDomain.CurrentDomain.Inject<GlobalClass2>();
            AppDomain.CurrentDomain.AcceptChanges();
        }

        [Test]
        public void PrintTest01()
        {
            var class1 = new Class1();
            var class2 = new Class2();
            
            // 実行
            string value = "aiueo";
            Console.WriteLine(class1.Print(value));
            Console.WriteLine(class2.Print(value));
            /*
             * Modified prefix at Class1.PrintModified prefix at Class2.Print aiueo Modified suffix at Class2.PrintModified suffix at Class1.Print
             * Modified prefix at Class2.Print aiueo Modified suffix at Class2.Print
             */
            //Assert.AreEqual(
            //    "Modified prefix at Class1.Print" +
            //    "Modified prefix at Class2.Print" +
            //    value +
            //    "Modified suffix at Class2.Print" +
            //    "Modified suffix at Class1.Print",
            //    class1.Print(value));

            //Assert.AreEqual(
            //    "Modified prefix at Class2.Print" +
            //    value +
            //    "Modified suffix at Class2.Print",
            //    class2.Print(value));
        }
    }
}
