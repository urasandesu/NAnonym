/* 
 * File: AppDomainMixinTest.cs
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
using System.Collections;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class AppDomainMixinTest
    {
        [Test]
        public void RunAtIsolatedDomainTest_ShouldInitStaticMember()
        {
            // Arrange
            var bag = new CrossDomainBag();
            bag["Actual"] = 0;


            // Act
            AppDomain.CurrentDomain.RunAtIsolatedDomain(bag_ =>
            {
                bag_["Actual"] = AppDomain.CurrentDomain.GetHashCode();
            }, bag);


            // Assert
            Assert.AreNotEqual(AppDomain.CurrentDomain.GetHashCode(), bag["Actual"]);
        }



        class CrossDomainBag : MarshalByRefObject
        {
            Hashtable m_hashtable = new Hashtable();

            public object this[object key]
            {
                get
                {
                    if (!m_hashtable.ContainsKey(key))
                        m_hashtable.Add(key, null);
                    return m_hashtable[key];
                }
                set
                {
                    if (!m_hashtable.ContainsKey(key))
                        m_hashtable.Add(key, null);
                    m_hashtable[key] = value;
                }
            }
        }
    }
}
