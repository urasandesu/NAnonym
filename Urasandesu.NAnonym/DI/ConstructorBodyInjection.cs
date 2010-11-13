using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorBodyInjection : BodyInjection
    {
        public new ConstructorInjectionBuilder ParentBuilder { get { return (ConstructorInjectionBuilder)base.ParentBuilder; } }
        public ConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, ConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyInjectionDefiner = new ConstructorBodyInjectionDefiner(this);
            bodyInjectionDefiner.Create();

            var bodyInjectionBuilder = new ConstructorBodyInjectionBuilder(bodyInjectionDefiner);
            bodyInjectionBuilder.Construct();
        }
    }
}
