using System;
using System.Linq;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static partial class TypeMixin
    {
        public static MethodInfo GetMethod(this Type type, MethodInfo methodInfo)
        {
            return type.GetMethod(
                            methodInfo.Name,
                            methodInfo.ExportBinding(),
                            null,
                            methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray(),
                            null);
        }

        public static bool Equivalent(this Type source, Type target)
        {
            if (!source.IsGenericTypeDefinition)
            {
                return source == target;
            }
            else
            {
                return target.Name == source.Name &&
                       target.IsGenericType &&
                       target.GetGenericArguments().Length == source.GetGenericArguments().Length &&
                       target == source.MakeGenericType(target.GetGenericArguments());
            }
        }

        public static bool EquivalentWithoutGenericArguments(this Type source, Type target)
        {
            if (!source.IsGenericType)
            {
                return source == target;
            }
            else
            {
                return target.Name == source.Name &&
                       target.IsGenericType &&
                       target.GetGenericArguments().Length == source.GetGenericArguments().Length;
            }
        }
    }
}
