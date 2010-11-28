using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public GlobalConstructorBodyInjectionBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
