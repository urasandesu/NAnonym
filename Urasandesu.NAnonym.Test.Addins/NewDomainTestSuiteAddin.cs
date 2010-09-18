using System;
using NUnit.Core.Extensibility;
using NC = NUnit.Core;

namespace Urasandesu.NAnonym.Test.Addins
{
    [NUnitAddin(
        Name = "NewDomainTestSuite", 
        Description = "The each test that has NewDomainTestFixtureAttribute becomes it running at new AppDomain.", 
        Type = ExtensionType.Core)]
    public class NewDomainTestSuiteAddin : IAddin
    {
        #region IAddin Member

        public bool Install(IExtensionHost host)
        {
            var suiteBuilders = host.GetExtensionPoint("SuiteBuilders");
            if (suiteBuilders == null)
                return false;

            suiteBuilders.Install(new NewDomainTestSuiteBuilder());
            return true;
        }

        #endregion
    }
}
