#region Obsolete
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Reflection;
//using System.Linq.Expressions;

//namespace Urasandesu.NAnonym.CREUtilities
//{
//    public static class ExpressiveType
//    {
//        public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<Func<T, TResult>>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static MethodInfo GetMethodInfo<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static MethodInfo GetMethodInfo<TResult>(Expression<Func<Func<TResult>>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static MethodInfo GetMethodInfo(Expression<Func<Action>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static MethodInfo GetMethodInfo<T>(Expression<Func<Action<T>>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static MethodInfo GetMethodInfo<T1, T2>(Expression<Func<Action<T1, T2>>> methodProvider)
//        {
//            return GetMethodInfo((LambdaExpression)methodProvider);
//        }

//        public static string GetMethodName<T1, T2>(Expression<Func<Action<T1, T2>>> methodNameProvider)
//        {
//            throw new NotImplementedException();
//        }

//        private static MethodInfo GetMethodInfo(LambdaExpression methodProvider)
//        {
//            return (MethodInfo)((ConstantExpression)((MethodCallExpression)((UnaryExpression)methodProvider.Body).Operand).Arguments[2]).Value;
//        }

//        public static string GetParamName<T>(Expression<Func<T>> paramNameProvider)
//        {
//            return ((MemberExpression)paramNameProvider.Body).Member.Name;
//        }
//    }
//}
#endregion