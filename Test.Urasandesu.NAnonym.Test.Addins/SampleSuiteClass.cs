using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Test;
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [NewDomainTestFixture]
    public class SampleSuiteClass : NewDomainTestBase
    {
        [NewDomainTestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainSetUp]
        public void SetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTearDown]
        public void TearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTest]
        public void Test1()
        {
            Assert.IsNotNull(Console);
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTest]
        public void Test2()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [NewDomainTest]
        public void Test3()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test4()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }

        public void Test5()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }

        public void Test6()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }
    }
}
