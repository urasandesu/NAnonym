using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionBuilder : ConstructorBodyInjectionBuilder
    {
        public new GlobalConstructorBodyInjectionDefiner Definer { get { return (GlobalConstructorBodyInjectionDefiner)base.Definer; } }
        public GlobalConstructorBodyInjectionBuilder(GlobalConstructorBodyInjectionDefiner definer)
            : base(definer)
        {
        }
    }
}
