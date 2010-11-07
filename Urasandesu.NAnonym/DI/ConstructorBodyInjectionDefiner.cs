using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorBodyInjectionDefiner : BodyInjectionDefiner
    {
        public new ConstructorBodyInjection Injection { get { return (ConstructorBodyInjection)base.Injection; } }
        public ConstructorBodyInjectionDefiner(ConstructorBodyInjection injection)
            : base(injection)
        {
        }

        public override void Create()
        {
        }
    }
}
