using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjection : ConstructorBodyInjection
    {
        public new GlobalConstructorInjectionBuilder Builder { get { return (GlobalConstructorInjectionBuilder)base.Builder; } }
        public GlobalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, GlobalConstructorInjectionBuilder builder)
            : base(gen, builder)
        {
        }
    }
}
