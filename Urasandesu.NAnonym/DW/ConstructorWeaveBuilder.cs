using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorWeaveBuilder : WeaveBuilder
    {
        public new ConstructorWeaveDefiner ParentDefiner { get { return (ConstructorWeaveDefiner)base.ParentDefiner; } }
        public ConstructorWeaveBuilder(ConstructorWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public abstract override void Construct();
    }
}
