using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.NAnonym.Test;
using Urasandesu.NAnonym.Test.Addins;
using Assert = Urasandesu.NAnonym.Test.Assert;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class NewDomainTestSuiteBuilderTest
    {
        [Test]
        public void CanBuildFromTest1()
        {
            var suiteBuilder = new NewDomainTestSuiteBuilder();
            Assert.IsTrue(suiteBuilder.CanBuildFrom(typeof(SampleSuiteClass)));
        }


        [Test]
        public void CanBuildFromTest2()
        {
            var suiteBuilder = new NewDomainTestSuiteBuilder();
            Assert.IsFalse(suiteBuilder.CanBuildFrom(typeof(SampleNotSuiteClass)));
        }


        [Test]
        public void BuildFromTest1()
        {
            var suiteBuilder = new NewDomainTestSuiteBuilder();
            var test = suiteBuilder.BuildFrom(typeof(SampleSuiteClass));
            Assert.IsNotNull(test);
            Assert.AreEqual(typeof(NewDomainTestFixture), test.GetType());
        }
    }
}
