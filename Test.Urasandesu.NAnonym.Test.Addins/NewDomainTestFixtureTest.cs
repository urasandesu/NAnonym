using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.NAnonym.Test.Addins;
using NUnit.Core;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class NewDomainTestFixtureTest
    {
        [Test]
        public void Test1()
        {
            var testFixture = new NewDomainTestFixture(typeof(SampleSuiteClass));
            Assert.AreEqual(3, testFixture.TestCount);
            var testResult = testFixture.Run(new QueuingEventListener(), new NullFilter());
            Assert.IsTrue(testResult.IsSuccess);
        }

        [Test]
        public void Test2()
        {
            var testFixture = new NewDomainTestFixture(typeof(SampleNotSuiteClass));
            Assert.AreEqual(0, testFixture.TestCount);
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
    }
}
