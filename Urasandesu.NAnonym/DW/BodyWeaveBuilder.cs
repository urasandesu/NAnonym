using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class BodyWeaveBuilder
    {
        public BodyWeaveDefiner ParentBodyDefiner { get; private set; }
        public BodyWeaveBuilder(BodyWeaveDefiner parentBodyDefiner)
        {
            ParentBodyDefiner = parentBodyDefiner;
        }

        public abstract void Construct();
    }
}
