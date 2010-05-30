using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonymousExtensions
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
    }
}
