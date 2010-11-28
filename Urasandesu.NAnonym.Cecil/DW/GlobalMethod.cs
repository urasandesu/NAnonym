using System;
using System.Reflection;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    public abstract class GlobalMethod
    {
        readonly GlobalClass globalClass;
        readonly MethodInfo source;

        public GlobalMethod(GlobalClass globalClass, MethodInfo source)
        {
            this.globalClass = globalClass;
            this.source = source;
        }

        public GlobalClass IsReplacedBy(Delegate @delegate)
        {
            globalClass.MethodSet.Add(new WeaveMethodInfo(SetupModes.Replace, source, @delegate.Method, @delegate.GetType()));
            return globalClass;
        }
    }

    public class GlobalFunc<TBase, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }
    
    public class GlobalFunc<TBase, T, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalFunc<TBase, T1, T2, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }

        public GlobalClass<TBase> IsReplacedBy(FuncWithBase<T1, T2, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalFunc<TBase, T1, T2, T3, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalFunc<TBase, T1, T2, T3, T4, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, T4, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3, T4> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3, T4> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }
}
