using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.NAnonym.Test.Addins;
using NUnit.Core;
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class NewDomainTestFixtureTest
    {
        [Test]
        public void Test1()
        {
            var testFixture = new NewDomainTestFixtureAccessor(typeof(SampleSuiteClass));
            Assert.AreEqual(3, testFixture.TestCount);
            Assert.AreEqual(1, testFixture.GetSetUpMethods().Length);
            Assert.AreEqual(1, testFixture.GetTearDownMethods().Length);
            Assert.AreEqual(1, testFixture.GetTestFixtureSetUpMethods().Length);
            Assert.AreEqual(1, testFixture.GetTestFixtureTearDownMethods().Length);
            var testResult = testFixture.Run(new QueuingEventListener(), new NullFilter());
            Assert.IsTrue(testResult.IsSuccess);
        }

        [Test]
        public void Test2()
        {
            var testFixture = new NewDomainTestFixtureAccessor(typeof(SampleNotSuiteClass));
            Assert.AreEqual(0, testFixture.TestCount);
            Assert.AreEqual(0, testFixture.GetSetUpMethods().Length);
            Assert.AreEqual(0, testFixture.GetTearDownMethods().Length);
            Assert.AreEqual(0, testFixture.GetTestFixtureSetUpMethods().Length);
            Assert.AreEqual(0, testFixture.GetTestFixtureTearDownMethods().Length);
        }





        [Serializable]
        class NullFilter : TestFilter
        {
            public override bool Match(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }

            public override bool Pass(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }
        }

        class NewDomainTestFixtureAccessor : NewDomainTestFixture
        {
            public NewDomainTestFixtureAccessor(Type fixtureType)
                : base(fixtureType)
            {
            }

            public MethodInfo[] GetTestFixtureSetUpMethods()
            {
                return fixtureSetUpMethods;
            }

            public MethodInfo[] GetTestFixtureTearDownMethods()
            {
                return fixtureTearDownMethods;
            }
        }
    }
}
