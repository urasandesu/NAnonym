using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorBodyInjectionDefiner : ConstructorBodyInjectionDefiner
    {
        //public new LocalConstructorBodyInjection ParentBody { get { return (LocalConstructorBodyInjection)base.ParentBody; } }
        public LocalConstructorBodyInjectionDefiner(ConstructorBodyInjection parentBody)
            : base(parentBody)
        {
        }

        public override void Create()
        {
        }
    }
}
