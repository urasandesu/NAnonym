using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodBodyInjection : MethodBodyInjection
    {
        public new GlobalMethodInjectionBuilder ParentBuilder { get { return (GlobalMethodInjectionBuilder)base.ParentBuilder; } }
        public GlobalMethodBodyInjection(ExpressiveMethodBodyGenerator gen, GlobalMethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = new GlobalMethodBodyInjectionDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GlobalMethodBodyInjectionBuilder.GetInstance(bodyDefiner);
            bodyBuilder.Construct();
        }
    }
}
