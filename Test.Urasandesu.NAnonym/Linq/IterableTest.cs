/* 
 * File: IterableTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Test.Urasandesu.NAnonym.Linq
{
    [TestFixture]
    public class IterableTest
    {
        [Test]
        public void ForeachTest01()
        {
            int[] items1 = new int[] { 0, 1, 0 };
            items1.ForEach((item1, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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

            var results = items1.Cross(items2, comparer, merger).ToArray();
            Assert.AreEqual(1, results.Length);
            results.ForEach((item, index) =>
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

            var results = items1.Cross(items2, comparer, merger).ToArray();
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

            var results = items1.Cross(items2, comparer, merger).ToArray();
            Assert.AreEqual(2, results.Length);
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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
            results.ForEach((item, index) =>
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



        [Test]
        public void Hoge()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            int[] source = new int[1000000];
            source[10] = 1;
            source[100] = 2;
            source[1000] = 3;
            source[10000] = 4;
            source[100000] = 5;
            var results = new List<int>();

            results.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != default(int))
                {
                    results.Add(source[i]);
                }
            }
            stopwatch.Stop();
            Assert.AreEqual(5, results.Count);
            Console.WriteLine("Results: {0}, Time {1} ms", results.Dump(), stopwatch.ElapsedMilliseconds);

            results.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            foreach (var item in source)
            {
                if (item != default(int))
                {
                    results.Add(item);
                }
            }
            stopwatch.Stop();
            Assert.AreEqual(5, results.Count);
            Console.WriteLine("Results: {0}, Time {1} ms", results.Dump(), stopwatch.ElapsedMilliseconds);

            results.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            foreach (var item in source.Where(item => item != default(int)))
            {
                results.Add(item);
            }
            stopwatch.Stop();
            Assert.AreEqual(5, results.Count);
            Console.WriteLine("Results: {0}, Time {1} ms", results.Dump(), stopwatch.ElapsedMilliseconds);

            results.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            foreach (var item in source.WhereNotDefault())
            {
                results.Add(item);
            }
            stopwatch.Stop();
            Assert.AreEqual(5, results.Count);
            Console.WriteLine("Results: {0}, Time {1} ms", results.Dump(), stopwatch.ElapsedMilliseconds);
        }


        [Test]
        public void IndexOfTest01()
        {
            int[] source = new int[] { 1, 2, 3, 4, 5 };
            int target = 3;
            Assert.AreEqual(2, source.IndexOf(target));
        }

        [Test]
        public void IndexOfTest02()
        {
            var source = new[] { new { Key = 1, Value = "value1" }, new { Key = 2, Value = "value2" }, new { Key = 3, Value = "value3" } };
            var target = new { Key = 2, Value = "value2" };
            var equalityComparer = Iterable.CreateEqualityComparerNullable(target, (x, y) => x.Key == y.Key && x.Value == y.Value);
            Assert.AreEqual(1, source.IndexOf(target, equalityComparer));
        }

        [Test]
        public void EqualsExTest101()
        {
            int[] first = new int[] { 1, 2, 3, 4, 5, 6 };
            int[] second = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, first.Equivalent(second));
        }

        [Test]
        public void EqualsExTest201()
        {
            int[] first = new int[] { 1, 2, 3, 4, 5, 6 };
            string[] second = new string[] { "1", "2", "3", "4", "5", "6" };
            var equalityComparer = Iterable.CreateEqualityComparerNullable(default(int), default(string), (x, y) => x.ToString() == y);
            Assert.AreEqual(true, first.Equivalent(second, equalityComparer));
        }

        [Test]
        public void AddRangeToTest101()
        {
            var source = new int[] { 4, 5, 6 };
            var destination = new int[] { 1, 2, 3 };
            var expected = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(ref destination).Equivalent(expected));
        }

        [Test]
        public void AddRangeToTest102()
        {
            var source = new int[] { 4, 5, 6, 7 };
            var destination = new int[] { 1, 2, 3 };
            var expected = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(ref destination, 3).Equivalent(expected));
        }

        [Test]
        public void AddRangeToTest103()
        {
            var source = new int[] { 3, 4, 5, 6, 7 };
            var destination = new int[] { 1, 2, 3, 4 };
            var expected = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(1, ref destination, 3, 3).Equivalent(expected));
        }

        [Test]
        public void AddRangeToTest201()
        {
            var source = new TestCollection<int>() { 4, 5, 6 };
            var destination = new TestCollection<int>() { 1, 2, 3 };
            var expected = new TestCollection<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(destination).Equivalent(expected));
        }

        [Test]
        public void AddRangeToTest202()
        {
            var source = new TestCollection<int>() { 4, 5, 6, 7 };
            var destination = new TestCollection<int>() { 1, 2, 3 };
            var expected = new TestCollection<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(destination, 3).Equivalent(expected));
        }

        [Test]
        public void AddRangeToTest203()
        {
            var source = new TestCollection<int>() { 3, 4, 5, 6, 7 };
            var destination = new TestCollection<int>() { 1, 2, 3 };
            var expected = new TestCollection<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(true, source.AddRangeTo(1, destination, 3).Equivalent(expected));
        }

        class TestCollection<T> : ICollection<T>
        {
            List<T> list = new List<T>();

            #region ICollection<T> メンバ

            public void Add(T item)
            {
                list.Add(item);
            }

            public void Clear()
            {
                list.Clear();
            }

            public bool Contains(T item)
            {
                return list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                list.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return list.Count; }
            }

            public bool IsReadOnly
            {
                get { return ((ICollection<T>)list).IsReadOnly; }
            }

            public bool Remove(T item)
            {
                return list.Remove(item);
            }

            #endregion

            #region IEnumerable<T> メンバ

            public IEnumerator<T> GetEnumerator()
            {
                return list.GetEnumerator();
            }

            #endregion

            #region IEnumerable メンバ

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }
    }
}

