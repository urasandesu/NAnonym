using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Urasandesu.NAnonym.DI
{
    public class LocalMethod<TBase> where TBase : class
    {
        [Obsolete]
        public LocalClass<TBase> As(Action method)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Override(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Instead(Expression<Func<TBase, Action>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalMethod<TBase, TResult> where TBase : class
    {
        [Obsolete]
        public LocalClass<TBase> As(Func<TResult> method)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Override(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Instead(Expression<Func<TBase, Func<TResult>>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalMethod<TBase, T, TResult> where TBase : class
    {
        [Obsolete]
        public LocalClass<TBase> As(Func<T, TResult> method)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Override(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Instead(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            throw new NotImplementedException();
        }
    }

}
