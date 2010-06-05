using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions.Linq
{
    public class DelegateComparer<T> : IComparer<T>
    {
        public static readonly Func<T, T, int> DefaultComparer = (x, y) => Comparer<T>.Default.Compare(x, y);
        public static readonly Func<T, T, int> NullableComparerAsc = ToNullableComparerAsc(DefaultComparer);
        public static readonly Func<T, T, int> NullableComparerDsc = ToNullableComparerDsc(DefaultComparer);

        private Func<T, T, int> comparer;

        public DelegateComparer()
            : this(null)
        {
        }

        public DelegateComparer(Func<T, T, int> comparer)
        {
            this.comparer = comparer == null ? DefaultComparer : comparer;
        }

        #region IComparer<T> Member

        public int Compare(T x, T y)
        {
            return comparer(x, y);
        }

        #endregion

        public static Func<T, T, int> ToNullableComparerAsc(Func<T, T, int> comparer)
        {
            return (x, y) => x == null && y == null ? 0 : x == null ? -1 : y == null ? 1 : comparer(x, y);
        }

        public static Func<T, T, int> ToNullableComparerDsc(Func<T, T, int> comparer)
        {
            return (x, y) => x == null && y == null ? 0 : x == null ? 1 : y == null ? -1 : comparer(x, y);
        }
    }
}
