using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodBodyInjection : MethodBodyInjection
    {
        public new LocalMethodInjectionBuilder ParentBuilder { get { return (LocalMethodInjectionBuilder)base.ParentBuilder; } }
        public LocalMethodBodyInjection(ExpressiveMethodBodyGenerator gen, LocalMethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = new LocalMethodBodyInjectionDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = LocalMethodBodyInjectionBuilder.GetInstance(bodyDefiner);
            bodyBuilder.Construct();
        }
    }
}
