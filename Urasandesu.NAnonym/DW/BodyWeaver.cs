using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class BodyWeaver
    {
        public ExpressiveMethodBodyGenerator Gen { get; private set; }
        public WeaveBuilder ParentBuilder { get; private set; }
        public BodyWeaver(ExpressiveMethodBodyGenerator gen, WeaveBuilder parentBuilder)
        {
            Gen = gen;
            ParentBuilder = parentBuilder;
        }

        public abstract void Apply();
    }
}
