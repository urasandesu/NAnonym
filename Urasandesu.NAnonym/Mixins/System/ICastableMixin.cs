/* 
 * File: ICastableMixin.cs
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

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class ICastableMixin
    {
        public static TDestination CastTo<S, TDestination>(this ICastable<S> @this, out TDestination other)
            where TDestination : ICastable<S>, new()
        {
            return CastTo(@this, _ => _.Source, (_1, _2) => _1.TrySetSourceWithCast(_2), out other);
        }

        public static TDestination CastTo<TSource, S, TDestination>(TSource @this, Func<TSource, S> getter, Func<TDestination, S, bool> setter, out TDestination other)
            where TDestination : TSource, new()
        {
            if (!TryCastTo(@this, getter, setter, out other))
                throw new InvalidCastException(string.Format("Cannot convert value \"{0}\" to type \"{1}\".", @this, typeof(TDestination)));
            return other;
        }

        public static bool TryCastTo<S, TDestination>(this ICastable<S> @this, out TDestination other)
            where TDestination : ICastable<S>, new()
        {
            return TryCastTo(@this, _ => _.Source, (_1, _2) => _1.TrySetSourceWithCast(_2), out other);
        }

        public static bool TryCastTo<TSource, S, TDestination>(TSource @this, Func<TSource, S> getter, Func<TDestination, S, bool> setter, out TDestination other)
            where TDestination : TSource, new()
        {
            other = default(TDestination);
            if (@this == null)
                return true;

            if (@this is TDestination)
            {
                other = (TDestination)@this;
                return true;
            }

            var result = new TDestination();
            if (setter(result, getter(@this)))
            {
                other = result;
                return true;
            }

            return false;
        }
    }
}
