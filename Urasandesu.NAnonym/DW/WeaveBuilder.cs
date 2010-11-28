using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class WeaveBuilder
    {
        public WeaveDefiner ParentDefiner { get; private set; }
        public WeaveBuilder(WeaveDefiner parentDefiner)
        {
            ParentDefiner = parentDefiner;
        }

        public abstract void Construct();
    }
}
