using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
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
