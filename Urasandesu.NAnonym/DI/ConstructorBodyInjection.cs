using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorBodyInjection : BodyInjection
    {
        public new ConstructorInjectionBuilder Builder { get { return (ConstructorInjectionBuilder)base.Builder; } }
        public ConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, ConstructorInjectionBuilder builder)
            : base(gen, builder)
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
