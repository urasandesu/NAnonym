using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class MethodBodyInjectionBuilder : BodyInjectionBuilder
    {
        public new MethodBodyInjectionDefiner ParentBodyDefiner { get { return (MethodBodyInjectionDefiner)base.ParentBodyDefiner; } }
        protected MethodBodyInjectionBuilder(MethodBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
