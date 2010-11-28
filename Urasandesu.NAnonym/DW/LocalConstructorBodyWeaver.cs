using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorBodyWeaver : ConstructorBodyWeaver
    {
        public LocalConstructorBodyWeaver(ExpressiveMethodBodyGenerator gen, ConstructorWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override ConstructorBodyWeaveDefiner GetBodyDefiner(ConstructorBodyWeaver parentBody)
        {
            return new LocalConstructorBodyWeaveDefiner(parentBody);
        }

        protected override ConstructorBodyWeaveBuilder GetBodyBuilder(ConstructorBodyWeaveDefiner parentBodyDefiner)
        {
            return new LocalConstructorBodyWeaveBuilder(parentBodyDefiner);
        }
    }
}
