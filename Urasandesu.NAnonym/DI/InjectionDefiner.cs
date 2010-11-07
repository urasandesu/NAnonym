using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class InjectionDefiner
    {
        public Injection Injection { get; private set; }
        public InjectionDefiner(Injection injection)
        {
            Injection = injection;
        }

        public abstract void Create();
    }
}
