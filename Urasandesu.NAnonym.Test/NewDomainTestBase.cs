using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Test
{
    public abstract class NewDomainTestBase : MarshalByRefObject
    {
        public ConsoleProxy Console { get; set; }
    }
}
