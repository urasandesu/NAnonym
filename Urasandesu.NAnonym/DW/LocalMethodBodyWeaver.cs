using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalMethodBodyWeaver : MethodBodyWeaver
    {
        public LocalMethodBodyWeaver(ExpressiveMethodBodyGenerator gen, MethodWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        protected override MethodBodyWeaveDefiner GetMethodBodyDefiner(MethodBodyWeaver parentBody)
        {
            return new LocalMethodBodyWeaveDefiner(parentBody);
        }
    }
}
