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

        
        
        
        
        
        public static MethodInfo ExtractMethod<TBase, TResult>(Expression<FuncReference<TBase, TResult>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T, TResult>(Expression<FuncReference<TBase, T, TResult>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        
        
        
        
        
        public static MethodInfo ExtractMethod<TBase>(Expression<ActionReference<TBase>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T>(Expression<ActionReference<TBase, T>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2>(Expression<ActionReference<TBase, T1, T2>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> reference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)reference);
        }
    }
}
