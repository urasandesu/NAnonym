using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodInjectionBuilder : MethodInjectionBuilder
    {
        public new GlobalMethodInjectionDefiner ParentDefiner { get { return (GlobalMethodInjectionDefiner)base.ParentDefiner; } }
        public GlobalMethodInjectionBuilder(GlobalMethodInjectionDefiner parentDefiner)
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
