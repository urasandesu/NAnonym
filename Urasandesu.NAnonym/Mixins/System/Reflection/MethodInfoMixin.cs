/* 
 * File: MethodInfoMixin.cs
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
using System.Linq;
using System.Reflection;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Mixins.System.Reflection
{
    public static class MethodInfoMixin
    {
        public static readonly MethodInfo GetTypeFromHandleInfo = TypeSavable.GetStaticMethod<RuntimeTypeHandle, Type>(() => Type.GetTypeFromHandle);
        public static readonly MethodInfo GetMethodFromHandleInfo = TypeSavable.GetStaticMethod<RuntimeMethodHandle, MethodBase>(() => MethodBase.GetMethodFromHandle);
        public static readonly MethodInfo GetFieldFromHandleInfo = TypeSavable.GetStaticMethod<RuntimeFieldHandle, FieldInfo>(() => FieldInfo.GetFieldFromHandle);

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

