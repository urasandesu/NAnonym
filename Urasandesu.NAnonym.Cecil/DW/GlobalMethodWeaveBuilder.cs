using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodWeaveBuilder : MethodWeaveBuilder
    {
        public GlobalMethodWeaveBuilder(MethodWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.MethodInterface.ExpressBody(
            gen =>
            {
                var bodyWeaver = new GlobalMethodBodyWeaver(gen, this);
                bodyWeaver.Apply();
            });
        }
    }
}
