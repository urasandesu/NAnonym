using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions.Linq
{
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        public static readonly Func<T, int> DefaultGetHashCode = obj => EqualityComparer<T>.Default.GetHashCode(obj);
        public static readonly Func<T, T, bool> DefaultEquals = (x, y) => EqualityComparer<T>.Default.Equals(x, y);
        public static readonly Func<T, int> NullableGetHashCode = ToNullableGetHashCode(DefaultGetHashCode);
        public static readonly Func<T, T, bool> NullableEquals = ToNullableEquals(DefaultEquals);

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
            return getHashCode(obj);
        }

        public bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        #endregion

        public static Func<T, int> ToNullableGetHashCode(Func<T, int> getHashCode)
        {
            return obj => obj == null ? 0 : getHashCode(obj);
        }

        public static Func<T, T, bool> ToNullableEquals(Func<T, T, bool> equals)
        {
            return (x, y) => x == null && y == null ? true : x == null || y == null ? false : equals(x, y);
        }
    }

}
