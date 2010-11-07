using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorInjectionBuilder : InjectionBuilder
    {
        public new ConstructorInjectionDefiner Definer { get { return (ConstructorInjectionDefiner)base.Definer; } }
        public ConstructorInjectionBuilder(ConstructorInjectionDefiner definer)
            : base(definer)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }
}
