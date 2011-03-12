using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.Formulas
{
    public partial class BaseNewFormula : NewFormula
    {
        public BaseNewFormula(Formula[] arguments)
            : base(arguments)
        {
        }

        public BaseNewFormula(ConstructorInfo ci, Formula[] arguments)
            : base(ci, arguments)
        {
        }
    }
}
