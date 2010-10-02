using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Urasandesu.NAnonym.DI
{
    public class DependencyUtil
    {
        protected DependencyUtil()
        {
        }

        static HashSet<LocalClass> classSet = new HashSet<LocalClass>();

        public static void RegisterLocal(LocalClass localClass)
        {
            classSet.Add(localClass);
            localClass.Register();
        }

        public static void LoadLocal()
        {
            foreach (var @class in classSet)
            {
                @class.Load();
            }
        }

        public static MethodInfo ExtractMethod<TBase, T, TResult>(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            return (MethodInfo)((ConstantExpression)((MethodCallExpression)(((UnaryExpression)expression.Body).Operand)).Arguments[2]).Value;
        }
    }
}
