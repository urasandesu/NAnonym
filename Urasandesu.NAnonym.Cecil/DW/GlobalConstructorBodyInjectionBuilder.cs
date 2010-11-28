using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public GlobalConstructorBodyInjectionBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
