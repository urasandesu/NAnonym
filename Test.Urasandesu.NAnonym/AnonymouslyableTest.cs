/* 
 * File: AnonymouslyableTest.cs
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
using System.IO;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;
using System.Xml.Linq;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace Test.Urasandesu.NAnonym
{
    [TestFixture]
    public class AnonymouslyableTest
    {
        [Test]
        public void CreateActionTest01()
        {
            var item = new { Key = 1, Value = "a" };
            var action = Anonymable.CreateAction(item, _item =>
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
            var action = Anonymable.CreateAction(item1, item2, (_item1, _item2) =>
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
            var func = Anonymable.CreateFunc(item, () =>
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
            var func = Anonymable.CreateFunc(item, item, _item =>
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
            var func = Anonymable.CreateFunc(item1, item2, item1, (_item1, _item2) =>
            {
                return new { Key = _item1.Key + _item2.Key, Value = _item1.Value + _item2.Value };
            });

            var result = func(item1, item2);
            Assert.AreEqual(3, result.Key);
            Assert.AreEqual("ab", result.Value);
        }


        #region Test classes for NotDefault
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
            var runner = Anonymable.CreateFunc(a, default(string), 
                _a => _a.NotDefault(_ => _
                        .B).NotDefault(_ => _
                            .C).NotDefault(_ => _
                                .D).NotDefault(_ => _
                                    .Value).NotDefault(_ => _
                                        .Trim()));

            Assert.AreEqual("a", runner(a));
        }



        [Test]
        public void IfNotNullTest02()
        {
            var a = default(A);
            var runner = Anonymable.CreateFunc(a, default(string),
                _a => _a.NotDefault(_ => _
                        .B).NotDefault(_ => _
                            .C).NotDefault(_ => _
                                .D).NotDefault(_ => _
                                    .Value).NotDefault(_ => _
                                        .Trim()));

            Assert.AreEqual(null, runner(a));
        }



        [Test]
        public void IfNotNullTest03()
        {
            var a = new A() { B = new B() { C = new C() { D = null } } };
            var runner = Anonymable.CreateFunc(a, default(string),
                _a => _a.NotDefault(_ => _
                        .B).NotDefault(_ => _
                            .C).NotDefault(_ => _
                                .D).NotDefault(_ => _
                                    .Value).NotDefault(_ => _
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
            Func<DirectoryInfo, IEnumerable<DirectoryInfo>> nextDirectoryInfo = directoryInfo => directoryInfo.GetDirectories();
            Func<DirectoryInfo, IEnumerable<FileInfo>> nextFileInfo = directoryInfo => directoryInfo.GetFiles();

            var fileInfos = directoryInfoNAnonymousExtensions.Recursion(nextDirectoryInfo, nextFileInfo).ToArray();
            Assert.IsTrue(0 < fileInfos.Length);
            foreach (var fileInfo in fileInfos)
            {
                Console.WriteLine(fileInfo.Name);
            }
        }



        [Test]
        public void RecursionTest03()
        {
            var contacts =
                new XElement("Contacts",
                    new XElement("Contact",
                        new XElement("Name", "Patrick Hines"),
                        new XComment("comment0"),
                        new XElement("Phone", "206-555-0144",
                            new XAttribute("Type", "Home")),
                        new XElement("phone", "425-555-0145",
                            new XAttribute("Type", "Work")),
                        new XElement("Address",
                            new XElement("Street1", "123 Main St"),
                            new XElement("City", "Mercer Island"),
                            new XComment("comment1"),
                            new XElement("State", "WA"),
                            new XElement("Postal", "68042")
                        )
                    )
                );

            Func<XElement, IEnumerable<XElement>> nextElement = 
                element => element.Nodes().Select(node => node as XElement).WhereNotDefault();
            var elements = contacts.Recursion(nextElement).ToArray();
            Assert.AreEqual(9, elements.Length);
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
            }
        }



        [Test]
        public void RecursionTest04()
        {
            var doc = new XmlDocument();

            var contacts = doc.CreateElement("Contacts");
            doc.AppendChild(contacts);

            var contact = doc.CreateElement("Contact");
            contacts.AppendChild(contact);

            var name = doc.CreateElement("Name");
            name.InnerText = "Patrick Hines";
            contact.AppendChild(name);

            var comment0 = doc.CreateComment("Comment0");
            contact.AppendChild(comment0);

            var phone = doc.CreateElement("Phone");
            phone.InnerText = "206-555-0144";

            var type = doc.CreateAttribute("Type");
            type.Value = "Home";
            phone.Attributes.Append(type);
            contact.AppendChild(phone);

            phone = doc.CreateElement("Phone");
            phone.InnerText = "425-555-0145";

            type = doc.CreateAttribute("Type");
            type.Value = "Work";
            phone.Attributes.Append(type);
            contact.AppendChild(phone);

            var address = doc.CreateElement("Address");
            contact.AppendChild(address);

            var street1 = doc.CreateElement("Street1");
            street1.InnerText = "123 Main St";
            address.AppendChild(street1);

            var city = doc.CreateElement("City");
            city.InnerText = "Mercer Island";
            address.AppendChild(city);

            var comment1 = doc.CreateComment("Comment1");
            address.AppendChild(comment1);

            var state = doc.CreateElement("State");
            state.InnerText = "WA";
            address.AppendChild(state);

            var postal = doc.CreateElement("Postal");
            postal.InnerText = "68042";
            address.AppendChild(postal);

            Func<XmlElement, IEnumerable<XmlElement>> nextElement = 
                element => element.ChildNodes.Cast<XmlNode>().Select(node => node as XmlElement).WhereNotDefault();
            var elements = contacts.Recursion(nextElement).ToArray();
            Assert.AreEqual(9, elements.Length);
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
            }
        }



        /*
        [Test]
        public void Hoge()
        {
            var typeRef = new TypeRef<int>();
        }*/
    }
}

