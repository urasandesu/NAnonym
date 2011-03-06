/* 
 * File: Anonymable.cs
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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym
{
    public static class Anonymable
    {
        public static Action<T> CreateAction<T>(T obj, Action<T> action)
        {
            return action;
        }



        public static Action<T1, T2> CreateAction<T1, T2>(T1 arg1, T2 arg2, Action<T1, T2> action)
        {
            return action;
        }



        public static Func<TResult> CreateFunc<TResult>(TResult result, Func<TResult> func)
        {
            return func;
        }



        public static Func<T, TResult> CreateFunc<T, TResult>(T obj, TResult result, Func<T, TResult> func)
        {
            return func;
        }



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

        public static IEnumerable<T> Enumerate<T>(this T obj, Func<T, T> next, Predicate<T> breaker)
        {
            for (T _obj = next(obj); !breaker(_obj); _obj = next(_obj))
                yield return _obj;
        }

        public static IEnumerable<TComposite> Recursion<TComposite>(this TComposite source, Func<TComposite, IEnumerable<TComposite>> nextComposites)
        {
            foreach (var composite in nextComposites(source))
            {
                yield return composite;

                foreach (var grandComposite in Recursion(composite, nextComposites))
                {
                    yield return grandComposite;
                }
            }
        }

        public static IEnumerable<TContent> Recursion<TComposite, TContent>(
            this TComposite source, Func<TComposite, IEnumerable<TComposite>> nextComposites, Func<TComposite, IEnumerable<TContent>> nextContents)
        {
            foreach (var content in nextContents(source))
            {
                yield return content;
            }

            foreach (var composite in nextComposites(source))
            {
                foreach (var grandContent in Recursion(composite, nextComposites, nextContents))
                {
                    yield return grandContent;
                }
            }
        }

        public static bool IsDefault<T>(this T obj)
        {
            return obj.NullableEquals(default(T));
        }

        public static int GetHashCodeNoBoxing<T>(this T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        public static int NullableGetHashCode<T>(this T obj)
        {
            return obj.IsDefault() ? 0 : obj.GetHashCodeNoBoxing();
        }

        public static bool NullableEquals<T>(this T that, T obj)
        {
            return EqualityComparer<T>.Default.Equals(that, obj);
        }

        public static bool NullableEquals<T>(this T that, object obj)
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
                return that.NullableEquals((T)obj);
            }
        }

        public static bool NullableEquals<T, TTarget>(this T that, object obj, Func<T, TTarget> selector)
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
                    return thatTarget.NullableEquals(objTarget);
                }
            }
        }

        public static string NullableToString<T>(this T obj)
        {
            if (typeof(T).IsValueType)
            {
                return obj.ToString();
            }
            else
            {
                return obj.IsDefault() ? "null" : obj.ToString();
            }
        }

        public static Type NullableGetType<T>(this T obj)
        {
            return obj.IsDefault() ? typeof(T) : obj.GetType();
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

    // MEMO: とりあえずこのライブラリ用の例外。
    [Serializable]
    public class NAnonymException : Exception
    {
        public NAnonymException() { }
        public NAnonymException(string message) : base(message) { }
        public NAnonymException(string message, Exception inner) : base(message, inner) { }
        protected NAnonymException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}

