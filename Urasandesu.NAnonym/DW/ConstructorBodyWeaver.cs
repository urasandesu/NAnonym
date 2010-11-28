using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorBodyWeaver : BodyWeaver
    {
        public new ConstructorWeaveBuilder ParentBuilder { get { return (ConstructorWeaveBuilder)base.ParentBuilder; } }
        public ConstructorBodyWeaver(ExpressiveMethodBodyGenerator gen, ConstructorWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = GetBodyDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GetBodyBuilder(bodyDefiner);
            bodyBuilder.Construct();
        }

        protected abstract ConstructorBodyWeaveDefiner GetBodyDefiner(ConstructorBodyWeaver parentBody);
        protected abstract ConstructorBodyWeaveBuilder GetBodyBuilder(ConstructorBodyWeaveDefiner parentBodyDefiner);
    }
}
