/* 
 * File: DelegateEqualityComparerTest.cs
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

namespace Test.Urasandesu.NAnonym.Linq
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

        [Test]
        public void CompareTest02()
        {
            var comparer1 = EqualityComparer<int, long>.Default;
            Assert.AreEqual(false, comparer1.Equals(10, 10L));

            var comparer2 = EqualityComparer<int, int>.Default;
            Assert.AreEqual(true, comparer2.Equals(10, 10));

            var comparer3 = EqualityComparer<Class1, Class2>.Default;
            var class1 = new Class1() { Key = 10 };
            var class2 = new Class2() { ID = 10 };
            Assert.AreEqual(true, comparer3.Equals(class1, class2));
        }

        #region class Class1
        class Class1
        {
            public int Key { get; set; }

            public override bool Equals(object obj)
            {
                var class1 = default(Class1);
                var class2 = default(Class2);
                if ((class1 = obj as Class1) != null)
                {
                    return Key == class1.Key;
                }
                else if ((class2 = obj as Class2) != null)
                {
                    return Key == class2.ID;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return Key;
            }
        }
        #endregion

        #region class Class2
        class Class2
        {
            public int ID { get; set; }

            public override bool Equals(object obj)
            {
                var class1 = default(Class1);
                var class2 = default(Class2);
                if ((class1 = obj as Class1) != null)
                {
                    return ID == class1.Key;
                }
                else if ((class2 = obj as Class2) != null)
                {
                    return ID == class2.ID;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return ID;
            }
        }
        #endregion

        [Test]
        public void CompareTest03()
        {
            var comparer = new DelegateEqualityComparer<string, string>();
            Assert.AreEqual(true, comparer.Equals(null, null));
            Assert.AreEqual(false, comparer.Equals(null, "b"));
            Assert.AreEqual(false, comparer.Equals("a", null));
            Assert.AreEqual(false, comparer.Equals("a", "b"));
            Assert.AreEqual(true, comparer.Equals("a", "a"));
        }
    }
}

