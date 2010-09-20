using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Linq
{
    public interface IEqualityComparer<T1, T2>
    {
        bool Equals(T1 x, T2 y);
        int GetHashCode(T1 x, T2 y);
    }
}
