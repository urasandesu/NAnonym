using System;
using System.Linq.Expressions;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{

    public abstract class LocalField : DependencyField
    {
        public LocalField(DependencyClass @class, LambdaExpression fieldReference, Type fieldType)
            : base(@class, fieldReference, fieldType)
        {
        }
    }

    public class LocalField<T> : LocalField
    {
        public LocalField(LocalClass @class, Expression<Func<T>> fieldReference)
            : base(@class, fieldReference, typeof(T))
        {
        }

        public void As(Expression<Func<Expressible, T>> initializer)
        {
            base.As(initializer);
        }
    }

    public class LocalFieldInt : LocalField
    {
        public LocalFieldInt(LocalClass @class, Expression<Func<int>> fieldReference)
            : base(@class, fieldReference, typeof(int))
        {
        }

        public void As(int arg)
        {
            Expression<Func<Expressible, int>> initializer = _ => _.X(arg);
            base.As(initializer);
        }
    }
}
