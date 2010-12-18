/* 
 * File: Iterable.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Urasandesu.NAnonym.Linq
{
    public static class Iterable
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            foreach (var item in source)
            {
                if (!predicate(item)) 
                    break;
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            foreach (var item in source.Select((item, index) => new { Item = item, Index = index }))
            {
                action(item.Item, item.Index);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicateWithAction)
        {
            foreach (var item in source.Select((item, index) => new { Item = item, Index = index }))
            {
                if (!predicateWithAction(item.Item, item.Index)) 
                    break;
            }
        }

        public static void NullableForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) return;
            source.ForEach(action);
        }

        public static void NullableForEach<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null) return;
            source.ForEach(predicate);
        }

        public static void NullableForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null) return;
            source.ForEach(action);
        }

        public static void NullableForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicateWithAction)
        {
            if (source == null) return;
            source.ForEach(predicateWithAction);
        }

        public static IEnumerable<TSource> Negate<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return Negate(first, second, EqualityComparer<TSource>.Default);
        }


        public static IEnumerable<TSource> Negate<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            var secondHashSet = new HashSet<TSource>(comparer);
            foreach (var secondItem in second)
            {
                if (secondHashSet.Contains(secondItem)) continue;
                secondHashSet.Add(secondItem);
            }

            foreach (var firstItem in first)
            {
                if (secondHashSet.Contains(firstItem)) continue;
                yield return firstItem;
            }
        }

        
        
        public static IEnumerable<TSource> Replenish<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return Replenish(first, second, EqualityComparer<TSource>.Default);
        }



        public static IEnumerable<TSource> Replenish<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            var firstHashSet = new HashSet<TSource>(comparer);
            foreach (var firstItem in first)
            {
                if (!firstHashSet.Contains(firstItem)) firstHashSet.Add(firstItem);
                yield return firstItem;
            }

            foreach (var secondItem in second)
            {
                if (firstHashSet.Contains(secondItem)) continue;
                yield return secondItem;
            }
        }


        
        public static IEnumerable<TSource> Cross<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, TSource> merger)
        {
            return Cross(first, second, EqualityComparer<TSource>.Default, merger);
        }



        public static IEnumerable<TSource> Cross<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer, 
            Func<TSource, TSource, TSource> merger)
        {
            var secondDictionary = new Dictionary<TSource, TSource>(comparer);
            foreach (var secondItem in second)
            {
                if (secondDictionary.ContainsKey(secondItem)) continue;
                secondDictionary.Add(secondItem, secondItem);
            }

            foreach (var firstItem in first)
            {
                if (!secondDictionary.ContainsKey(firstItem)) continue;
                yield return merger(firstItem, secondDictionary[firstItem]);
            }
        }



        public static IEnumerable<TSource> CrossLeft<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return CrossLeft(first, second, EqualityComparer<TSource>.Default);
        }



        public static IEnumerable<TSource> CrossLeft<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return Cross(first, second, comparer, (firstItem, secondItem) => firstItem);
        }



        public static IEnumerable<TSource> CrossRight<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return CrossRight(first, second, EqualityComparer<TSource>.Default);
        }



        public static IEnumerable<TSource> CrossRight<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return Cross(first, second, comparer, (firstItem, secondItem) => secondItem);
        }

        public static IEnumerable<TSource> WhereNotDefault<TSource>(this IEnumerable<TSource> source)
        {
            return source.Where(item => !EqualityComparer<TSource>.Default.Equals(item, default(TSource)));
        }

        public static IEnumerable<Tuple2<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                yield return Tuple.Create(firstEnumerator.Current, secondEnumerator.Current);
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource target)
        {
            return source.IndexOf(target, EqualityComparer<TSource>.Default);    
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource target, IEqualityComparer<TSource> equalityComparer)
        {
            int index = 0;
            foreach (var item in source)
            {
                if (equalityComparer.Equals(item, target))
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }

        public static bool Equivalent<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.Equivalent(second, EqualityComparer<TSource>.Default);
        }

        public static bool Equivalent<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> equalityComparer)
        {
            if (first.Count() != second.Count()) return false;
            return first.Zip(second).All(tuple => equalityComparer.Equals(tuple.Item1, tuple.Item2));
        }

        public static bool Equivalent<TSource1, TSource2>(
            this IEnumerable<TSource1> first, IEnumerable<TSource2> second, IEqualityComparer<TSource1, TSource2> equalityComparer)
        {
            if (first.Count() != second.Count()) return false;
            return first.Zip(second).All(tuple => equalityComparer.Equals(tuple.Item1, tuple.Item2));
        }


        public static TSource[] AddRangeTo<TSource>(this IEnumerable<TSource> source, ref TSource[] destinationArray)
        {
            var sourceCollection = default(ICollection<TSource>);
            if ((sourceCollection = source as ICollection<TSource>) != null)
            {
                sourceCollection.AddRangeTo(ref destinationArray, sourceCollection.Count);
            }
            else
            {
                var destinationList = new List<TSource>(destinationArray);
                foreach (var sourceItem in source)
                {
                    destinationList.Add(sourceItem);
                }
                destinationArray = destinationList.ToArray();
            }
            return destinationArray;
        }

        public static ICollection<TSource> AddRangeTo<TSource>(this IEnumerable<TSource> source, ICollection<TSource> destination)
        {
            var sourceCollection = default(ICollection<TSource>);
            if ((sourceCollection = source as ICollection<TSource>) != null)
            {
                sourceCollection.AddRangeTo(destination, sourceCollection.Count);
            }
            else
            {
                foreach (var sourceItem in source)
                {
                    destination.Add(sourceItem);
                }
            }
            return destination;
        }

        public static TSource[] AddRangeTo<TSource>(
            this ICollection<TSource> source, ref TSource[] destinationArray, int length)
        {
            return source.AddRangeTo(0, ref destinationArray, destinationArray.Length, length);
        }

        public static ICollection<TSource> AddRangeTo<TSource>(
            this ICollection<TSource> source, ICollection<TSource> destination, int length)
        {
            var destinationList = default(IList<TSource>);
            if ((destinationList = destination as IList<TSource>) != null)
            {
                return source.AddRangeTo(0, destinationList, destinationList.Count, length);
            }
            else
            {
                return source.AddRangeTo(0, destination, length);
            }
        }

        public static TSource[] AddRangeTo<TSource>(
            this ICollection<TSource> source, int sourceIndex, ref TSource[] destinationArray, int destinationIndex, int length)
        {
            var sourceArray = default(TSource[]);
            var sourceList = default(IList<TSource>);

            int newSize = 0;
            if (destinationArray.Length < (newSize = destinationIndex + length))
            {
                Array.Resize(ref destinationArray, newSize);
            }

            if ((sourceArray = source as TSource[]) != null)
            {
                Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
            }
            else if ((sourceList = source as IList<TSource>) != null)
            {
                for (int index = 0; index < length; index++)
                {
                    destinationArray[destinationIndex++] = sourceList[sourceIndex++];
                }
            }
            else
            {
                var sourceEnumerator = source.Skip(sourceIndex).GetEnumerator();
                for (int index = 0; index < length && sourceEnumerator.MoveNext(); index++)
                {
                    destinationArray[destinationIndex++] = sourceEnumerator.Current;
                }
            }

            return destinationArray;
        }

        public static ICollection<TSource> AddRangeTo<TSource>(
            this ICollection<TSource> source, int sourceIndex, ICollection<TSource> destination, int length)
        {
            var sourceList = default(IList<TSource>);

            if ((sourceList = source as IList<TSource>) != null)
            {
                for (int index = 0; index < length; index++)
                {
                    destination.Add(sourceList[sourceIndex++]);
                }
            }
            else
            {
                var sourceEnumerator = source.Skip(sourceIndex).GetEnumerator();
                for (int index = 0; index < length && sourceEnumerator.MoveNext(); index++)
                {
                    destination.Add(sourceEnumerator.Current);
                }
            }
            return destination;
        }

        public static ICollection<TSource> AddRangeTo<TSource>(
            this ICollection<TSource> source, int sourceIndex, IList<TSource> destinationList, int destinationIndex, int length)
        {
            var sourceList = default(IList<TSource>);

            if ((sourceList = source as IList<TSource>) != null)
            {
                for (int index = 0; index < length; index++)
                {
                    destinationList.Insert(destinationIndex++, sourceList[sourceIndex++]);
                }
            }
            else
            {
                var sourceEnumerator = source.Skip(sourceIndex).GetEnumerator();
                for (int index = 0; index < length && sourceEnumerator.MoveNext(); index++)
                {
                    destinationList.Insert(destinationIndex++, sourceEnumerator.Current);
                }
            }
            return destinationList;
        }


        public static IList<TDestination> TransformEnumerateOnly<TSource, TDestination>(
            this IList<TSource> source, Func<TSource, TDestination> selector)
        {
            return new TransformerEnumerateOnly<TSource, TDestination>(source, selector);
        }

        public static IList<TDestination> Transform<TSource, TDestination>(
            this IList<TSource> source, Func<TSource, TDestination> selector, Func<TDestination, TSource> reSelector)
        {
            return new Transformer<TSource, TDestination>(source, selector, reSelector);
        }

        public static int GetAggregatedHashCode<T>(this IEnumerable<T> source)
        {
            return source.Aggregate(0, (accumelate, next) => accumelate ^ next.GetHashCode());
        }

        public static int GetAggregatedHashCodeOrDefault<T>(this IEnumerable<T> source)
        {
            return source.Aggregate(0, (accumelate, next) => accumelate ^ next.NullableGetHashCode());
        }

        public static decimal? MaxOrDefault(this IEnumerable<decimal?> source) { return source.FirstOrDefault().IsDefault() ? default(decimal?) : source.Max(); }
        public static decimal MaxOrDefault(this IEnumerable<decimal> source) { return source.FirstOrDefault().IsDefault() ? default(decimal) : source.Max(); }
        public static double? MaxOrDefault(this IEnumerable<double?> source) { return source.FirstOrDefault().IsDefault() ? default(double?) : source.Max(); }
        public static double MaxOrDefault(this IEnumerable<double> source) { return source.FirstOrDefault().IsDefault() ? default(double) : source.Max(); }
        public static float? MaxOrDefault(this IEnumerable<float?> source) { return source.FirstOrDefault().IsDefault() ? default(float?) : source.Max(); }
        public static float MaxOrDefault(this IEnumerable<float> source) { return source.FirstOrDefault().IsDefault() ? default(float) : source.Max(); }
        public static int? MaxOrDefault(this IEnumerable<int?> source) { return source.FirstOrDefault().IsDefault() ? default(int?) : source.Max(); }
        public static int MaxOrDefault(this IEnumerable<int> source) { return source.FirstOrDefault().IsDefault() ? default(int) : source.Max(); }
        public static long? MaxOrDefault(this IEnumerable<long?> source) { return source.FirstOrDefault().IsDefault() ? default(long?) : source.Max(); }
        public static long MaxOrDefault(this IEnumerable<long> source) { return source.FirstOrDefault().IsDefault() ? default(long) : source.Max(); }
        public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) { return source.FirstOrDefault().IsDefault() ? default(TSource) : source.Max(); }
        public static decimal? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector) { return source.FirstOrDefault().IsDefault() ? default(decimal?) : source.Max(selector); }
        public static decimal MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) { return source.FirstOrDefault().IsDefault() ? default(decimal) : source.Max(selector); }
        public static double? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) { return source.FirstOrDefault().IsDefault() ? default(double?) : source.Max(selector); }
        public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) { return source.FirstOrDefault().IsDefault() ? default(double) : source.Max(selector); }
        public static float? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector) { return source.FirstOrDefault().IsDefault() ? default(float?) : source.Max(selector); }
        public static float MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) { return source.FirstOrDefault().IsDefault() ? default(float) : source.Max(selector); }
        public static int? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector) { return source.FirstOrDefault().IsDefault() ? default(int?) : source.Max(selector); }
        public static int MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) { return source.FirstOrDefault().IsDefault() ? default(int) : source.Max(selector); }
        public static long? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector) { return source.FirstOrDefault().IsDefault() ? default(long?) : source.Max(selector); }
        public static long MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) { return source.FirstOrDefault().IsDefault() ? default(long) : source.Max(selector); }
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) { return source.FirstOrDefault().IsDefault() ? default(TResult) : source.Max(selector); }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(T obj)
        {
            return new DelegateEqualityComparer<T>();
        }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(T obj, Func<T, int> getHashCode)
        {
            return new DelegateEqualityComparer<T>(getHashCode);
        }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(T obj, Func<T, T, bool> equals)
        {
            return new DelegateEqualityComparer<T>(equals);
        }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(T obj, Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            return new DelegateEqualityComparer<T>(getHashCode, equals);
        }

        public static IEqualityComparer<T> CreateEqualityComparerNullable<T>(T obj)
        {
            return new DelegateEqualityComparer<T>(
                DelegateEqualityComparer<T>.NullableGetHashCode, DelegateEqualityComparer<T>.NullableEquals);
        }

        public static IEqualityComparer<T> CreateEqualityComparerNullable<T>(T obj, Func<T, int> getHashCode)
        {
            return new DelegateEqualityComparer<T>(
                DelegateEqualityComparer<T>.ToNullableGetHashCode(getHashCode), DelegateEqualityComparer<T>.NullableEquals);
        }

        public static IEqualityComparer<T> CreateEqualityComparerNullable<T>(T obj, Func<T, T, bool> equals)
        {
            return new DelegateEqualityComparer<T>(
                DelegateEqualityComparer<T>.NullableGetHashCode, DelegateEqualityComparer<T>.ToNullableEquals(equals));
        }

        public static IEqualityComparer<T> CreateEqualityComparerNullable<T>(T obj, Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            return new DelegateEqualityComparer<T>(
                DelegateEqualityComparer<T>.ToNullableGetHashCode(getHashCode), DelegateEqualityComparer<T>.ToNullableEquals(equals));
        }

        public static IEqualityComparer<T1, T2> CreateEqualityComparerNullable<T1, T2>(
            T1 x, T2 y, Func<T1, T2, bool> equals)
        {
            return new DelegateEqualityComparer<T1, T2>(
                EqualityComparer<T1, T2>.Default.GetHashCode,
                DelegateEqualityComparer<T1, T2>.ToNullableEquals(equals));
        }

        public static IEqualityComparer<T1, T2> CreateEqualityComparerNullable<T1, T2>(
            T1 x, T2 y, Func<T1, T2, int> getHashCode, Func<T1, T2, bool> equals)
        {
            return new DelegateEqualityComparer<T1, T2>(
                DelegateEqualityComparer<T1, T2>.ToNullableGetHashCode(getHashCode), 
                DelegateEqualityComparer<T1, T2>.ToNullableEquals(equals));
        }


        public static IComparer<T> CreateComparer<T>(T obj)
        {
            return new DelegateComparer<T>();
        }

        public static IComparer<T> CreateComparer<T>(T obj, Func<T, T, int> comparer)
        {
            return new DelegateComparer<T>(comparer);
        }

        public static IComparer<T> CreateComparerNullableAsc<T>(T obj)
        {
            return new DelegateComparer<T>(DelegateComparer<T>.NullableComparerAsc);
        }

        public static IComparer<T> CreateComparerNullableAsc<T>(T obj, Func<T, T, int> comparer)
        {
            return new DelegateComparer<T>(DelegateComparer<T>.ToNullableComparerAsc(comparer));
        }

        public static IComparer<T> CreateComparerNullableDsc<T>(T obj)
        {
            return new DelegateComparer<T>(DelegateComparer<T>.NullableComparerDsc);
        }

        public static IComparer<T> CreateComparerNullableDsc<T>(T obj, Func<T, T, int> comparer)
        {
            return new DelegateComparer<T>(DelegateComparer<T>.ToNullableComparerDsc(comparer));
        }

        public static List<T> CreateList<T>(T obj)
        {
            return new List<T>();
        }

        public static void NullableClear<T>(this ICollection<T> source)
        {
            if (source == null) return;
            source.Clear();
        }
    }
}

