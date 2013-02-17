/* 
 * File: LambdaEqualityComparer.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        class Holder
        {
            public static LambdaEqualityComparer<T> ms_instance = new LambdaEqualityComparer<T>(
                (_1, _2) => EqualityComparer<T>.Default.Equals(_1, _2),
                _ => EqualityComparer<T>.Default.GetHashCode(_)
            );
        }
        public static LambdaEqualityComparer<T> Default { get { return Holder.ms_instance; } }

        Func<T, T, bool> m_equals;
        Func<T, int> m_getHashCode;

        public LambdaEqualityComparer()
            : this(Default.m_equals, Default.m_getHashCode)
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> equals)
            : this(equals, Default.m_getHashCode)
        {
        }

        public LambdaEqualityComparer(Func<T, int> getHashCode)
            : this(Default.m_equals, getHashCode)
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            if (equals == null)
                throw new ArgumentNullException("equals");
            if (getHashCode == null)
                throw new ArgumentNullException("getHashCode");

            m_equals = equals;
            m_getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            return m_equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return m_getHashCode(obj);
        }
    }
}
