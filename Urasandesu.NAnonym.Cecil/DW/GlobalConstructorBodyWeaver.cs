using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorBodyWeaver : ConstructorBodyWeaver
    {
        public GlobalConstructorBodyWeaver(ExpressiveMethodBodyGenerator gen, ConstructorWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override ConstructorBodyWeaveDefiner GetBodyDefiner(ConstructorBodyWeaver parentBody)
        {
            return new GlobalConstructorBodyWeaveDefiner(parentBody);
        }

        protected override ConstructorBodyWeaveBuilder GetBodyBuilder(ConstructorBodyWeaveDefiner parentBodyDefiner)
        {
            return new GlobalConstructorBodyWeaveBuilder(parentBodyDefiner);
        }
    }
}
