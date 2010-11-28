/* 
 * File: SampleSuiteClass.cs
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
using Urasandesu.NAnonym.Test;
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [NewDomainTestFixture]
    public class SampleSuiteClass : NewDomainTestBase
    {
        [NewDomainTestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainSetUp]
        public void SetUp()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTearDown]
        public void TearDown()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTest]
        public void Test1()
        {
            Assert.IsNotNull(Console);
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
            Assert.AreEqual(typeof(SampleSuiteClass).FullName, AppDomain.CurrentDomain.FriendlyName);
        }

        [NewDomainTest]
        public void Test2()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        [NewDomainTest]
        public void Test3()
        {
            Console.WriteLine("Run: {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void Test4()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }

        public void Test5()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }

        public void Test6()
        {
            Assert.Fail(string.Format("This method must not run: {0}", MethodBase.GetCurrentMethod().Name));
        }
    }
}

