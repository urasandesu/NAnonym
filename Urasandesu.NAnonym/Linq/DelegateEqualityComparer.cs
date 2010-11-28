/* 
 * File: DelegateEqualityComparer.cs
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
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        public static readonly Func<T, int> DefaultGetHashCode = obj => EqualityComparer<T>.Default.GetHashCode(obj);
        public static readonly Func<T, T, bool> DefaultEquals = (x, y) => EqualityComparer<T>.Default.Equals(x, y);
        public static readonly Func<T, int> NullableGetHashCode = ToNullableGetHashCode(DefaultGetHashCode);
        public static readonly Func<T, T, bool> NullableEquals = ToNullableEquals(DefaultEquals);

        Func<T, int> getHashCode;
        Func<T, T, bool> equals;


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

    public abstract class EqualityComparer<T1, T2> : IEqualityComparer<T1, T2>
    {
        // boxing 回避したいお（´・ω・｀）
        class DefaultEqualityComparer : EqualityComparer<T1, T2>
        {
            public override bool Equals(T1 x, T2 y)
            {
                return x == null && y == null ? true : x == null || y == null ? false : x.Equals(y);
            }

            public override int GetHashCode(T1 x, T2 y)
            {
                return
                    (x == null ? 0 : x.GetHashCode()) ^
                    (y == null ? 0 : y.GetHashCode());
            }
        }

        static EqualityComparer()
        {
            Default = new DefaultEqualityComparer();
        }

        public static EqualityComparer<T1, T2> Default { get; private set; }
        public abstract bool Equals(T1 x, T2 y);
        public abstract int GetHashCode(T1 x, T2 y);
    }


    
    public class DelegateEqualityComparer<T1, T2> : IEqualityComparer<T1, T2>
    {
        public static readonly Func<T1, T2, int> DefaultGetHashCode = (x, y) => EqualityComparer<T1, T2>.Default.GetHashCode(x, y);
        public static readonly Func<T1, T2, bool> DefaultEquals = (x, y) => EqualityComparer<T1, T2>.Default.Equals(x, y);

        Func<T1, T2, int> getHashCode;
        Func<T1, T2, bool> equals;


        public DelegateEqualityComparer()
            : this(null, null)
        {
        }

        public DelegateEqualityComparer(Func<T1, T2, int> getHashCode)
            : this(getHashCode, null)
        {
        }

        public DelegateEqualityComparer(Func<T1, T2, bool> equals)
            : this(null, equals)
        {
        }

        public DelegateEqualityComparer(Func<T1, T2, int> getHashCode, Func<T1, T2, bool> equals)
        {
            this.getHashCode = getHashCode == null ? DefaultGetHashCode : getHashCode;
            this.equals = equals == null ? DefaultEquals : equals;
        }

        public static Func<T1, T2, int> ToNullableGetHashCode(Func<T1, T2, int> getHashCode)
        {
            return (x, y) => x == null || y == null ? 0 : getHashCode(x, y);
        }

        public static Func<T1, T2, bool> ToNullableEquals(Func<T1, T2, bool> equals)
        {
            return (x, y) => x == null && y == null ? true : x == null || y == null ? false : equals(x, y);
        }

        #region IEqualityComparer<T1,T2> Member

        public bool Equals(T1 x, T2 y)
        {
            return equals(x, y);
        }

        public int GetHashCode(T1 x, T2 y)
        {
            return getHashCode(x, y);
        }

        #endregion
    }

}

