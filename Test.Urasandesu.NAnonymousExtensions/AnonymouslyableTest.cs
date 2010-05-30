using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonymousExtensions;

namespace Test.Urasandesu.NAnonymousExtensions
{
    [TestFixture]
    public class AnonymouslyableTest
    {
        [Test]
        public void CreateActionTest01()
        {
            var item = new { Key = 1, Value = "a" };
            var action = Anonymouslyable.CreateAction(item, _item =>
            {
                Assert.AreEqual(1, _item.Key);
                Assert.AreEqual("a", _item.Value);
            });

            action(item);
        }



        [Test]
        public void CreateActionTest02()
        {
            var item1 = new { Key = 1, Value = "a" };
            var item2 = new { Key = 2, Value = "b" };
            var action = Anonymouslyable.CreateAction(item1, item2, (_item1, _item2) =>
            {
                Assert.AreEqual(1, _item1.Key);
                Assert.AreEqual("a", _item1.Value);
                Assert.AreEqual(2, _item2.Key);
                Assert.AreEqual("b", _item2.Value);
            });

            action(item1, item2);
        }



        [Test]
        public void CreateFuncTest01()
        {
            var item = new { Key = 1, Value = "a" };
            var func = Anonymouslyable.CreateFunc(item, () =>
            {
                return new { Key = 2, Value = "b" };
            });

            var result = func();
            Assert.AreEqual(2, result.Key);
            Assert.AreEqual("b", result.Value);
        }



        [Test]
        public void CreateFuncTest02()
        {
            var item = new { Key = 1, Value = "a" };
            var func = Anonymouslyable.CreateFunc(item, item, _item =>
            {
                return new { Key = _item.Key + 1, Value = _item.Value + "b" };
            });

            var result = func(item);
            Assert.AreEqual(2, result.Key);
            Assert.AreEqual("ab", result.Value);
        }



        [Test]
        public void CreateFuncTest03()
        {
            var item1 = new { Key = 1, Value = "a" };
            var item2 = new { Key = 2, Value = "b" };
            var func = Anonymouslyable.CreateFunc(item1, item2, item1, (_item1, _item2) =>
            {
                return new { Key = _item1.Key + _item2.Key, Value = _item1.Value + _item2.Value };
            });

            var result = func(item1, item2);
            Assert.AreEqual(3, result.Key);
            Assert.AreEqual("ab", result.Value);
        }
    }
}
