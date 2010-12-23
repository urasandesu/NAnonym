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
        public static MethodInfo GetStaticMethod<TResult>(Expression<StaticFunc<TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T, TResult>(Expression<StaticFunc<T, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2, TResult>(Expression<StaticFunc<T1, T2, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2, T3, TResult>(Expression<StaticFunc<T1, T2, T3, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2, T3, T4, TResult>(Expression<StaticFunc<T1, T2, T3, T4, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod(Expression<StaticAction> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T>(Expression<StaticAction<T>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2>(Expression<StaticAction<T1, T2>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2, T3>(Expression<StaticAction<T1, T2, T3>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetStaticMethod<T1, T2, T3, T4>(Expression<StaticAction<T1, T2, T3, T4>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }


        public static string GetStaticMethodName(Expression<StaticAction> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T>(Expression<StaticAction<T>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2>(Expression<StaticAction<T1, T2>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2, T3>(Expression<StaticAction<T1, T2, T3>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2, T3, T4>(Expression<StaticAction<T1, T2, T3, T4>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<TResult>(Expression<StaticFunc<TResult>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T, TResult>(Expression<StaticFunc<T, TResult>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2, TResult>(Expression<StaticFunc<T1, T2, TResult>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2, T3, TResult>(Expression<StaticFunc<T1, T2, T3, TResult>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }

        public static string GetStaticMethodName<T1, T2, T3, T4, TResult>(Expression<StaticFunc<T1, T2, T3, T4, TResult>> methodNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodNameProvider).Name;
        }






        public static ParameterInfo[] GetStaticMethodParameters(Expression<StaticAction> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T>(Expression<StaticAction<T>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2>(Expression<StaticAction<T1, T2>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2, T3>(Expression<StaticAction<T1, T2, T3>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2, T3, T4>(Expression<StaticAction<T1, T2, T3, T4>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<TResult>(Expression<StaticFunc<TResult>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T, TResult>(Expression<StaticFunc<T, TResult>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2, TResult>(Expression<StaticFunc<T1, T2, TResult>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2, T3, TResult>(Expression<StaticFunc<T1, T2, T3, TResult>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }

        public static ParameterInfo[] GetStaticMethodParameters<T1, T2, T3, T4, TResult>(Expression<StaticFunc<T1, T2, T3, T4, TResult>> methodParameterProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterProvider).GetParameters();
        }






        public static Type[] GetStaticMethodParameterTypes(Expression<StaticAction> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T>(Expression<StaticAction<T>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2>(Expression<StaticAction<T1, T2>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2, T3>(Expression<StaticAction<T1, T2, T3>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2, T3, T4>(Expression<StaticAction<T1, T2, T3, T4>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<TResult>(Expression<StaticFunc<TResult>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T, TResult>(Expression<StaticFunc<T, TResult>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2, TResult>(Expression<StaticFunc<T1, T2, TResult>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2, T3, TResult>(Expression<StaticFunc<T1, T2, T3, TResult>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }

        public static Type[] GetStaticMethodParameterTypes<T1, T2, T3, T4, TResult>(Expression<StaticFunc<T1, T2, T3, T4, TResult>> methodParameterTypeProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterTypeProvider).ParameterTypes();
        }






        public static string[] GetStaticMethodParameterNames(Expression<StaticAction> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T>(Expression<StaticAction<T>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2>(Expression<StaticAction<T1, T2>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2, T3>(Expression<StaticAction<T1, T2, T3>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2, T3, T4>(Expression<StaticAction<T1, T2, T3, T4>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<TResult>(Expression<StaticFunc<TResult>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T, TResult>(Expression<StaticFunc<T, TResult>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2, TResult>(Expression<StaticFunc<T1, T2, TResult>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2, T3, TResult>(Expression<StaticFunc<T1, T2, T3, TResult>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }

        public static string[] GetStaticMethodParameterNames<T1, T2, T3, T4, TResult>(Expression<StaticFunc<T1, T2, T3, T4, TResult>> methodParameterNameProvider)
        {
            return GetMethodInfo((LambdaExpression)methodParameterNameProvider).ParameterNames();
        }






        public static FieldInfo GetFieldInfo<T>(Expression<Func<T>> fieldProvider)
        {
            return GetFieldInfo((LambdaExpression)fieldProvider);
        }

        
        
        

        public static MethodInfo GetInstanceMethod<TBase, TResult>(Expression<InstanceFunc<TBase, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T, TResult>(Expression<InstanceFunc<TBase, T, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2, TResult>(Expression<InstanceFunc<TBase, T1, T2, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2, T3, TResult>(Expression<InstanceFunc<TBase, T1, T2, T3, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2, T3, T4, TResult>(Expression<InstanceFunc<TBase, T1, T2, T3, T4, TResult>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase>(Expression<InstanceAction<TBase>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T>(Expression<InstanceAction<TBase, T>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2>(Expression<InstanceAction<TBase, T1, T2>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2, T3>(Expression<InstanceAction<TBase, T1, T2, T3>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }

        public static MethodInfo GetInstanceMethod<TBase, T1, T2, T3, T4>(Expression<InstanceAction<TBase, T1, T2, T3, T4>> methodProvider)
        {
            return GetMethodInfo((LambdaExpression)methodProvider);
        }






        public static MethodInfo GetMethodInfo(LambdaExpression methodProvider)
        {
            return (MethodInfo)((ConstantExpression)((MethodCallExpression)((UnaryExpression)methodProvider.Body).Operand).Arguments[2]).Value;
        }

        public static FieldInfo GetFieldInfo(LambdaExpression fieldProvider)
        {
            return (FieldInfo)((MemberExpression)fieldProvider.Body).Member;
        }
        
        public static string GetName<T>(Expression<Func<T>> nameProvider)
        {
            return ((MemberExpression)nameProvider.Body).Member.Name;
        }
    }
}

