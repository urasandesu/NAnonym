/* 
 * File: DelegateComparer.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Linq
{
    public class DelegateComparer<T> : IComparer<T>
    {
        public static readonly Func<T, T, int> DefaultComparer = (x, y) => Comparer<T>.Default.Compare(x, y);
        public static readonly Func<T, T, int> NullableComparerAsc = ToNullableComparerAsc(DefaultComparer);
        public static readonly Func<T, T, int> NullableComparerDsc = ToNullableComparerDsc(DefaultComparer);

        Func<T, T, int> comparer;

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

