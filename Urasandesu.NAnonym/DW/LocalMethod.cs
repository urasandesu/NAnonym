using System;
using System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    public abstract class LocalMethod : DependencyMethod
    {
        public LocalMethod(LocalClass localClass, MethodInfo source)
            : base(localClass, source)
        {
        }
    }


    public class LocalFunc<TBase, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }

        public LocalClass<TBase> IsOverridedBy(FuncWithBase<T1, T2, TResult> destination)
        {
            return (LocalClass<TBase>)IsOverridedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, T4, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, T4, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2, T3> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2, T3, T4> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3, T4> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }
}
