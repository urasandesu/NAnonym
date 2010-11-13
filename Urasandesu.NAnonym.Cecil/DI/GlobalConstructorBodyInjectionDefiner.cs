using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionDefiner : ConstructorBodyInjectionDefiner
    {
        public new GlobalConstructorBodyInjection ParentBody { get { return (GlobalConstructorBodyInjection)base.ParentBody; } }
        public GlobalConstructorBodyInjectionDefiner(GlobalConstructorBodyInjection parentBody)
            : base(parentBody)
        {
        }
    }
}
