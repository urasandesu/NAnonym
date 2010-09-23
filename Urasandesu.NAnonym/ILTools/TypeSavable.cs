using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Urasandesu.NAnonym.ILTools
{
    public static class TypeSavable
    {
        public static MethodInfo GetMethodInfo<TResult>(Expression<Func<Func<TResult>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<Func<T, TResult>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo(Expression<Func<Action>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T>(Expression<Func<Action<T>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2>(Expression<Func<Action<T1, T2>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static string GetMethodName(Expression<Func<Action>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T>(Expression<Func<Action<T>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2>(Expression<Func<Action<T1, T2>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<TResult>(Expression<Func<Func<TResult>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T, TResult>(Expression<Func<Func<T, TResult>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetMethodName<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static ParameterInfo[] GetMethodParameters<T>(Expression<Func<Action<T>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2>(Expression<Func<Action<T1, T2>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T, TResult>(Expression<Func<Func<T, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetMethodParameters<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static Type[] GetMethodParameterTypes<T>(Expression<Func<Action<T>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2>(Expression<Func<Action<T1, T2>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T, TResult>(Expression<Func<Func<T, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.ParameterType).ToArray();
        }

        public static string[] GetMethodParameterNames<T>(Expression<Func<Action<T>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2>(Expression<Func<Action<T1, T2>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T, TResult>(Expression<Func<Func<T, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodParameterProvider)
        {
            return GetMethodParameters(methodParameterProvider).Select(parameter => parameter.Name).ToArray();
        }

        static MethodInfo GetMethodInfo(LambdaExpression methodProvider)
        {
            return (MethodInfo)((ConstantExpression)((MethodCallExpression)((UnaryExpression)methodProvider.Body).Operand).Arguments[2]).Value;
        }

        public static string GetName<T>(Expression<Func<T>> nameProvider)
        {
            return ((MemberExpression)nameProvider.Body).Member.Name;
        }
    }
}
