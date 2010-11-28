using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodBodyWeaveDefiner : BodyWeaveDefiner
    {
        public new MethodBodyWeaver ParentBody { get { return (MethodBodyWeaver)base.ParentBody; } }
        public MethodBodyWeaveDefiner(MethodBodyWeaver parentBody)
            : base(parentBody)
        {
        }
    }
}
