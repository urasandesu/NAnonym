using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public new GlobalConstructorBodyInjectionDefiner ParentBodyDefiner { get { return (GlobalConstructorBodyInjectionDefiner)base.ParentBodyDefiner; } }
        public GlobalConstructorBodyInjectionBuilder(GlobalConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
