/* 
 * File: LambdaEqualityComparerTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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


using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Urasandesu.NAnonym.Collections.Generic;

namespace Test.Urasandesu.NAnonym.Collections.Generic
{
    [TestFixture]
    public class LambdaEqualityComparerTest
    {
        [Test]
        public void Constructor_should_use_default_equality()
        {
            // Arrange
            var getHashCodeCalled = false;
            var equalsCalled = false;
            var mock = new MockObject();
            mock.GetHashCodeProvider = () => { getHashCodeCalled = true; return 0; };
            mock.EqualsProvider = obj => { equalsCalled = true; return true; };


            // Act
            var comparer = new LambdaEqualityComparer<MockObject>();
            var hashCode = comparer.GetHashCode(mock);
            var isEqual = comparer.Equals(mock, mock);


            // Assert
            Assert.IsTrue(getHashCodeCalled);
            Assert.IsTrue(equalsCalled);
            Assert.AreEqual(0, hashCode);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ConstructorFuncOfTOfTOfbool_should_use_specified_Equals_and_default_GetHashCode()
        {
            // Arrange
            var getHashCodeCalled = false;
            var equalsCalled = false;
            var mock = new MockObject();
            mock.GetHashCodeProvider = () => { getHashCodeCalled = true; return 0; };
            mock.EqualsProvider = obj => { equalsCalled = true; return true; };


            // Act
            var comparer = new LambdaEqualityComparer<MockObject>((_1, _2) => true);
            var hashCode = comparer.GetHashCode(mock);
            var isEqual = comparer.Equals(mock, mock);


            // Assert
            Assert.IsTrue(getHashCodeCalled);
            Assert.IsFalse(equalsCalled);
            Assert.AreEqual(0, hashCode);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ConstructorFuncOfTOfint_should_use_default_Equals_and_specified_GetHashCode()
        {
            // Arrange
            var getHashCodeCalled = false;
            var equalsCalled = false;
            var mock = new MockObject();
            mock.GetHashCodeProvider = () => { getHashCodeCalled = true; return 0; };
            mock.EqualsProvider = obj => { equalsCalled = true; return true; };


            // Act
            var comparer = new LambdaEqualityComparer<MockObject>(_ => 0);
            var hashCode = comparer.GetHashCode(mock);
            var isEqual = comparer.Equals(mock, mock);


            // Assert
            Assert.IsFalse(getHashCodeCalled);
            Assert.IsTrue(equalsCalled);
            Assert.AreEqual(0, hashCode);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ConstructorFuncOfTOfTOfboolFuncOfTOfint_should_use_specified_Equals_and_specified_GetHashCode()
        {
            // Arrange
            var getHashCodeCalled = false;
            var equalsCalled = false;
            var mock = new MockObject();
            mock.GetHashCodeProvider = () => { getHashCodeCalled = true; return 0; };
            mock.EqualsProvider = obj => { equalsCalled = true; return true; };


            // Act
            var comparer = new LambdaEqualityComparer<MockObject>((_1, _2) => true, _ => 0);
            var hashCode = comparer.GetHashCode(mock);
            var isEqual = comparer.Equals(mock, mock);


            // Assert
            Assert.IsFalse(getHashCodeCalled);
            Assert.IsFalse(equalsCalled);
            Assert.AreEqual(0, hashCode);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ConstructorFuncOfTOfObject_should_use_specified_key_equality()
        {
            // Arrange
            var getHashCodeCalled = new List<bool>();
            var equalsCalled = new List<bool>();
            var mock1 = new MockParent();
            mock1.Key.GetHashCodeProvider = () => { getHashCodeCalled.Add(true); return 0; };
            mock1.Key.EqualsProvider = obj => { equalsCalled.Add(true); return obj.Equals(null); };

            var mock2 = new MockParent();
            mock2.Key.GetHashCodeProvider = () => { getHashCodeCalled.Add(true); return 0; };
            mock2.Key.EqualsProvider = obj => { equalsCalled.Add(true); return true; };


            // Act
            var comparer = new LambdaEqualityComparer<MockParent>(_ => _.Key);
            var hashCode = comparer.GetHashCode(mock1);
            var isEqual = comparer.Equals(mock1, mock2);


            // Assert
            CollectionAssert.AreEqual(new[] { true }, getHashCodeCalled);
            CollectionAssert.AreEqual(new[] { true, true }, equalsCalled);
            Assert.AreEqual(0, hashCode);
            Assert.IsTrue(isEqual);
        }



        class MockParent
        {
            public MockObject Key = new MockObject();
        }

        class MockObject
        {
            public Func<int> GetHashCodeProvider { get; set; }
            public override int GetHashCode()
            {
                return GetHashCodeProvider == null ? base.GetHashCode() : GetHashCodeProvider();
            }

            public Func<object, bool> EqualsProvider { get; set; }
            public override bool Equals(object obj)
            {
                return EqualsProvider == null ? base.Equals(obj) : EqualsProvider(obj);
            }
        }
    }
}
