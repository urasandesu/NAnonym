/* 
 * File: NewDomainTestFixture.cs
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
using NUnit.Core;
using System.Reflection;

namespace Urasandesu.NAnonym.Test.Addins
{
    public class NewDomainTestFixture : NUnitTestFixture
    {
        AppDomain newDomain;

        public NewDomainTestFixture(Type fixtureType)
            : base(fixtureType)
        {
            base.fixtureSetUpMethods = Reflect.GetMethodsWithAttribute(FixtureType, "Urasandesu.NAnonym.Test.NewDomainTestFixtureSetUpAttribute", true);
            base.fixtureTearDownMethods = Reflect.GetMethodsWithAttribute(FixtureType, "Urasandesu.NAnonym.Test.NewDomainTestFixtureTearDownAttribute", true);
            base.setUpMethods = Reflect.GetMethodsWithAttribute(FixtureType, "Urasandesu.NAnonym.Test.NewDomainSetUpAttribute", true);
            base.tearDownMethods = Reflect.GetMethodsWithAttribute(FixtureType, "Urasandesu.NAnonym.Test.NewDomainTearDownAttribute", true);
            foreach (var methodInfo in FixtureType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (Reflect.HasAttribute(methodInfo, "Urasandesu.NAnonym.Test.NewDomainTestAttribute", true))
                    Add(new NUnitTestMethod(methodInfo));
            }
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult)
        {
            newDomain = AppDomain.CreateDomain(suiteResult.FullName, null, AppDomain.CurrentDomain.SetupInformation);
            var o = newDomain.CreateInstanceAndUnwrap(
                                    FixtureType.Assembly.FullName,
                                    FixtureType.FullName,
                                    true,
                                    BindingFlags.Default,
                                    null,
                                    new object[] { },
                                    null,
                                    null,
                                    null);

            if (Reflect.InheritsFrom(FixtureType, "Urasandesu.NAnonym.Test.NewDomainTestBase"))
            {
                var consoleProxy = 
                    AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
                        "Urasandesu.NAnonym.Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 
                        "Urasandesu.NAnonym.Test.ConsoleProxy", 
                        false, 
                        BindingFlags.Default, 
                        null, 
                        new object[] { }, 
                        null, 
                        null, 
                        null);

                var consolePropertyInfo = Reflect.GetNamedProperty(FixtureType, "Console", BindingFlags.Public | BindingFlags.Instance);
                consolePropertyInfo.SetValue(o, consoleProxy, null);
            }
            Fixture = o;
            base.DoOneTimeSetUp(suiteResult);
        }

        protected override void DoOneTimeTearDown(TestResult suiteResult)
        {
            base.DoOneTimeTearDown(suiteResult);
            Fixture = null;
            AppDomain.Unload(newDomain);
            newDomain = null;
        }
    }

}

