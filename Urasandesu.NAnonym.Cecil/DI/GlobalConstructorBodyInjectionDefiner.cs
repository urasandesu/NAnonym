using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorBodyInjectionDefiner : ConstructorBodyInjectionDefiner
    {
        public new GlobalConstructorBodyInjection Injection { get { return (GlobalConstructorBodyInjection)base.Injection; } }
        public GlobalConstructorBodyInjectionDefiner(GlobalConstructorBodyInjection injection)
            : base(injection)
        {
        }
    }
}
