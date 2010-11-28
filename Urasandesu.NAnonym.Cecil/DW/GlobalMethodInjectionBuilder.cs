using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodInjectionBuilder : MethodInjectionBuilder
    {
        public GlobalMethodInjectionBuilder(MethodInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.MethodInterface.ExpressBody(
            gen =>
            {
                var bodyInjection = new GlobalMethodBodyInjection(gen, this);
                bodyInjection.Apply();
            });
        }
    }
}
