/* 
 * File: LambdaComparer.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Urasandesu.NAnonym.Collections.Generic
{
    public class LambdaComparer<T> : IComparer<T>, IComparer
    {
        class Holder
        {
            public static LambdaComparer<T> ms_instance = new LambdaComparer<T>(
                (_1, _2) => Comparer<T>.Default.Compare(_1, _2)
            );
        }
        public static LambdaComparer<T> Default { get { return Holder.ms_instance; } }

        Func<T, T, int> m_compare;

        public LambdaComparer()
            : this(Default.m_compare)
        {
        }

        public LambdaComparer(Func<T, T, int> compare)
        {
            if (compare == null)
                throw new ArgumentNullException("compare");

            m_compare = compare;
        }
        
        public int Compare(T x, T y)
        {
            return m_compare(x, y);
        }

        int IComparer.Compare(object x, object y)
        {
            var _x = x == null || !(x is T) ? default(T) : (T)x;
            var _y = y == null || !(y is T) ? default(T) : (T)y;
            return Compare(_x, _y);
        }
    }
}
