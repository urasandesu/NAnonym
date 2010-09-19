using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class SampleNotSuiteClass
    {
        [Test]
        public void Test1()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [Test]
        public void Test2()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [Test]
        public void Test3()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test4()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test5()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test6()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }
    }
}
