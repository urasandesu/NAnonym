using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    class LocalMethodBodyInjectionDefiner : MethodBodyInjectionDefiner
    {
        public LocalMethodBodyInjectionDefiner(MethodBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
