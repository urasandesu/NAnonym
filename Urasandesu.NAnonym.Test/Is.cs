using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NF = NUnit.Framework;

namespace Urasandesu.NAnonym.Test
{
    public class Is : NF::Is
    {
        protected Is()
        {
        }

        public static NF::Constraints.EqualConstraint EqualTo<T1, T2>(T1 expected, Func<T1, T2, bool> equalityComparer)
        {
            return new EqualConstraint<T1, T2>(expected, equalityComparer);
        }
    }
}
