using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorBodyInjectionDefiner : ConstructorBodyInjectionDefiner
    {
        public new LocalConstructorBodyInjection Injection { get { return (LocalConstructorBodyInjection)base.Injection; } }
        public LocalConstructorBodyInjectionDefiner(LocalConstructorBodyInjection injection)
            : base(injection)
        {
        }
    }
}
