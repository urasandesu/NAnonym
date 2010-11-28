using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionDefiner : ConstructorBodyInjectionDefiner
    {
        public GlobalConstructorBodyInjectionDefiner(ConstructorBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
