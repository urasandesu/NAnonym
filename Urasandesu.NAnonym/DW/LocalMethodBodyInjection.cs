using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalMethodBodyInjection : MethodBodyInjection
    {
        public LocalMethodBodyInjection(ExpressiveMethodBodyGenerator gen, MethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override MethodBodyInjectionDefiner GetMethodBodyDefiner(MethodBodyInjection parentBody)
        {
            return new LocalMethodBodyInjectionDefiner(parentBody);
        }
    }
}
