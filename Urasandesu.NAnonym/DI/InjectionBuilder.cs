using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class InjectionBuilder
    {
        public InjectionDefiner Definer { get; private set; }
        public InjectionBuilder(InjectionDefiner definer)
        {
            Definer = definer;
        }

        public abstract void Construct();
    }
}
