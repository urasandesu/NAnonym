using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class BodyInjectionDefiner
    {
        public BodyInjection Injection { get; private set; }
        public BodyInjectionDefiner(BodyInjection injection)
        {
            Injection = injection;
        }

        public abstract void Create();
    }
}
