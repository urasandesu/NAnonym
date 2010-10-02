using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Test;
using Urasandesu.NAnonym.ILTools;

namespace Test.Urasandesu.NAnonym.Cecil.ILTools
{
    [Serializable]
    public class NewDomainTestInfoWithScope : NewDomainTestInfo
    {
        public NewDomainTestInfoWithScope()
            : base()
        {
        }

        public NewDomainTestInfoWithScope(string friendlyName)
            : base(friendlyName)
        {
        }

        public PortableScope Scope { get; set; }
    }
}
