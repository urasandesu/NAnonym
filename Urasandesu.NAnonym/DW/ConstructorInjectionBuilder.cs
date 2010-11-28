using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorInjectionBuilder : InjectionBuilder
    {
        public new ConstructorInjectionDefiner ParentDefiner { get { return (ConstructorInjectionDefiner)base.ParentDefiner; } }
        public ConstructorInjectionBuilder(ConstructorInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public abstract override void Construct();
    }
}
