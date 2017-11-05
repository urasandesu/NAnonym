/* 
 * File: NullValueIsMinimumComparer`1.cs
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
    public class NullValueIsMinimumComparer<T>
    {
        protected NullValueIsMinimumComparer()
        { }

        class Comparer<TProp> : IComparer<T>
        {
            readonly Func<T, TProp> m_selector;
            readonly Func<TProp, TProp, int> m_compareTo;
            public Comparer(Func<T, TProp> selector, Func<TProp, TProp, int> compareTo)
            {
                m_selector = selector;
                m_compareTo = compareTo;
            }

            public int Compare(T x, T y)
            {
                var result = 0;
                if (TryGetNullComparison(x, y, out result))
                    return result;

                return CompareProp(m_selector(x), m_selector(y));
            }

            int CompareProp(TProp x, TProp y)
            {
                var result = 0;
                if (TryGetNullComparison(x, y, out result))
                    return result;

                return m_compareTo(x, y);
            }

            static bool TryGetNullComparison<U>(U x, U y, out int result)
            {
                if (x == null && y == null)
                {
                    result = 0;
                    return true;
                }
                else if (x == null)
                {
                    result = -1;
                    return true;
                }
                else if (y == null)
                {
                    result = 1;
                    return true;
                }

                result = 0;
                return false;
            }
        }

        public static IComparer<T> Make<TProp>(Func<T, TProp> selector) where TProp : IComparable<TProp>
        {
            return Make(selector, (_1, _2) => _1.CompareTo(_2));
        }

        public static IComparer<T> Make(Func<T, IComparable> selector)
        {
            return Make(selector, (_1, _2) => _1.CompareTo(_2));
        }

        public static IComparer<T> Make<TProp>(Func<T, TProp> selector, Func<TProp, TProp, int> compareTo)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if (compareTo == null)
                throw new ArgumentNullException("compareTo");

            return new Comparer<TProp>(selector, compareTo);
        }

        public static IComparer<T> Make(Func<T, T, int> compareTo)
        {
            if (compareTo == null)
                throw new ArgumentNullException("compareTo");

            return new Comparer<T>(_ => _, compareTo);
        }
    }
}

