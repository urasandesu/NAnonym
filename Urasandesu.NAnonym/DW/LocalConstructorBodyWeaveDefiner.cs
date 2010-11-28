using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorBodyWeaveDefiner : ConstructorBodyWeaveDefiner
    {
        //public new LocalConstructorBodyWeaver ParentBody { get { return (LocalConstructorBodyWeaver)base.ParentBody; } }
        public LocalConstructorBodyWeaveDefiner(ConstructorBodyWeaver parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
