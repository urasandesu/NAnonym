using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodBodyInjection : MethodBodyInjection
    {
        public GlobalMethodBodyInjection(ExpressiveMethodBodyGenerator gen, MethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override MethodBodyInjectionDefiner GetMethodBodyDefiner(MethodBodyInjection parentBody)
        {
            return new GlobalMethodBodyInjectionDefiner(parentBody);
        }
    }
}
