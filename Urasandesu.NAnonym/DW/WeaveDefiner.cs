using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class WeaveDefiner
    {
        public Weaver Parent { get; private set; }
        public WeaveDefiner(Weaver parent)
        {
            Parent = parent;
        }

        public abstract void Create();
    }
}
