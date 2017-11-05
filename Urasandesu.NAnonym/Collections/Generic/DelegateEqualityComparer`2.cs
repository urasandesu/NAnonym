/* 
 * File: DelegateEqualityComparer`2.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2016 Akira Sugiura
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

namespace Urasandesu.NAnonym.Collections.Generic
{
    public class DelegateEqualityComparer<T, TProp> : IEqualityComparer<T>
    {
        readonly Func<T, TProp> m_selector;
        readonly Func<TProp, TProp, bool> m_equals;
        readonly Func<TProp, int> m_getHashCode;
        public DelegateEqualityComparer(Func<T, TProp> selector, Func<TProp, TProp, bool> equals, Func<TProp, int> getHashCode)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if (equals == null)
                throw new ArgumentNullException("equals");

            if (getHashCode == null)
                throw new ArgumentNullException("getHashCode");

            m_selector = selector;
            m_equals = equals;
            m_getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            var result = false;
            if (TryGetNullEquality(x, y, out result))
                return result;

            return EqualsProp(m_selector(x), m_selector(y));
        }

        bool EqualsProp(TProp x, TProp y)
        {
            var result = false;
            if (TryGetNullEquality(x, y, out result))
                return result;

            return m_equals(x, y);
        }

        static bool TryGetNullEquality<U>(U x, U y, out bool result)
        {
            if (x == null && y == null)
            {
                result = true;
                return true;
            }
            else if (x == null)
            {
                result = false;
                return true;
            }
            else if (y == null)
            {
                result = false;
                return true;
            }

            result = false;
            return false;
        }

        public int GetHashCode(T obj)
        {
            var result = 0;
            if (TryGetNullHashCode(obj, out result))
                return result;

            return GetPropHashCode(m_selector(obj));
        }

        int GetPropHashCode(TProp obj)
        {
            var result = 0;
            if (TryGetNullHashCode(obj, out result))
                return result;

            return m_getHashCode(obj);
        }

        static bool TryGetNullHashCode<U>(U obj, out int result)
        {
            if (obj == null)
            {
                result = 0;
                return true;
            }

            result = 0;
            return false;
        }
    }
}

