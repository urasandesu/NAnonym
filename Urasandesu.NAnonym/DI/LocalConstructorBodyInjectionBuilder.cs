using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public LocalConstructorBodyInjectionBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
