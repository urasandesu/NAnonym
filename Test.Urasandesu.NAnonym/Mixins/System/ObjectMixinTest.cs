/* 
 * File: ObjectMixinTest.cs
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


using System;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class ObjectMixinTest
    {
        [Test]
        public void MemberwiseCloneTest_ShouldReturnShallowCopy()
        {
            // Arrange
            var a = new MemberwiseCloneMock();
            a.ValueMember = 1;
            a.ObjectMember = new object();

            // Act
            var b = (MemberwiseCloneMock)a.MemberwiseClone();

            // Assert
            Assert.AreNotSame(a, b);
            Assert.AreEqual(a.ValueMember, b.ValueMember);
            Assert.AreSame(a.ObjectMember, b.ObjectMember);
        }

        class MemberwiseCloneMock
        {
            public int ValueMember;
            public object ObjectMember;
        }
        
        [Test]
        public void SmartlyCloneTest_ShouldReturnOriginal_IfNonValueNonCloneableIsPassed()
        {
            // Arrange
            var a = new NonValueNonCloneableMock();
            a.ValueMember = 1;
            a.ObjectMember = new object();

            // Act
            var b = (NonValueNonCloneableMock)a.SmartlyClone();

            // Assert
            Assert.AreSame(a, b);
        }

        class NonValueNonCloneableMock
        {
            public int ValueMember;
            public object ObjectMember;
        }

        [Test]
        public void SmartlyCloneTest_ShouldReturnClone_IfCloneableIsPassed()
        {
            // Arrange
            var a = new CloneableMock();
            a.ValueMember = 1;
            a.ObjectMember = new object();

            // Act
            var b = (CloneableMock)a.SmartlyClone();

            // Assert
            Assert.AreNotSame(a, b);
            Assert.AreEqual(a.ValueMember, b.ValueMember);
            Assert.AreNotSame(a.ObjectMember, b.ObjectMember);
        }

        class CloneableMock : ICloneable
        {
            public int ValueMember;
            public object ObjectMember;

            public object Clone()
            {
                var clone = (CloneableMock)MemberwiseClone();
                clone.ObjectMember = ObjectMember.MemberwiseClone();
                return clone;
            }
        }

        [Test]
        public void SmartlyCloneTest_ShouldReturnClone_IfValueIsPassed()
        {
            // Arrange
            var a = new ValueMock();
            a.ValueMember = 1;
            a.ObjectMember = new object();

            // Act
            var b = (ValueMock)a.SmartlyClone();

            // Assert
            Assert.AreEqual(a.ValueMember, b.ValueMember);
            Assert.AreSame(a.ObjectMember, b.ObjectMember);
        }

        struct ValueMock
        {
            public int ValueMember;
            public object ObjectMember;
        }
    }
}
