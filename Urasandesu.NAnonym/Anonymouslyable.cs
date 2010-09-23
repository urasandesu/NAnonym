using System;
using System.Collections.Generic;
//using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace Urasandesu.NAnonym
{
    public static class Anonymouslyable
    {
        /// <summary>
        /// 指定された情報を引数に取る処理を生成する。
        /// </summary>
        /// <typeparam name="T">引数の型。</typeparam>
        /// <param name="obj">引数の型のオブジェクト。</param>
        /// <param name="action">指定された情報を引数に取る処理。</param>
        /// <returns>指定された情報を引数に取る処理。</returns>
        public static Action<T> CreateAction<T>(T obj, Action<T> action)
        {
            return action;
        }



        /// <summary>
        /// 指定された情報を引数に取る処理を生成する。
        /// </summary>
        /// <typeparam name="T1">第1引数の型。</typeparam>
        /// <typeparam name="T2">第2引数の型。</typeparam>
        /// <param name="arg1">第1引数の型のオブジェクト。</param>
        /// <param name="arg2">第2引数の型のオブジェクト。</param>
        /// <param name="action">指定された情報を引数に取る処理。</param>
        /// <returns>指定された情報を引数に取る処理。</returns>
        public static Action<T1, T2> CreateAction<T1, T2>(T1 arg1, T2 arg2, Action<T1, T2> action)
        {
            return action;
        }



        /// <summary>
        /// 指定された情報を引数に取り、結果を返す処理を生成する。
        /// </summary>
        /// <typeparam name="TResult">結果の型。</typeparam>
        /// <param name="result">結果の型のオブジェクト。</param>
        /// <param name="func">指定された情報を引数に取り、結果を返す処理。</param>
        /// <returns>指定された情報を引数に取り、結果を返す処理。</returns>
        public static Func<TResult> CreateFunc<TResult>(TResult result, Func<TResult> func)
        {
            return func;
        }



        /// <summary>
        /// 指定された情報を引数に取り、結果を返す処理を生成する。
        /// </summary>
        /// <typeparam name="T">引数の型。</typeparam>
        /// <typeparam name="TResult">結果の型。</typeparam>
        /// <param name="obj">引数の型のオブジェクト。</param>
        /// <param name="result">結果の型のオブジェクト。</param>
        /// <param name="func">指定された情報を引数に取り、結果を返す処理。</param>
        /// <returns>指定された情報を引数に取り、結果を返す処理。</returns>
        public static Func<T, TResult> CreateFunc<T, TResult>(T obj, TResult result, Func<T, TResult> func)
        {
            return func;
        }



        /// <summary>
        /// 指定された情報を引数に取り、結果を返す処理を生成する。
        /// </summary>
        /// <typeparam name="T1">第1引数の型。</typeparam>
        /// <typeparam name="T2">第2引数の型。</typeparam>
        /// <typeparam name="TResult">結果の型。</typeparam>
        /// <param name="arg1">第1引数の型のオブジェクト。</param>
        /// <param name="arg2">第2引数の型のオブジェクト。</param>
        /// <param name="result">結果の型のオブジェクト。</param>
        /// <param name="func">指定された情報を引数に取り、結果を返す処理。</param>
        /// <returns>指定された情報を引数に取り、結果を返す処理。</returns>
        public static Func<T1, T2, TResult> CreateFunc<T1, T2, TResult>(T1 arg1, T2 arg2, TResult result, Func<T1, T2, TResult> func)
        {
            return func;
        }

        //public static bool EqualsEx<T1, T2>(this T1 x, T2 y)
        //{
        //    return Urasandesu.NAnonym.Linq.EqualityComparer<T1, T2>.Default.Equals(x, y);
        //}


        public static S NotDefault<T, S>(this T obj, Func<T, S> f)
        {
            Required.NotDefault(f, () => f);
            return obj.IsDefault() ? default(S) : f(obj);
        }


        public static IEnumerable<T> Enumerate<T>(this T obj, Func<T, T> next)
        {
            //if (next == null) throw new ArgumentNullException("next");
            //if (obj == null) yield break;

            //for (T _obj = next(obj); _obj != null; _obj = next(_obj))
            //    yield return _obj;
            return Enumerate(obj, next, _obj => _obj.IsDefault());
        }

        // MEMO: これらを参考に、EqualityComparer の boxing 回避版を作成するべし！！
        // MEMO: 仮想メソッド使ってるので効率が相殺されちゃってる…orz Expression を使って書き換え！
        public static IEnumerable<T> Repeat<T>(T element)
        {
            while (true)
            {
                yield return element;
            }
        }

        public static IEnumerable<T> Repeat<T>(Func<T> elementInitializer)
        {
            return RepeatImpl<T>.Default.Repeat(elementInitializer);
        }

        private class RepeatImpl<T>
        {
            public virtual IEnumerable<T> Repeat(Func<T> elementInitializer)
            {
                var element = elementInitializer();
                while (true)
                {
                    yield return element;
                }
            }
            public static readonly RepeatImpl<T> Default;
            static RepeatImpl()
            {
                Type t = typeof(T);
                if (typeof(IDisposable).IsAssignableFrom(t))
                {
                    var implType = typeof(RepeatImplWithUsing<>).MakeGenericType(t);
                    Default = Activator.CreateInstance(implType) as RepeatImpl<T>;
                }
                else
                {
                    Default = new RepeatImpl<T>();
                }
            }
        }
        private sealed class RepeatImplWithUsing<U> : RepeatImpl<U> where U : IDisposable
        {
            public override IEnumerable<U> Repeat(Func<U> elementInitializer)
            {
                using (var element = elementInitializer())
                {
                    while (true)
                    {
                        yield return element;
                    }
                }
            }
        }

        public static IEnumerable<T> Enumerate<T>(this T obj, Func<T, T> next, Predicate<T> breaking)
        {
            for (T _obj = next(obj); !breaking(_obj); _obj = next(_obj))
                yield return _obj;
        }

        public static IEnumerable<T> Recursion<T>(this T obj, Func<T, IEnumerable<T>> nextChildren) 
        {
            //if (nextChildren == null) throw new ArgumentNullException("nextChildren");
            //if (obj == null) yield break;

            foreach (var child in nextChildren(obj))
            {
                yield return child;

                foreach (var childchild in Recursion(child, nextChildren))
                {
                    yield return childchild;
                }
            }
        }

        public static IEnumerable<TChild> Recursion<T, TChild>(
            this T obj, Func<T, IEnumerable<T>> nextSibling, Func<T, IEnumerable<TChild>> nextChildren)
            //where T : class
            //where TChild : class
        {
            //if (nextChildren == null) throw new ArgumentNullException("nextChildren");
            //if (nextSibling == null) throw new ArgumentNullException("nextSibling");
            //if (obj == null) yield break;

            foreach (var sibling in nextSibling(obj))
            {
                foreach (var child in nextChildren(sibling))
                {
                    yield return child;

                    foreach (var childchild in Recursion(sibling, nextSibling, nextChildren))
                    {
                        yield return childchild;
                    }
                }
            }
        }

        /*
        public static TypeRef<T> GetTypeRef<T>(this T obj)
        {
            return new TypeRef<T>();
        }*/

        public static bool IsDefault<T>(this T obj)
        {
            return obj.EqualsNullable(default(T));
        }

        public static int GetHashCodeNoBoxing<T>(this T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        public static int GetHashCodeOrDefault<T>(this T obj)
        {
            return obj.IsDefault() ? 0 : obj.GetHashCodeNoBoxing();
        }

        public static bool EqualsNullable<T>(this T that, T obj)
        {
            return EqualityComparer<T>.Default.Equals(that, obj);
        }

        public static bool EqualsNullable<T>(this T that, object obj)
        {
            if (that.IsDefault() && obj == null)
            {
                return true;
            }
            else if (that.IsDefault() || obj == null || !(obj is T))
            {
                return false;
            }
            else
            {
                return that.EqualsNullable((T)obj);
            }
        }

        public static bool EqualsNullable<T, TTarget>(this T that, object obj, Func<T, TTarget> selector)
        {
            Required.NotDefault(selector, () => selector);

            if (that.IsDefault() && obj == null)
            {
                return true;
            }
            else if (that.IsDefault() || obj == null || !(obj is T))
            {
                return false;
            }
            else
            {
                var thatTarget = selector(that);
                var objTarget = selector((T)obj);

                if (thatTarget.IsDefault() && objTarget.IsDefault())
                {
                    return true;
                }
                else if (thatTarget.IsDefault() || objTarget.IsDefault())
                {
                    return false;
                }
                else
                {
                    return thatTarget.EqualsNullable(objTarget);
                }
            }
        }
    }

    public static class Tuple
    {
        public static Tuple1<T1> Create<T1>(T1 item1)
        {
            return new Tuple1<T1>(item1);
        }

        public static Tuple2<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple2<T1, T2>(item1, item2);
        }

        public static Tuple3<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple3<T1, T2, T3>(item1, item2, item3);
        }

        public static Tuple4<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple4<T1, T2, T3, T4>(item1, item2, item3, item4);
        }
    }

    public sealed class Tuple1<T1>
    {
        public T1 Item1 { get; private set; }
        public Tuple1(T1 item1)
        {
            this.Item1 = item1;
        }
    }

    public sealed class Tuple2<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public Tuple2(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }

    public sealed class Tuple3<T1, T2, T3>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }
        public Tuple3(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }

    public sealed class Tuple4<T1, T2, T3, T4>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }
        public T4 Item4 { get; private set; }
        public Tuple4(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }
    }

}
