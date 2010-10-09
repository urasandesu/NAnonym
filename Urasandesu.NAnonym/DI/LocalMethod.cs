using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    // TODO: LocalMethod は Action 系と Func 系に分ける。
    // TODO: LocalMethod で Instead はできない。Override と明示的な実装のみ。
    public class LocalMethod<TBase> where TBase : class
    {
        public LocalClass<TBase> Override(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalMethod<TBase, TResult> where TBase : class
    {
        public LocalClass<TBase> Override(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalMethod<TBase, T, TResult>
    {
        readonly LocalClass<TBase> localClass;
        readonly MethodInfo oldMethod;
        //readonly Func<T, TResult> func;

        //public LocalMethod(LocalClass<TBase> localClass, Func<T, TResult> func)
        //{
        //    this.localClass = localClass;
        //    this.func = func;
        //}

        //public LocalClass<TBase> Override(Expression<Func<TBase, Func<T, TResult>>> expression)
        //{
        //    var method = DependencyUtil.ExtractMethod(expression);
        //    var targetMethod = typeof(TBase).GetMethod(method);
        //    localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Override, targetMethod, func.Method));
        //    return localClass;
        //}

        //public LocalClass<TBase> Implement(Expression<Func<TBase, Func<T, TResult>>> expression)
        //{
        //    var method = DependencyUtil.ExtractMethod(expression);
        //    var targetMethod = typeof(TBase).GetMethod(method);
        //    localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Implement, targetMethod, func.Method));
        //    return localClass;
        //}

        public LocalMethod(LocalClass<TBase> localClass, MethodInfo oldMethod)
        {
            this.localClass = localClass;
            this.oldMethod = oldMethod;
        }

        public LocalClass<TBase> IsImplementedBy(Func<T, TResult> newFunc)
        {
            localClass.TargetInfoSet.Add(new TargetMethodInfo(SetupMode.Implement, oldMethod, newFunc.Method, newFunc.GetType()));
            return localClass;
        }
    }

}
