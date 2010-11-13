using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class MethodBodyInjectionDefiner : BodyInjectionDefiner
    {
        public new MethodBodyInjection ParentBody { get { return (MethodBodyInjection)base.ParentBody; } }
        public MethodBodyInjectionDefiner(MethodBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
