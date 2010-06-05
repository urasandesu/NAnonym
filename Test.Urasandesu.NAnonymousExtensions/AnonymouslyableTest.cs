using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
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


        #region Test classes for IfNotNull
        class A
        {
            public B B { get; set; }
        }

        class B
        {
            public C C { get; set; }
        }

        class C
        {
            public D D { get; set; }
        }

        class D
        {
            public string Value { get; set; }
        }
        #endregion

        [Test]
        public void IfNotNullTest01()
        {
            var a = new A() { B = new B() { C = new C() { D = new D() { Value = "      a " } } } };
            var runner = Anonymouslyable.CreateFunc(a, default(string), 
                _a => _a.IfNotNull(_ => _
                        .B).IfNotNull(_ => _
                            .C).IfNotNull(_ => _
                                .D).IfNotNull(_ => _
                                    .Value).IfNotNull(_ => _
                                        .Trim()));

            Assert.AreEqual("a", runner(a));
        }



        [Test]
        public void IfNotNullTest02()
        {
            var a = default(A);
            var runner = Anonymouslyable.CreateFunc(a, default(string),
                _a => _a.IfNotNull(_ => _
                        .B).IfNotNull(_ => _
                            .C).IfNotNull(_ => _
                                .D).IfNotNull(_ => _
                                    .Value).IfNotNull(_ => _
                                        .Trim()));

            Assert.AreEqual(null, runner(a));
        }



        [Test]
        public void IfNotNullTest03()
        {
            var a = new A() { B = new B() { C = new C() { D = null } } };
            var runner = Anonymouslyable.CreateFunc(a, default(string),
                _a => _a.IfNotNull(_ => _
                        .B).IfNotNull(_ => _
                            .C).IfNotNull(_ => _
                                .D).IfNotNull(_ => _
                                    .Value).IfNotNull(_ => _
                                        .Trim()));

            Assert.AreEqual(null, runner(a));
        }



        [Test]
        public void RecursionTest01()
        {
            var directoryOutput = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directoryBin = Path.GetDirectoryName(directoryOutput);
            var directoryTUNAE = Path.GetDirectoryName(directoryBin);
            var directoryNAnonymousExtensions = Path.GetDirectoryName(directoryTUNAE);

            var directoryInfoNAnonymousExtensions = new DirectoryInfo(directoryNAnonymousExtensions);
            Func<DirectoryInfo, IEnumerable<DirectoryInfo>> nextDirectoryInfo = directoryInfo => directoryInfo.GetDirectories();

            var directoryInfos = directoryInfoNAnonymousExtensions.Recursion(nextDirectoryInfo).ToArray();
            Assert.IsTrue(0 < directoryInfos.Length);
            foreach (var directoryInfo in directoryInfos)
            {
                Console.WriteLine(directoryInfo.Name);
            }
        }



        [Test]
        public void RecursionTest02()
        {
            var directoryOutput = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directoryBin = Path.GetDirectoryName(directoryOutput);
            var directoryTUNAE = Path.GetDirectoryName(directoryBin);
            var directoryNAnonymousExtensions = Path.GetDirectoryName(directoryTUNAE);

            var directoryInfoNAnonymousExtensions = new DirectoryInfo(directoryNAnonymousExtensions);
            Func<DirectoryInfo, IEnumerable<FileInfo>> nextFileInfo = directoryInfo => directoryInfo.GetFiles();
            Func<DirectoryInfo, IEnumerable<DirectoryInfo>> nextDirectoryInfo = directoryInfo => directoryInfo.GetDirectories();

            var fileInfos = directoryInfoNAnonymousExtensions.Recursion(nextFileInfo, nextDirectoryInfo).ToArray();
            Assert.IsTrue(0 < fileInfos.Length);
            foreach (var fileInfo in fileInfos)
            {
                Console.WriteLine(fileInfo.Name);
            }
        }
    }
}
