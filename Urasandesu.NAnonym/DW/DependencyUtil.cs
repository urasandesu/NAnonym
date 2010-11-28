using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.DW
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

        public static void RollbackLocal()
        {
            classSet.Clear();
        }

        
        
        
        
        
        public static MethodInfo ExtractMethod<TBase, TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        
        
        
        
        
        public static MethodInfo ExtractMethod<TBase>(Expression<ActionReference<TBase>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            return TypeSavable.GetMethodInfo((LambdaExpression)methodReference);
        }
    }
}
