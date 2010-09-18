using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NF = NUnit.Framework;

namespace Urasandesu.NAnonym.Test
{
    public class Assert : NF::Assert
    {
        protected Assert()
        {
        }

        public static void AreEquivalent<T1, T2>(T1 expected, T2 actual, Func<T1, T2, bool> equalityComparer)
        {
            NF::Assert.That(actual, Is.EqualTo<T1, T2>(expected, equalityComparer));
        }
    }
}
