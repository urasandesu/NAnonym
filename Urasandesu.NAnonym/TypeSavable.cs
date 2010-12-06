/* 
 * File: TypeSavable.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym
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






        public static ParameterInfo[] GetMethodParameters(Expression<Func<Action>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
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






        public static ParameterInfo[] GetMethodParameters<TResult>(Expression<Func<Func<TResult>>> methodParameterProvider)
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






        public static Type[] GetMethodParameterTypes(Expression<Func<Action>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T>(Expression<Func<Action<T>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2>(Expression<Func<Action<T1, T2>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }






        public static Type[] GetMethodParameterTypes<TResult>(Expression<Func<Func<TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T, TResult>(Expression<Func<Func<T, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }

        public static Type[] GetMethodParameterTypes<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterTypes();
        }






        public static string[] GetMethodParameterNames(Expression<Func<Action>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T>(Expression<Func<Action<T>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2>(Expression<Func<Action<T1, T2>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }






        public static string[] GetMethodParameterNames<TResult>(Expression<Func<Func<TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T, TResult>(Expression<Func<Func<T, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2, TResult>(Expression<Func<Func<T1, T2, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, TResult>(Expression<Func<Func<T1, T2, T3, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }

        public static string[] GetMethodParameterNames<T1, T2, T3, T4, TResult>(Expression<Func<Func<T1, T2, T3, T4, TResult>>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).ParameterNames();
        }


        
        
        
        
        public static MethodInfo GetMethodInfo(LambdaExpression methodProvider)
        {
            return (MethodInfo)((ConstantExpression)((MethodCallExpression)((UnaryExpression)methodProvider.Body).Operand).Arguments[2]).Value;
        }

        public static FieldInfo GetFieldInfo<T>(Expression<Func<T>> fieldProvider)
        {
            return GetFieldInfo((LambdaExpression)fieldProvider);
        }

        public static FieldInfo GetFieldInfo(LambdaExpression fieldProvider)
        {
            return (FieldInfo)((MemberExpression)fieldProvider.Body).Member;
        }

        
        
        
        public static MethodInfo ExtractMethod<TBase, TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }






        public static MethodInfo ExtractMethod<TBase>(Expression<ActionReference<TBase>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        public static MethodInfo ExtractMethod<TBase, T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            return GetMethodInfo((LambdaExpression)methodReference);
        }

        
        
        
        
        
        public static string GetName<T>(Expression<Func<T>> nameProvider)
        {
            return ((MemberExpression)nameProvider.Body).Member.Name;
        }
    }
}

