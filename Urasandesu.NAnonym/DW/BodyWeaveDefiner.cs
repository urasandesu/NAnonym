using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class BodyWeaveDefiner
    {
        public BodyWeaver ParentBody { get; private set; }
        public BodyWeaveDefiner(BodyWeaver parentBody)
        {
            ParentBody = parentBody;
        }

        public abstract void Create();
    }
}
