using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorBodyInjectionDefiner : BodyInjectionDefiner
    {
        public new ConstructorBodyInjection ParentBody { get { return (ConstructorBodyInjection)base.ParentBody; } }
        public ConstructorBodyInjectionDefiner(ConstructorBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
