using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{

    public abstract class LocalField : DependencyField
    {
        public LocalField(DependencyClass @class, LambdaExpression reference, Type type)
            : base(@class, reference, type)
        {
        }
    }

    public class LocalField<T> : LocalField
    {
        public LocalField(LocalClass @class, Expression<Func<T>> reference)
            : base(@class, reference, typeof(T))
        {
        }

        public void As(Expression<Func<Expressible, T>> exp)
        {
            base.As(exp);
        }
    }

    public class LocalFieldInt : LocalField
    {
        public LocalFieldInt(LocalClass @class, Expression<Func<int>> reference)
            : base(@class, reference, typeof(int))
        {
        }

        public void As(int arg)
        {
            Expression<Func<Expressible, int>> exp = _ => _.X(arg);
            base.As(exp);
        }
    }
}
