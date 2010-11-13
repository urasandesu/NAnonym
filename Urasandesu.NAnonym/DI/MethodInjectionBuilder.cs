using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class MethodInjectionBuilder : InjectionBuilder
    {
        public new MethodInjectionDefiner ParentDefiner { get { return (MethodInjectionDefiner)base.ParentDefiner; } }
        public MethodInjectionBuilder(MethodInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            throw new NotImplementedException();
        }
    }
}
