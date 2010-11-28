using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorBodyWeaveBuilder : ConstructorBodyWeaveBuilder
    {
        public LocalConstructorBodyWeaveBuilder(ConstructorBodyWeaveDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }
    }
}
