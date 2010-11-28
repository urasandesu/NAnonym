using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodBodyWeaver : MethodBodyWeaver
    {
        public GlobalMethodBodyWeaver(ExpressiveMethodBodyGenerator gen, MethodWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override MethodBodyWeaveDefiner GetMethodBodyDefiner(MethodBodyWeaver parentBody)
        {
            return new GlobalMethodBodyWeaveDefiner(parentBody);
        }
    }
}
