using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonymousExtensions;
using Urasandesu.NAnonymousExtensions.Linq;

namespace Test.Urasandesu.NAnonymousExtensions.Linq
{
    [TestFixture]
    public class DelegateEqualityComparerTest
    {
        [Test]
        public void CompareTest01()
        {
            var comparer = new DelegateEqualityComparer<string>();
            Assert.AreEqual(true, comparer.Equals(null, null));
            Assert.AreEqual(false, comparer.Equals(null, "b"));
            Assert.AreEqual(false, comparer.Equals("a", null));
            Assert.AreEqual(false, comparer.Equals("a", "b"));
            Assert.AreEqual(true, comparer.Equals("a", "a"));
        }
    }
}
