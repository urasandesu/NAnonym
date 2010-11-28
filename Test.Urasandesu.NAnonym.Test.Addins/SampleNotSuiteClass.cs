/* 
 * File: SampleNotSuiteClass.cs
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
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class SampleNotSuiteClass
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [Test]
        public void Test2()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [Test]
        public void Test3()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test4()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test5()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test6()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }
    }
}

