/* 
 * File: DynamicMethodMixin.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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
using System.Reflection;
using SRE = System.Reflection.Emit;

namespace Urasandesu.NAnonym.Mixins.System.Reflection.Emit
{
    public static class DynamicMethodMixin
    {
        public static readonly Type Type = typeof(SRE::DynamicMethod);

        const string MethodName_GetMethodDescriptor = "GetMethodDescriptor";
        class MethodDelegate_GetMethodDescriptor
        {
            public static readonly Exec Invoke = Type.GetMethodDelegate(MethodName_GetMethodDescriptor, Type.EmptyTypes);
        }
        public static RuntimeMethodHandle GetMethodDescriptor(this SRE::DynamicMethod source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (RuntimeMethodHandle)MethodDelegate_GetMethodDescriptor.Invoke(source, null);
        }

        public class RTDynamicMethodProxy
        {
            public static readonly Type Type = typeof(SRE::DynamicMethod).GetNestedType("RTDynamicMethod", BindingFlags.NonPublic);

            internal MethodInfo Target { get; private set; }

            public RTDynamicMethodProxy(MethodInfo target)
            {
                Target = target;
            }

            const string FieldName_m_owner = "m_owner";
            class FieldGetterDelegate_m_owner
            {
                public static readonly Exec Get = Type.GetFieldGetterDelegate(FieldName_m_owner);
            }
            public SRE::DynamicMethod Get_m_owner()
            {
                return (SRE::DynamicMethod)FieldGetterDelegate_m_owner.Get(Target, null);
            }
        }
    }
}
