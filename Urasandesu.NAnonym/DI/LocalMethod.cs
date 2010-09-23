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
        //[Obsolete]
        //public LocalClass<TBase> As(Action method)
        //{
        //    throw new NotImplementedException();
        //}

        public LocalClass<TBase> Override(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }

        // MEMO: LocalClass で Instead はムリぽ。
        //public LocalClass<TBase> Instead(Expression<Func<TBase, Action>> expression)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class LocalMethod<TBase, TResult> where TBase : class
    {
        //[Obsolete]
        //public LocalClass<TBase> As(Func<TResult> method)
        //{
        //    throw new NotImplementedException();
        //}

        public LocalClass<TBase> Override(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }

        // MEMO: LocalClass で Instead はムリぽ。
        //public LocalClass<TBase> Instead(Expression<Func<TBase, Func<TResult>>> expression)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class LocalMethod<TBase, T, TResult> where TBase : class
    {
        readonly LocalClass<TBase> localClass;
        readonly Func<T, TResult> func;

        public LocalMethod(LocalClass<TBase> localClass, Func<T, TResult> func)
        {
            this.localClass = localClass;
            this.func = func;
        }

        //[Obsolete]
        //public LocalClass<TBase> As(Func<T, TResult> method)
        //{
        //    throw new NotImplementedException();
        //}

        public LocalClass<TBase> Override(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            var method = (MethodInfo)((ConstantExpression)(
                (MethodCallExpression)(((UnaryExpression)expression.Body).Operand)).Arguments[2]).Value;
            var targetMethod = typeof(TBase).GetMethod(method);
            localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Override, targetMethod, func.Method));
            return localClass;
        }

        // MEMO: LocalClass で Instead はムリぽ。
        //public LocalClass<TBase> Instead(Expression<Func<TBase, Func<T, TResult>>> expression)
        //{
        //    throw new NotImplementedException();
        //}

        public LocalClass<TBase> Implement(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            var method = DependencyUtil.ExtractMethod(expression);
            //var method = (MethodInfo)((ConstantExpression)(
            //    (MethodCallExpression)(((UnaryExpression)expression.Body).Operand)).Arguments[2]).Value;
            var targetMethod = typeof(TBase).GetMethod(method);
            localClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Implement, targetMethod, func.Method));
            return localClass;
        }
    }

}
