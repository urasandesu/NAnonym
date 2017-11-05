/* 
 * File: EnumerableMixin.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using Urasandesu.NAnonym.Collections.Generic;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.Collections.Linq
{
    public static class EnumerableMixin
    {
        public static bool StartsWith<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                return false;

            var e1 = first.GetEnumerator();
            var e2 = second.GetEnumerator();
            while (e2.MoveNext())
            {
                if (!e1.MoveNext())
                    return false;

                if (!object.Equals(e1.Current, e2.Current))
                    return false;
            }

            return true;
        }

        public static bool NullableDictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == null && second == null)
                return true;
            else if (first == null || second == null)
                return false;
            else if (object.ReferenceEquals(first, second))
                return true;

            var keyComparer = BaseEqualityComparer<KeyValuePair<TKey, TValue>>.Make((_1, _2) => object.Equals(_1.Key, _2.Key), _ => _.Key.NullableGetHashCode());
            return first.Count == second.Count && !first.Except(second, keyComparer).Any();
        }

        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return NullableSequenceEqual(first, second, BaseEqualityComparer<TSource>.Make());
        }

        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null && second == null)
                return true;
            else if (first == null || second == null)
                return false;
            else if (object.ReferenceEquals(first, second))
                return true;

            return first.SequenceEqual(second, comparer);
        }

        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource> source)
        {
            return source.Where(_ => _ != null);
        }

        public static int NullableSequenceGetHashCode<TSource>(this IEnumerable<TSource> source)
        {
            return NullableSequenceGetHashCode(source, _ => _.GetHashCode());
        }

        public static int NullableSequenceGetHashCode<TSource>(this IEnumerable<TSource> source, Func<TSource, int> hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            if (source == null)
                return 0;

            return source.Aggregate(0, (result, next) => result ^ (next != null ? hash(next) : 0));
        }

        public static int NullableDictionaryGetHashCode<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            if (source == null)
                return 0;

            return NullableSequenceGetHashCode(source, _ => ObjectMixin.GetHashCode(_.Key));
        }

        class EmptyReadOnlyCollectionHolder<T>
        {
            public static readonly ReadOnlyCollection<T> Value = new ReadOnlyCollection<T>(new T[0]);
        }
        public static ReadOnlyCollection<T> EmptyReadOnlyCollection<T>()
        {
            return EmptyReadOnlyCollectionHolder<T>.Value;
        }
    }
}
