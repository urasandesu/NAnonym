using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Test
{
    public class Is : NUnit.Framework.Is
    {
        protected Is()
        {
        }

        public static NUnit.Framework.Constraints.EqualConstraint EqualTo<T1, T2>(T1 expected, Func<T1, T2, bool> equalityComparer)
        {
            return new EqualConstraint<T1, T2>(expected, equalityComparer);
        }
    }
}
