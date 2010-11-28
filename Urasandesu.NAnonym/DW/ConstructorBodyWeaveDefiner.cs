using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorBodyWeaveDefiner : BodyWeaveDefiner
    {
        public new ConstructorBodyWeaver ParentBody { get { return (ConstructorBodyWeaver)base.ParentBody; } }
        public ConstructorBodyWeaveDefiner(ConstructorBodyWeaver parentBody)
            : base(parentBody)
        {
        }

        public abstract override void Create();
    }
}
