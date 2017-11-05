/* 
 * File: BaseEqualityComparer`1.cs
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
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.Collections.Generic
{
    public abstract class BaseEqualityComparer<T>
    {
        public static IEqualityComparer<T> Make()
        {
            return Make(_ => _, (_1, _2) => Equals(_1, _2), _ => _.NullableGetHashCode());
        }

        public static IEqualityComparer<T> Make<TProp>(Func<T, TProp> selector, Func<TProp, TProp, bool> equals, Func<TProp, int> getHashCode)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if (equals == null)
                throw new ArgumentNullException("equals");

            if (getHashCode == null)
                throw new ArgumentNullException("getHashCode");

            return new DelegateEqualityComparer<T, TProp>(selector, equals, getHashCode);
        }

        public static IEqualityComparer<T> Make(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            if (equals == null)
                throw new ArgumentNullException("equals");

            if (getHashCode == null)
                throw new ArgumentNullException("getHashCode");

            return new DelegateEqualityComparer<T, T>(_ => _, equals, getHashCode);
        }
    }
}

