using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions.Linq
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

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> proc)
        {
            foreach (var item in source)
            {
                if (!proc(item)) 
                    break;
            }
        }


        /// <summary>
        /// シーケンスに指定された処理を適用する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="source">入力シーケンス。</param>
        /// <param name="action">適用する処理。</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            foreach (var item in source.Select((item, index) => new { Item = item, Index = index }))
            {
                action(item.Item, item.Index);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> proc)
        {
            foreach (var item in source.Select((item, index) => new { Item = item, Index = index }))
            {
                if (!proc(item.Item, item.Index)) 
                    break;
            }
        }


        /// <summary>
        /// 要素の重複が可能なリストに対する差リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second には含まれていないが、返される要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれ、返されたシーケンスからは削除される要素を含む IEnumerable&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの要素の差リストが格納されているシーケンス。</returns>
        public static IEnumerable<TSource> Negate<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return Negate(first, second, EqualityComparer<TSource>.Default);
        }


        /// <summary>
        /// 要素の重複が可能なリストに対する差リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second には含まれていないが、返される要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれ、返されたシーケンスからは削除される要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="comparer">値を比較する IEqualityComparer&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの要素の差リストが格納されているシーケンス。</returns>
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

        
        
        /// <summary>
        /// 要素の重複が可能なリストに対する和リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">和リストの最初のセットを形成する要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">和リストの 2 番目のセットを形成する一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <returns>2 つの入力シーケンスの要素 (2 番目のセットの重複する要素は除く) を格納している IEnumerable&lt;T&gt;。</returns>
        public static IEnumerable<TSource> Replenish<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return Replenish(first, second, EqualityComparer<TSource>.Default);
        }



        /// <summary>
        /// 要素の重複が可能なリストに対する和リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">和リストの最初のセットを形成する要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">和リストの 2 番目のセットを形成する一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="comparer">値を比較する IEqualityComparer&lt;T&gt;。</param>
        /// <returns>2 つの入力シーケンスの要素 (2 番目のセットの重複する要素は除く) を格納している IEnumerable&lt;T&gt;。</returns>
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


        
        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="merger">両方のシーケンスに含まれる値を、どのようにしてマージするかの処理。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
        public static IEnumerable<TSource> Cross<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, TSource> merger)
        {
            return Cross(first, second, EqualityComparer<TSource>.Default, merger);
        }



        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="merger">両方のシーケンスに含まれる値を、どのようにしてマージするかの処理。</param>
        /// <param name="comparer">値を比較する IEqualityComparer&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
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



        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// 両方のシーケンスに含まれる値については、デフォルトで最初のシーケンスの値が利用される。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
        public static IEnumerable<TSource> CrossLeft<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return CrossLeft(first, second, EqualityComparer<TSource>.Default);
        }



        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// 両方のシーケンスに含まれる値については、デフォルトで最初のシーケンスの値が利用される。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="comparer">値を比較する IEqualityComparer&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
        public static IEnumerable<TSource> CrossLeft<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return Cross(first, second, comparer, (firstItem, secondItem) => firstItem);
        }



        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// 両方のシーケンスに含まれる値については、デフォルトで second のシーケンスの値が利用される。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
        public static IEnumerable<TSource> CrossRight<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return CrossRight(first, second, EqualityComparer<TSource>.Default);
        }



        /// <summary>
        /// 要素の重複が可能なリストに対する積リストを生成する。
        /// 両方のシーケンスに含まれる値については、デフォルトで second のシーケンスの値が利用される。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="first">second にも含まれる、返されるの要素を含む IEnumerable&lt;T&gt;。重複が有効なリスト。</param>
        /// <param name="second">最初のシーケンスにも含まれる、返される一意の要素を含む IEnumerable&lt;T&gt;。</param>
        /// <param name="comparer">値を比較する IEqualityComparer&lt;T&gt;。</param>
        /// <returns>2 つのシーケンスの積リストを構成する要素が格納されているシーケンス。</returns>
        public static IEnumerable<TSource> CrossRight<TSource>(
            this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return Cross(first, second, comparer, (firstItem, secondItem) => secondItem);
        }






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
    }
}
