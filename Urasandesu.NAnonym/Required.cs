using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Urasandesu.NAnonym
{
    [DebuggerStepThrough]
    public static class Required
    {
        public static T NotDefault<T>(T value, Expression<Func<T>> paramNameProvider)
        {
            return NotDefault(value, paramNameProvider, "Value cannot be default.");
        }

        public static T NotDefault<T>(T value, Expression<Func<T>> paramNameProvider, string message)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentException(message, GetParamName(paramNameProvider));
            }
            return value;
        }

        public static T Default<T>(T value, Expression<Func<T>> paramNameProvider)
        {
            if (!EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentException("Value must be default.", GetParamName(paramNameProvider));
            }
            return value;
        }

        public static T JustOnce<T>(T value, ref T destination, Expression<Func<T>> paramNameProvider)
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                throw new ArgumentException(
                    "Value must be meaningful because destination can set just once.", GetParamName(paramNameProvider));
            }
            else if (!EqualityComparer<T>.Default.Equals(destination, default(T)))
            {
                throw new ArgumentException("Destination has already set.", GetParamName(paramNameProvider));
            }
            destination = value;
            return destination;
        }

        public static int Identical<T>(
            int value, IEnumerable<T> source, Func<IEnumerable<T>, int> identifier, Expression<Func<int>> paramNameProvider)
        {
            if (value != identifier(source))
            {
                throw new ArgumentException("Value must be identical.", GetParamName(paramNameProvider));
            }
            return value;
        }

        public static T Assert<T>(T value, Predicate<T> predicate, Expression<Func<T>> paramNameProvider)
        {
            if (!predicate(value))
            {
                throw new ArgumentException("Value is different from predicate.", GetParamName(paramNameProvider));
            }
            return value;
        }

        public static T MustBeSet<T>(T value, T requiredValue, Expression<Func<T>> paramNameProvider)
        {
            if (!EqualityComparer<T>.Default.Equals(value, requiredValue))
            {
                throw new ArgumentException(string.Format("Value must be set {0}", requiredValue), GetParamName(paramNameProvider));
            }
            return value;
        }

        static string GetParamName(LambdaExpression paramNameProvider)
        {
            return ((MemberExpression)paramNameProvider.Body).Member.Name;
        }
    }
}
