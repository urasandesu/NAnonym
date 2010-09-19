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
                if (Reflect.HasAttribute(methodInfo, "Urasandesu.NAnonym.Test.NewDomainTestAttribute", false))
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
