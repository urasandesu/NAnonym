using System;
using System.Linq.Expressions;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{

    public abstract class GlobalField : DependencyField
    {
        public GlobalField(DependencyClass @class, LambdaExpression fieldReference, Type fieldType)
            : base(@class, fieldReference, fieldType)
        {
        }
    }

    public class GlobalField<T> : GlobalField
    {
        public GlobalField(GlobalClass @class, Expression<Func<T>> fieldReference)
            : base(@class, fieldReference, typeof(T))
        {
        }

        public void As(Expression<Func<Expressible, T>> initializer)
        {
            base.As(initializer);
        }
    }

    public class GlobalFieldInt : GlobalField
    {
        public GlobalFieldInt(GlobalClass @class, Expression<Func<int>> fieldReference)
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
