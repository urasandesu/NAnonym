using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class BodyInjectionDefiner
    {
        public BodyInjection ParentBody { get; private set; }
        public BodyInjectionDefiner(BodyInjection parentBody)
        {
            ParentBody = parentBody;
        }

        public abstract void Create();
    }
}
