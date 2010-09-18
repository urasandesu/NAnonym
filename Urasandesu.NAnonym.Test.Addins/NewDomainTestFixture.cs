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
            foreach (var methodInfo in FixtureType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (Reflect.HasAttribute(methodInfo, typeof(NewDomainTestAttribute).FullName, false))
                    Add(new NUnitTestMethod(methodInfo));
            }
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult)
        {
            newDomain = AppDomain.CreateDomain(suiteResult.FullName, null, AppDomain.CurrentDomain.SetupInformation);
            var testBase = (NewDomainTestBase)newDomain.CreateInstanceAndUnwrap(
                                                    FixtureType.Assembly.FullName,
                                                    FixtureType.FullName,
                                                    true,
                                                    BindingFlags.Default,
                                                    null,
                                                    new object[] { },
                                                    null,
                                                    null,
                                                    null);
            testBase.Console = new ConsoleProxy();
            Fixture = testBase;
            base.DoOneTimeSetUp(suiteResult);
        }

        protected override void DoOneTimeTearDown(TestResult suiteResult)
        {
            base.DoOneTimeTearDown(suiteResult);
            AppDomain.Unload(newDomain);
            newDomain = null;
        }
    }

}
