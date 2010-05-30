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
    public class IterableTest
    {
        [Test]
        public void ForeachTest01()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            items1.Foreach((item1, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item1);
                        break;
                    case 1:
                        Assert.AreEqual(1, item1);
                        break;
                    case 2:
                        Assert.AreEqual(0, item1);
                        break;
                    default:
                        break;
                }
            });
        }



        [Test]
        public void NegateTest01()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 1 };

            var results = items1.Negate(items2).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(0, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void NegateTest02()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 2 };

            var results = items1.Negate(items2).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(1, item);
                        break;
                    case 2:
                        Assert.AreEqual(0, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void NegateTest03()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 0, 0 };

            var results = items1.Negate(items2).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void NegateTest04()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 1, Value = "bb" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Negate(items2, comparer).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void NegateTest05()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 2, Value = "d" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Negate(items2, comparer).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    case 2:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void NegateTest06()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 0, Value = "aa" }, new { Key = 0, Value = "cc" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Negate(items2, comparer).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }

        
        
        [Test]
        public void ReplenishTest01()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 1 };

            var results = items1.Replenish(items2).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(1, item);
                        break;
                    case 2:
                        Assert.AreEqual(0, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void ReplenishTest02()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 2 };

            var results = items1.Replenish(items2).ToArray();
            Assert.AreEqual(4, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(1, item);
                        break;
                    case 2:
                        Assert.AreEqual(0, item);
                        break;
                    case 3:
                        Assert.AreEqual(2, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void ReplenishTest03()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 0, 0 };

            var results = items1.Replenish(items2).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(1, item);
                        break;
                    case 2:
                        Assert.AreEqual(0, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void ReplenishTest04()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 1, Value = "bb" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Replenish(items2, comparer).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    case 2:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void ReplenishTest05()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 2, Value = "d" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Replenish(items2, comparer).ToArray();
            Assert.AreEqual(4, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    case 2:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    case 3:
                        Assert.AreEqual(2, item.Key);
                        Assert.AreEqual("d", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void ReplenishTest06()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 0, Value = "aa" }, new { Key = 0, Value = "cc" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Replenish(items2, comparer).ToArray();
            Assert.AreEqual(3, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    case 2:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossTest01()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 1 };
            Func<int, int, int> merger = (item1, item2) => item1;

            var results = items1.Cross(items2, merger).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossTest02()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 2 };
            Func<int, int, int> merger = (item1, item2) => item1;

            var results = items1.Cross(items2, merger).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossTest03()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            int[] items2 = new int[] { 0, 0 };
            Func<int, int, int> merger = (item1, item2) => item1;

            var results = items1.Cross(items2, merger).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item);
                        break;
                    case 1:
                        Assert.AreEqual(0, item);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossTest04()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 1, Value = "bb" } };
            var merger = Anonymouslyable.CreateFunc(items1.FirstOrDefault(), items2.FirstOrDefault(), items1.FirstOrDefault(),
                (item1, item2) => new { Key = item2.Key, Value = item1.Value });
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Cross(items2, merger, comparer).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossTest05()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 2, Value = "d" } };
            var merger = Anonymouslyable.CreateFunc(items1.FirstOrDefault(), items2.FirstOrDefault(), items1.FirstOrDefault(),
                (item1, item2) => new { Key = item2.Key, Value = item1.Value });
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Cross(items2, merger, comparer).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossTest06()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 0, Value = "aa" }, new { Key = 0, Value = "cc" } };
            var merger = Anonymouslyable.CreateFunc(items1.FirstOrDefault(), items2.FirstOrDefault(), items1.FirstOrDefault(),
                (item1, item2) => new { Key = item2.Key, Value = item1.Value });
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.Cross(items2, merger, comparer).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossLeftTest01()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(1, "bb") };

            var results = items1.CrossLeft(items2).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossLeftTest02()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(2, "d") };

            var results = items1.CrossLeft(items2).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossLeftTest03()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(0, "aa"), new TestItem(0, "cc") };

            var results = items1.CrossLeft(items2).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossLeftTest04()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 1, Value = "bb" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossLeft(items2, comparer).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("b", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossLeftTest05()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 2, Value = "d" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossLeft(items2, comparer).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossLeftTest06()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 0, Value = "aa" }, new { Key = 0, Value = "cc" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossLeft(items2, comparer).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("a", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("c", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossRightTest01()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(1, "bb") };

            var results = items1.CrossRight(items2).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("bb", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossRightTest02()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(2, "d") };

            var results = items1.CrossRight(items2).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossRightTest03()
        {
            var items1 = new TestItem[] { new TestItem(0, "a"), new TestItem(1, "b"), new TestItem(0, "c") };
            var items2 = new TestItem[] { new TestItem(0, "aa"), new TestItem(0, "cc") };

            var results = items1.CrossRight(items2).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("aa", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("aa", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossRightTest04()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 1, Value = "bb" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossRight(items2, comparer).ToArray();
            Assert.AreEqual(1, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(1, item.Key);
                        Assert.AreEqual("bb", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }



        [Test]
        public void CrossRightTest05()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 2, Value = "d" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossRight(items2, comparer).ToArray();
            Assert.AreEqual(0, results.Length);
        }



        [Test]
        public void CrossRightTest06()
        {
            var items1 = new[] { new { Key = 0, Value = "a" }, new { Key = 1, Value = "b" }, new { Key = 0, Value = "c" } };
            var items2 = new[] { new { Key = 0, Value = "aa" }, new { Key = 0, Value = "cc" } };
            var comparer = Iterable.CreateEqualityComparer(
                items1.FirstOrDefault(), item => item.Key, (item1, item2) => item1.Key == item2.Key);

            var results = items1.CrossRight(items2, comparer).ToArray();
            Assert.AreEqual(2, results.Length);
            results.Foreach((item, index) =>
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("aa", item.Value);
                        break;
                    case 1:
                        Assert.AreEqual(0, item.Key);
                        Assert.AreEqual("aa", item.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            });
        }


        // HACK: こういうクラスを書かなくてもいいようにするには、コード自動生成しか無さげ。また今度。
        class TestItem
        {
            public int Key { get; protected set; }
            public string Value { get; protected set; }

            public TestItem(int key, string value)
            {
                Key = key;
                Value = value;
            }

            public override bool Equals(object obj)
            {
                TestItem that = null;
                return obj == null ? false : (that = obj as TestItem) == null ? false : Key == that.Key;
            }

            public override int GetHashCode()
            {
                return Key;
            }
        }
    }
}
