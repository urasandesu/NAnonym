using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodBodyInjectionDefiner : MethodBodyInjectionDefiner
    {
        public new GlobalMethodBodyInjection ParentBody { get { return (GlobalMethodBodyInjection)base.ParentBody; } }
        public GlobalMethodBodyInjectionDefiner(GlobalMethodBodyInjection parentBody)
            : base(parentBody)
        {
        }
    }
}
