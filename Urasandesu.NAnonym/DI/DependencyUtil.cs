using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    public class DependencyUtil
    {
        protected DependencyUtil()
        {
        }

        public static MethodInfo ExtractMethod<TBase, T, TResult>(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            return (MethodInfo)((ConstantExpression)((MethodCallExpression)(((UnaryExpression)expression.Body).Operand)).Arguments[2]).Value;
        }
    }
}
