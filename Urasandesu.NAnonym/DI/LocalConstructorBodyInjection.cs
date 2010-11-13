using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjection : ConstructorBodyInjection
    {
        public new LocalConstructorInjectionBuilder ParentBuilder { get { return (LocalConstructorInjectionBuilder)base.ParentBuilder; } }
        public LocalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, LocalConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }
    }
}
