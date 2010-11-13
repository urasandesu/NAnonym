using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodBodyInjectionDefiner : MethodBodyInjectionDefiner
    {
        public new LocalMethodBodyInjection ParentBody { get { return (LocalMethodBodyInjection)base.ParentBody; } }
        public LocalMethodBodyInjectionDefiner(LocalMethodBodyInjection parentBody)
            : base(parentBody)
        {
        }
    }
}
