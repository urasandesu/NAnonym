using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorBodyWeaveDefiner : ConstructorBodyWeaveDefiner
    {
        public GlobalConstructorBodyWeaveDefiner(ConstructorBodyWeaver parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
