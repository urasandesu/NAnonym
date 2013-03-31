/* 
 * File: EnumerableMixin.cs
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
using System.Linq;
using Urasandesu.NAnonym.Collections.Generic;

namespace Urasandesu.NAnonym.Mixins.System.Linq
{
    public static class EnumerableMixin
    {
        public static IDictionary MutableForEach(this IDictionary source, Action<IDictionary, DictionaryEntry> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            var entries = new DictionaryEntry[source.Count];
            source.CopyTo(entries, 0);
            for (int i = 0; i < entries.Length; i++)
                action(source, entries[i]);

            return source;
        }

        public static IDictionary<TKey, TValue> MutableForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<IDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            var entries = new KeyValuePair<TKey, TValue>[source.Count];
            source.CopyTo(entries, 0);
            for (int i = 0; i < entries.Length; i++)
                action(source, entries[i]);

            return source;
        }

        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (var entry in dictionary)
                source.Add(entry.Key, entry.Value);

            return source;
        }

        public static IDictionary<TKey, TValue> AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (var entry in dictionary)
                if (source.ContainsKey(entry.Key))
                    source[entry.Key] = entry.Value;
                else
                    source.Add(entry.Key, entry.Value);

            return source;
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> equals)
        {
            return first.Union(second, new LambdaEqualityComparer<TSource>(equals));
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, int> getHashCode)
        {
            return first.Union(second, new LambdaEqualityComparer<TSource>(getHashCode));
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> equals, Func<TSource, int> getHashCode)
        {
            return first.Union(second, new LambdaEqualityComparer<TSource>(equals, getHashCode));
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> equals)
        {
            return source.Contains(value, new LambdaEqualityComparer<TSource>(equals));
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, int> getHashCode)
        {
            return source.Contains(value, new LambdaEqualityComparer<TSource>(getHashCode));
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> equals, Func<TSource, int> getHashCode)
        {
            return source.Contains(value, new LambdaEqualityComparer<TSource>(equals, getHashCode));
        }
    }
}
