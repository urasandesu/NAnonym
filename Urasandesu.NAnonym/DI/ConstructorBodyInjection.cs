using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    abstract class ConstructorBodyInjection : BodyInjection
    {
        public new ConstructorInjectionBuilder ParentBuilder { get { return (ConstructorInjectionBuilder)base.ParentBuilder; } }
        public ConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, ConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = GetBodyDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GetBodyBuilder(bodyDefiner);
            bodyBuilder.Construct();
        }

        protected abstract ConstructorBodyInjectionDefiner GetBodyDefiner(ConstructorBodyInjection parentBody);
        protected abstract ConstructorBodyInjectionBuilder GetBodyBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner);
    }
}
