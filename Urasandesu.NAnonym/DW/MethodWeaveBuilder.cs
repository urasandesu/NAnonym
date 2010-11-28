using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodWeaveBuilder : WeaveBuilder
    {
        public new MethodWeaveDefiner ParentDefiner { get { return (MethodWeaveDefiner)base.ParentDefiner; } }
        public MethodWeaveBuilder(MethodWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public abstract override void Construct();
    }
}
