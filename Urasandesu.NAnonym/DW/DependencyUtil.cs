/* 
 * File: DependencyUtil.cs
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

