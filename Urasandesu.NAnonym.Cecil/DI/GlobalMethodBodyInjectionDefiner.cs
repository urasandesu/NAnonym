using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodBodyInjectionDefiner : MethodBodyInjectionDefiner
    {
        public GlobalMethodBodyInjectionDefiner(MethodBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
