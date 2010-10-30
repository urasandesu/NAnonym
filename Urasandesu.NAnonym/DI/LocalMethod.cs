using System;
using System.Reflection;

namespace Urasandesu.NAnonym.DI
{


    public abstract class LocalMethod : DependencyMethod
    {
        public LocalMethod(LocalClass localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }
    }


    public class LocalFunc<TBase, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<TResult> newFunc)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newFunc);
        }
    }

    public class LocalFunc<TBase, T, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T, TResult> newFunc)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newFunc);
        }
    }

    public class LocalFunc<TBase, T1, T2, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, TResult> newFunc)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newFunc);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, TResult> newFunc)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newFunc);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, T4, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, T4, TResult> newFunc)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newFunc);
        }
    }

    public class LocalAction<TBase> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action newAction)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newAction);
        }
    }

    public class LocalAction<TBase, T> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T> newAction)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newAction);
        }
    }

    public class LocalAction<TBase, T1, T2> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2> newAction)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newAction);
        }
    }

    public class LocalAction<TBase, T1, T2, T3> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3> newAction)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newAction);
        }
    }

    public class LocalAction<TBase, T1, T2, T3, T4> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo oldMethod)
            : base(localClass, oldMethod)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3, T4> newAction)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)newAction);
        }
    }

    //// TODO: LocalMethod は Action 系と Func 系に分ける。
    //// TODO: LocalMethod で Instead はできない。Override と明示的な実装のみ。
    //public class LocalMethod<TBase> where TBase : class
    //{
    //    public LocalClass<TBase> Override(Expression<Func<TBase, Action>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public LocalClass<TBase> Implement(Expression<Func<TBase, Action>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class LocalMethod<TBase, TResult> where TBase : class
    //{
    //    public LocalClass<TBase> Override(Expression<Func<TBase, Func<TResult>>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public LocalClass<TBase> Implement(Expression<Func<TBase, Func<TResult>>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class LocalMethod<TBase, T, TResult>
    //{
    //    readonly LocalClass<TBase> localClass;
    //    readonly MethodInfo oldMethod;
    //    //readonly Func<T, TResult> func;

    //    //public LocalMethod(LocalClass<TBase> localClass, Func<T, TResult> func)
    //    //{
    //    //    this.localClass = localClass;
    //    //    this.func = func;
    //    //}

    //    //public LocalClass<TBase> Override(Expression<Func<TBase, Func<T, TResult>>> expression)
    //    //{
    //    //    var method = DependencyUtil.ExtractMethod(expression);
    //    //    var targetMethod = typeof(TBase).GetMethod(method);
    //    //    localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Override, targetMethod, func.Method));
    //    //    return localClass;
    //    //}

    //    //public LocalClass<TBase> Implement(Expression<Func<TBase, Func<T, TResult>>> expression)
    //    //{
    //    //    var method = DependencyUtil.ExtractMethod(expression);
    //    //    var targetMethod = typeof(TBase).GetMethod(method);
    //    //    localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Implement, targetMethod, func.Method));
    //    //    return localClass;
    //    //}

    //    public LocalMethod(LocalClass<TBase> localClass, MethodInfo oldMethod)
    //    {
    //        this.localClass = localClass;
    //        this.oldMethod = oldMethod;
    //    }

    //    public LocalClass<TBase> IsImplementedBy(Func<T, TResult> newFunc)
    //    {
    //        localClass.TargetMethodInfoSet.Add(new TargetMethodInfo(SetupModes.Implement, oldMethod, newFunc.Method, newFunc.GetType()));
    //        return localClass;
    //    }
    //}

}
