using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjection : ConstructorBodyInjection
    {
        public LocalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, ConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override ConstructorBodyInjectionDefiner GetBodyDefiner(ConstructorBodyInjection parentBody)
        {
            return new LocalConstructorBodyInjectionDefiner(parentBody);
        }

        protected override ConstructorBodyInjectionBuilder GetBodyBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
        {
            return new LocalConstructorBodyInjectionBuilder(parentBodyDefiner);
        }
    }
}
