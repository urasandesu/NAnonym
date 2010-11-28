using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    public delegate TResult FuncWithBase<TResult>(Func<TResult> baseFunc);
    public delegate TResult FuncWithBase<T, TResult>(Func<T, TResult> baseFunc, T arg);
    public delegate TResult FuncWithBase<T1, T2, TResult>(Func<T1, T2, TResult> baseFunc, T1 arg1, T2 arg2);
    public delegate TResult FuncWithBase<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> baseFunc, T1 arg1, T2 arg2, T3 arg3);
    public delegate TResult FuncWithBase<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> baseFunc, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}
