using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class BodyInjectionBuilder
    {
        public BodyInjectionDefiner Definer { get; private set; }
        public BodyInjectionBuilder(BodyInjectionDefiner definer)
        {
            Definer = definer;
        }

        public abstract void Construct();
    }
}
