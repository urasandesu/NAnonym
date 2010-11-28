/* 
 * File: RequiredTest.cs
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

namespace Test.Urasandesu.NAnonym
{
    [TestFixture]
    public class RequiredTest
    {
        [Test]
        public void AllTest()
        {
            try
            {
                int a = 0;
                Required.NotDefault(a, () => a);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("a", e.ParamName);
            }

            try
            {
                object o = new object();
                Required.Default(o, () => o);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("o", e.ParamName);
            }

            try
            {
                object value = null;
                object destination = null;
                Required.JustOnce(value, ref destination, () => value);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }

            try
            {
                object value = new object();
                object destination = new object();
                Required.JustOnce(value, ref destination, () => value);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }

            {
                object value = new object();
                object destination = null;
                Assert.AreSame(value, Required.JustOnce(value, ref destination, () => value));
            }

            {
                int value = 10;
                int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                Assert.AreEqual(10, Required.Identical(value, array, _ => _.Count(), () => value));
            }

            try
            {
                int value = 10;
                int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
                Assert.AreEqual(10, Required.Identical(
                    value, array, _ => _.Aggregate((accumulate, source) => accumulate + source), () => value));
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Assert.AreEqual("value", e.ParamName);
            }
        }
    }
}

