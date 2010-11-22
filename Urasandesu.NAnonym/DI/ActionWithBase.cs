using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public delegate void ActionWithBase(Action baseAction);
    public delegate void ActionWithBase<T>(Action<T> baseAction, T arg);
    public delegate void ActionWithBase<T1, T2>(Action<T1, T2> baseAction, T1 arg1, T2 arg2);
    public delegate void ActionWithBase<T1, T2, T3>(Action<T1, T2, T3> baseAction, T1 arg1, T2 arg2, T3 arg3);
    public delegate void ActionWithBase<T1, T2, T3, T4>(Action<T1, T2, T3, T4> baseAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}
