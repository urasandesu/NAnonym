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


using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class AppDomainMixinTest
    {
        [Test]
        public void RunAtIsolatedDomain_should_run_specified_action_in_different_app()
        {
            // Arrange
            var bag = new MarshalByRefBag();
            bag["Expected"] = AppDomain.CurrentDomain.GetHashCode();
            bag["Actual"] = bag["Expected"];


            // Act
            AppDomain.CurrentDomain.RunAtIsolatedDomain(bag_ =>
            {
                bag_["Actual"] = AppDomain.CurrentDomain.GetHashCode();
            }, bag);


            // Assert
            Assert.AreNotEqual(bag["Expected"], bag["Actual"]);
        }



        [Repeat(5)]
        [Test]
        public void RunAtIsolatedProcess_should_run_specified_action_in_different_process()
        {
            // Arrange
            var bag = new MarshalByRefBag();
            bag["Expected"] = Process.GetCurrentProcess().Id;
            bag["Actual"] = bag["Expected"];


            // Act
            AppDomain.CurrentDomain.RunAtIsolatedProcess(bag_ =>
            {
                bag_["Actual"] = Process.GetCurrentProcess().Id;
            }, bag);


            // Assert
            Assert.AreNotEqual(bag["Expected"], bag["Actual"]);
        }



        [Repeat(5)]
        [Test]
        public void RunAtIsolatedProcess_can_run_parallel()
        {
            // Arrange
            var n = 0;
            var n_Increment = new MarshalByRefAction(() => Interlocked.Increment(ref n));
            var threads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(() => AppDomain.CurrentDomain.RunAtIsolatedProcess(n_Increment_ => n_Increment_.Invoke(), n_Increment));
                threads.Add(thread);
            }


            // Act
            threads.ForEach(_ => _.Start());
            threads.ForEach(_ => _.Join());


            // Assert
            Assert.AreEqual(10, n);
        }



        [Test]
        public void RunAtIsolatedProcess_should_wrap_the_exception_that_occurred_in_the_action_in_TargetInvocationException_and_rethrow_it()
        {
            // Arrange
            var errorAction = new Action(() => throw new ApplicationException("aiueo"));


            // Act, Assert
            var ex = Assert.Throws<TargetInvocationException>(() => AppDomain.CurrentDomain.RunAtIsolatedProcess(errorAction));
            Assert.IsInstanceOf<ApplicationException>(ex.InnerException);
            Assert.AreEqual("aiueo", ex.InnerException.Message);
        }



        [Test]
        public void RunAtIsolatedProcess_can_run_repeatedly_even_if_an_exception_has_occurred()
        {
            // Arrange
            var errorAction = new Action(() => throw new ApplicationException());
            Assert.Throws<TargetInvocationException>(() => AppDomain.CurrentDomain.RunAtIsolatedProcess(errorAction));

            var run = false;
            var run_Assign = new MarshalByRefAction<bool>(value => run = value);


            // Act
            AppDomain.CurrentDomain.RunAtIsolatedProcess(run_Assign_ =>
            {
                run_Assign_.Invoke(true);
            }, run_Assign);


            // Assert
            Assert.IsTrue(run);
        }



        [Test]
        public void RunAtIsolatedProcess_should_throw_ArgumentException_if_domain_uncrossable_action_is_passed()
        {
            // Arrange
            var x = 1;
            var domainUncrossableAction = new Action(() => x++);

            // Act, Assert
            Assert.Throws<ArgumentException>(() => AppDomain.CurrentDomain.RunAtIsolatedProcess(domainUncrossableAction));
        }



        [Test]
        public void RunAtIsolatedProcess_should_throw_ArgumentException_if_domain_uncrossable_parameter_for_the_action_is_passed()
        {
            // Arrange
            var domainUncrossableParameter = new Stopwatch();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => AppDomain.CurrentDomain.RunAtIsolatedProcess(_ => { }, domainUncrossableParameter));
        }



        class MarshalByRefBag : MarshalByRefObject
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
