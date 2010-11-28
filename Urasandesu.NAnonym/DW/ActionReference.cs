using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    public delegate Action ActionReference<TBase>(TBase @base);
    public delegate Action<T> ActionReference<TBase, T>(TBase @base);
    public delegate Action<T1, T2> ActionReference<TBase, T1, T2>(TBase @base);
    public delegate Action<T1, T2, T3> ActionReference<TBase, T1, T2, T3>(TBase @base);
    public delegate Action<T1, T2, T3, T4> ActionReference<TBase, T1, T2, T3, T4>(TBase @base);
}
