using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class InjectionDefiner
    {
        public Injection Parent { get; private set; }
        public InjectionDefiner(Injection parent)
        {
            Parent = parent;
        }

        public abstract void Create();
    }
}
