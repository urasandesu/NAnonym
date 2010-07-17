using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;

namespace Test.Urasandesu.NAnonym.Linq
{
    [TestFixture]
    public class DelegateComparerTest
    {
        [Test]
        public void CompareTest01()
        {
            var comparer = new DelegateComparer<string>();
            Assert.AreEqual(0, comparer.Compare(null, null));
            Assert.AreEqual(-1, comparer.Compare(null, "b"));
            Assert.AreEqual(1, comparer.Compare("a", null));
            Assert.AreEqual(-1, comparer.Compare("a", "b"));
        }

        [Test]
        public void CompareTest02()
        {
            var comparer = new DelegateComparer<string>((x, y) => Comparer<string>.Default.Compare(x, y));
            Assert.AreEqual(0, comparer.Compare(null, null));
            Assert.AreEqual(-1, comparer.Compare(null, "b"));
            Assert.AreEqual(1, comparer.Compare("a", null));
            Assert.AreEqual(-1, comparer.Compare("a", "b"));
        }
    }
}
