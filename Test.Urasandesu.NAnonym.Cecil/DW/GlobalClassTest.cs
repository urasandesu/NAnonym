/* 
 * File: GlobalClassTest.cs
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
using Test.Urasandesu.NAnonym.Etc;
using Urasandesu.NAnonym.Cecil.DW;
using Urasandesu.NAnonym.Test;
using Assert = Urasandesu.NAnonym.Test.Assert;

namespace Test.Urasandesu.NAnonym.Cecil.DW
{
    [NewDomainTestFixture]
    public class GlobalClassTest : NewDomainTestBase
    {
        [NewDomainTestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            GlobalDomain.Revert();

            GlobalDomain.Register<GlobalClass1>();
            GlobalDomain.Register<GlobalClass2>();
            GlobalDomain.Register<GlobalClass3_1>();
            GlobalDomain.Register<GlobalClass3_2>();
            GlobalDomain.Register<GlobalClass3_3>();
            GlobalDomain.Register<GlobalClass3_4>();
            GlobalDomain.Register<GlobalClass3_5>();
            GlobalDomain.Register<GlobalClass3_6>();
            GlobalDomain.Register<GlobalClass3_7>();
            GlobalDomain.Register<GlobalClass4_1>();
            GlobalDomain.Load();
        }


        [NewDomainTest]
        public void Class1Class2Test()
        {
            var class1 = new Class1();
            var class2 = new Class2();
            string value = "aiueo";
            
            Assert.AreEqual(
                "Modified prefix at Class1.Print" +
                "Modified prefix at Class2.Print" +
                value +
                "Modified suffix at Class2.Print" +
                "Modified suffix at Class1.Print",
                class1.Print(value));

            Assert.AreEqual(
                "Modified prefix at Class2.Print" +
                value +
                "Modified suffix at Class2.Print",
                class2.Print(value));
        }


        [NewDomainTest]
        public void Class3_1Test()
        {
            var class3 = new Class3_1();

            Assert.AreEqual(4, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_2Test()
        {
            var class3 = new Class3_2();

            Assert.AreEqual(4, class3.Add(1, 1));
            Assert.AreEqual(6, class3.Add(1, 1));
            Assert.AreEqual(8, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_3Test()
        {
            var class3 = new Class3_3();

            Assert.AreEqual(2, class3.Add(1, 1));
            Assert.AreEqual(4, class3.Add(1, 1));
            Assert.AreEqual(6, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_4Test()
        {
            var class3 = new Class3_4();

            Assert.AreEqual(2, class3.Add(1, 1));
            Assert.AreEqual(4, class3.Add(1, 1));
            Assert.AreEqual(6, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_5Test()
        {
            var class3 = new Class3_5();

            Assert.AreEqual(2, class3.Add(1, 1));
            Assert.AreEqual(3, class3.Add(1, 1));
            Assert.AreEqual(4, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_6Test()
        {
            var class3 = new Class3_6();

            Assert.AreEqual(4, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class3_7Test()
        {
            var class3 = new Class3_7();

            Assert.AreEqual(6, class3.Add(1, 1));
            Assert.AreEqual(10, class3.Add(1, 1));
            Assert.AreEqual(14, class3.Add(1, 1));
        }


        [NewDomainTest]
        public void Class4_1Test()
        {
            var class4 = new Class4_1();

            TestHelper.ClearLog();

            Assert.AreEqual(-10, class4.Inverse(10));
            Assert.AreEqual(
@"sourceType = Test.Urasandesu.NAnonym.Etc.Class4_1, sourceMethod = Inverse, n = 10
sourceType = Test.Urasandesu.NAnonym.Etc.Class4_1, sourceMethod = Inverse, n = 10, result = -10", TestHelper.ReadLogToEnd());
        }
    }
}

