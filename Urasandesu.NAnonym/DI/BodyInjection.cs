using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    abstract class BodyInjection
    {
        public ExpressiveMethodBodyGenerator Gen { get; private set; }
        public InjectionBuilder ParentBuilder { get; private set; }
        public BodyInjection(ExpressiveMethodBodyGenerator gen, InjectionBuilder parentBuilder)
        {
            Gen = gen;
            ParentBuilder = parentBuilder;
        }

        public abstract void Apply();
    }
}
