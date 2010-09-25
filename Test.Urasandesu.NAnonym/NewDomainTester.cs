using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Test;
using NUnit.Framework;

namespace Test.Urasandesu.NAnonym
{
    [NewDomainTestFixture]
    public class NewDomainTester : NewDomainTestBase
    {
        [NewDomainTest]
        public void Test1()
        {
            Console.WriteLine("Current Domain: {0}", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
