using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Test
{
    sealed class MarshalByRefTester : MarshalByRefObject
    {
        NewDomainTestInfo testInfo;
        public MarshalByRefTester(NewDomainTestInfo testInfo)
        {
            this.testInfo = testInfo;
        }

        public void Verify()
        {
            testInfo.Verify();
        }
    }
}
