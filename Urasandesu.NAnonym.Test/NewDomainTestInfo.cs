using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Test
{
    [Serializable]
    public class NewDomainTestInfo
    {
        public NewDomainTestInfo()
            : this(null)
        {
        }

        public NewDomainTestInfo(string friendlyName)
        {
            FriendlyName = string.IsNullOrEmpty(friendlyName) ? "NewDomain" : friendlyName;
        }

        public string FriendlyName { get; set; }
        public string AssemblyFileName { get; set; }
        public string TypeFullName { get; set; }
        public string MethodName { get; set; }
        public NewDomainTestVerifier TestVerifier { get; set; }
        public virtual void Verify()
        {
            TestVerifier(new NewDomainTestTarget(this));
        }
    }
}
