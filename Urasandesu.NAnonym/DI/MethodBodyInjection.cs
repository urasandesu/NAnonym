using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class MethodBodyInjection : BodyInjection
    {
        public new MethodInjectionBuilder ParentBuilder { get { return (MethodInjectionBuilder)base.ParentBuilder; } }
        public MethodBodyInjection(ExpressiveMethodBodyGenerator gen, MethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
