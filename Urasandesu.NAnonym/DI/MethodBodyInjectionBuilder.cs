using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class MethodBodyInjectionBuilder : BodyInjectionBuilder
    {
        public new MethodBodyInjectionDefiner ParentBodyDefiner { get { return (MethodBodyInjectionDefiner)base.ParentBodyDefiner; } }
        protected MethodBodyInjectionBuilder(MethodBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }
}
