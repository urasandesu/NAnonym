using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodBodyWeaveBuilder : BodyWeaveBuilder
    {
        public new MethodBodyWeaveDefiner ParentBodyDefiner { get { return (MethodBodyWeaveDefiner)base.ParentBodyDefiner; } }
        protected MethodBodyWeaveBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
