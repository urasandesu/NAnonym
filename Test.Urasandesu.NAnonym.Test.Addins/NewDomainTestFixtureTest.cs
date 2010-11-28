/* 
 * File: NewDomainTestFixtureTest.cs
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
using Urasandesu.NAnonym.Test.Addins;
using NUnit.Core;
using System.Reflection;

namespace Test.Urasandesu.NAnonym.Test.Addins
{
    [TestFixture]
    public class NewDomainTestFixtureTest
    {
        [Test]
        public void Test1()
        {
            var testFixture = new NewDomainTestFixtureAccessor(typeof(SampleSuiteClass));
            Assert.AreEqual(3, testFixture.TestCount);
            Assert.AreEqual(1, testFixture.GetSetUpMethods().Length);
            Assert.AreEqual(1, testFixture.GetTearDownMethods().Length);
            Assert.AreEqual(1, testFixture.GetTestFixtureSetUpMethods().Length);
            Assert.AreEqual(1, testFixture.GetTestFixtureTearDownMethods().Length);
            var testResult = testFixture.Run(new QueuingEventListener(), new NullFilter());
            Assert.IsTrue(testResult.IsSuccess);
        }

        [Test]
        public void Test2()
        {
            var testFixture = new NewDomainTestFixtureAccessor(typeof(SampleNotSuiteClass));
            Assert.AreEqual(0, testFixture.TestCount);
            Assert.AreEqual(0, testFixture.GetSetUpMethods().Length);
            Assert.AreEqual(0, testFixture.GetTearDownMethods().Length);
            Assert.AreEqual(0, testFixture.GetTestFixtureSetUpMethods().Length);
            Assert.AreEqual(0, testFixture.GetTestFixtureTearDownMethods().Length);
        }





        [Serializable]
        class NullFilter : TestFilter
        {
            public override bool Match(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }

            public override bool Pass(ITest test)
            {
                return test.RunState != RunState.Explicit;
            }
        }

        class NewDomainTestFixtureAccessor : NewDomainTestFixture
        {
            public NewDomainTestFixtureAccessor(Type fixtureType)
                : base(fixtureType)
            {
            }

            public MethodInfo[] GetTestFixtureSetUpMethods()
            {
                return fixtureSetUpMethods;
            }

            public MethodInfo[] GetTestFixtureTearDownMethods()
            {
                return fixtureTearDownMethods;
            }
        }
    }
}

