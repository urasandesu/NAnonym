using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions.Linq
{
    public static class Iterable
    {
        public static void Foreach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }


        /// <summary>
        /// シーケンスに指定された処理を適用する。
        /// </summary>
        /// <typeparam name="TSource">入力シーケンスの要素の型。</typeparam>
        /// <param name="source">入力シーケンス。</param>
        /// <param name="action">適用する処理。</param>
        public static void Foreach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            foreach (var item in source.Select((item, index) => new { Item = item, Index = index }))
            {
                action(item.Item, item.Index);
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
            return Negate(first, second, CreateEqualityComparer(default(TSource)));
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
            return Replenish(first, second, CreateEqualityComparer(default(TSource)));
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
            return Cross(first, second, merger, CreateEqualityComparer(default(TSource)));
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
            this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, TSource> merger, 
            IEqualityComparer<TSource> comparer)
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
            return CrossLeft(first, second, CreateEqualityComparer(default(TSource)));
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
            return Cross(first, second, (firstItem, secondItem) => firstItem, comparer);
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
            return CrossRight(first, second, CreateEqualityComparer(default(TSource)));
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
            return Cross(first, second, (firstItem, secondItem) => secondItem, comparer);
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

        public static List<T> CreateList<T>(T obj)
        {
            return new List<T>();
        }
    }
}
