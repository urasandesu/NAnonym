using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
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
