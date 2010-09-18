using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core.Extensibility;
using NC = NUnit.Core;
using NUnit.Core;

namespace Urasandesu.NAnonym.Test.Addins
{
    public class NewDomainTestSuiteBuilder : ISuiteBuilder
    {
        #region ISuiteBuilder Member

        public NC::Test BuildFrom(Type type)
        {
            return new NewDomainTestFixture(type);
        }

        public bool CanBuildFrom(Type type)
        {
            return Reflect.HasAttribute(type, typeof(NewDomainTestFixtureAttribute).FullName, false);
        }

        #endregion
    }
}
