using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjection : ConstructorBodyInjection
    {
        public new LocalConstructorInjectionBuilder Builder { get { return (LocalConstructorInjectionBuilder)base.Builder; } }
        public LocalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, LocalConstructorInjectionBuilder builder)
            : base(gen, builder)
        {
        }
    }
}
