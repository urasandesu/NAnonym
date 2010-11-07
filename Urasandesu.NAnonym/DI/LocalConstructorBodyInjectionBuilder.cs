using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public new LocalConstructorBodyInjectionDefiner Definer { get { return (LocalConstructorBodyInjectionDefiner)base.Definer; } }
        public LocalConstructorBodyInjectionBuilder(LocalConstructorBodyInjectionDefiner definer)
            : base(definer)
        {
        }
    }
}
