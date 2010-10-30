using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Mixins.System.Reflection
{
    public static partial class MethodInfoMixin
    {
        public static BindingFlags ExportBinding(this MethodInfo methodInfo)
        {
            BindingFlags bindingAttr = BindingFlags.Default;

            if (methodInfo.IsPublic)
            {
                bindingAttr |= BindingFlags.Public;
            }
            else
            {
                bindingAttr |= BindingFlags.NonPublic;
            }

            if (methodInfo.IsStatic)
            {
                bindingAttr |= BindingFlags.Static;
            }
            else
            {
                bindingAttr |= BindingFlags.Instance;
            }

            return bindingAttr;
        }

        public static Type[] ParameterTypes(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public static string[] ParameterNames(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Select(parameter => parameter.Name).ToArray();
        }

        public static bool Equivalent(this MethodInfo source, MethodInfo target)
        {
            if (!source.IsGenericMethodDefinition)
            {
                return source == target;
            }
            else
            {
                return target.Name == source.Name &&
                       target.IsGenericMethod &&
                       target.GetGenericArguments().Length == source.GetGenericArguments().Length &&
                       target == source.MakeGenericMethod(target.GetGenericArguments());
            }
        }

        public static bool EquivalentWithoutGenericArguments(this MethodInfo source, MethodInfo target)
        {
            if (!source.IsGenericMethod && !source.ContainsGenericParameters && !source.DeclaringType.IsGenericType)
            {
                return source == target;
            }
            else
            {
                if (target.Name != source.Name) return false;
                if (target.IsGenericMethod && target.GetGenericArguments().Length == source.GetGenericArguments().Length) return true;

                return !target.ReturnType.IsGenericParameter && 
                       !source.ReturnType.IsGenericParameter && 
                       target.ParameterTypes().Where(parameterType => !parameterType.IsGenericParameter).Equivalent(
                            source.ParameterTypes().Where(parameterType => !parameterType.IsGenericParameter));
            }
        }
    }
}
