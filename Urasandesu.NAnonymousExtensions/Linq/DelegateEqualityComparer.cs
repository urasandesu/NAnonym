using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions.Linq
{
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        public static readonly Func<T, int> DefaultGetHashCode = obj => obj.GetHashCode();
        public static readonly Func<T, T, bool> DefaultEquals = (x, y) => x.Equals(y);

        private Func<T, int> getHashCode;
        private Func<T, T, bool> equals;


        public DelegateEqualityComparer()
            : this(null, null)
        {
        }

        public DelegateEqualityComparer(Func<T, int> getHashCode)
            : this(getHashCode, null)
        {
        }

        public DelegateEqualityComparer(Func<T, T, bool> equals)
            : this(null, equals)
        {
        }

        public DelegateEqualityComparer(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            this.getHashCode = getHashCode == null ? DefaultGetHashCode : getHashCode;
            this.equals = equals == null ? DefaultEquals : equals;
        }

        #region IEqualityComparer<T> Member

        public int GetHashCode(T obj)
        {
            return obj == null ? 0 : getHashCode(obj);
        }

        public bool Equals(T x, T y)
        {
            return x == null && y == null ? true : x == null || y == null ? false : equals(x, y);
        }

        #endregion
    }
}
