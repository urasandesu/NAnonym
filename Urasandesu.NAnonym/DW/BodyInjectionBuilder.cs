using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class BodyInjectionBuilder
    {
        public BodyInjectionDefiner ParentBodyDefiner { get; private set; }
        public BodyInjectionBuilder(BodyInjectionDefiner parentBodyDefiner)
        {
            ParentBodyDefiner = parentBodyDefiner;
        }

        public abstract void Construct();
    }
}
