using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Test
{
    public class EqualConstraint<T1, T2> : NUnit.Framework.Constraints.EqualConstraint
    {
        readonly T1 expected;
        readonly Func<T1, T2, bool> equalityComparer;

        public EqualConstraint(T1 expected, Func<T1, T2, bool> equalityComparer)
            : base(expected)
        {
            this.expected = expected;
            this.equalityComparer = equalityComparer;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            return equalityComparer(expected, (T2)actual);
        }
    }
}
