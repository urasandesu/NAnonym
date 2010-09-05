using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.CREUtilities;

namespace Urasandesu.NAnonym.Test
{
    [Serializable]
    public class NewDomainTestInfo
    {
        public string AssemblyFileName { get; set; }
        public string TypeFullName { get; set; }
        public string MethodName { get; set; }
        public NewDomainTestVerifier TestVerifier { get; set; }
        public PortableScope Scope { get; set; }
        public virtual void Verify()
        {
            TestVerifier(new NewDomainTestTarget(this));
        }
    }
}
