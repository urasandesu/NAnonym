using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjection : ConstructorBodyInjection
    {
        public new GlobalConstructorInjectionBuilder ParentBuilder { get { return (GlobalConstructorInjectionBuilder)base.ParentBuilder; } }
        public GlobalConstructorBodyInjection(ExpressiveMethodBodyGenerator gen, GlobalConstructorInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }
    }
}
