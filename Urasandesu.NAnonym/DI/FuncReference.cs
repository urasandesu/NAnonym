using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public delegate Func<TResult> FuncReference<TBase, TResult>(TBase @base);
    public delegate Func<T, TResult> FuncReference<TBase, T, TResult>(TBase @base);
    public delegate Func<T1, T2, TResult> FuncReference<TBase, T1, T2, TResult>(TBase @base);
    public delegate Func<T1, T2, T3, TResult> FuncReference<TBase, T1, T2, T3, TResult>(TBase @base);
    public delegate Func<T1, T2, T3, T4, TResult> FuncReference<TBase, T1, T2, T3, T4, TResult>(TBase @base);
}
