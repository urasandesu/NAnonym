using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public new LocalConstructorBodyInjectionDefiner ParentBodyDefiner { get { return (LocalConstructorBodyInjectionDefiner)base.ParentBodyDefiner; } }
        public LocalConstructorBodyInjectionBuilder(LocalConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
