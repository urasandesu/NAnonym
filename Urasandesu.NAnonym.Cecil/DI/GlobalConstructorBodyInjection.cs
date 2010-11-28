using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjection : ConstructorBodyInjection
    {
        public GlobalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, ConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override ConstructorBodyInjectionDefiner GetBodyDefiner(ConstructorBodyInjection parentBody)
        {
            return new GlobalConstructorBodyInjectionDefiner(parentBody);
        }

        protected override ConstructorBodyInjectionBuilder GetBodyBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
        {
            return new GlobalConstructorBodyInjectionBuilder(parentBodyDefiner);
        }
    }
}
