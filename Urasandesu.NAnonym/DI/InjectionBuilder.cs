using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class InjectionBuilder
    {
        public InjectionDefiner ParentDefiner { get; private set; }
        public InjectionBuilder(InjectionDefiner parentDefiner)
        {
            ParentDefiner = parentDefiner;
        }

        public abstract void Construct();
    }
}
