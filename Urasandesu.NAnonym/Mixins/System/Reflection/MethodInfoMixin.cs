/* 
 * File: MethodInfoMixin.cs
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
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;
using OpCodes = Urasandesu.NAnonym.Reflection.Emit.OpCodes;

namespace Urasandesu.NAnonym.Mixins.System.Reflection
{
    public static class MethodInfoMixin
    {
        public static Exec GetDelegate(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            var method = new DynamicMethod("Invoke_" + methodInfo.Name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = method.GetILGenerator();
            var paramInfos = methodInfo.GetParameters();
            var locals = new LocalBuilder[] { };
            if (0 < paramInfos.Length)
            {
                locals = new LocalBuilder[paramInfos.Length];
                for (int i = 0; i < paramInfos.Length; i++)
                {
                    var localType = paramInfos[i].ParameterType.ElementIfByRef();
                    locals[i] = gen.DeclareLocal(localType);
                    gen.Emit(OpCodes.Ldarg_1);
                    gen.Emit(OpCodes.Ldc_I4_Opt, i);
                    gen.Emit(OpCodes.Ldelem_Ref);
                    gen.Emit(OpCodes.Unbox_Opt, localType);
                    gen.Emit(OpCodes.Stloc_Opt, i);
                }
            }

            if (!methodInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, methodInfo.DeclaringType);
            }

            if (0 < paramInfos.Length)
            {
                for (int i = 0; i < paramInfos.Length; i++)
                {
                    if (paramInfos[i].ParameterType.IsByRef)
                        gen.Emit(OpCodes.Ldloca_Opt, i);
                    else
                        gen.Emit(OpCodes.Ldloc_Opt, i);
                }
            }

            if (!methodInfo.IsStatic)
                gen.Emit(OpCodes.Callvirt, methodInfo);
            else
                gen.Emit(OpCodes.Call, methodInfo);

            var result = default(LocalBuilder);
            if (methodInfo.ReturnType != typeof(void))
            {
                result = gen.DeclareLocal(methodInfo.ReturnType);
                gen.Emit(OpCodes.Stloc, result);
            }

            if (0 < paramInfos.Length)
            {
                for (int i = 0; i < paramInfos.Length; i++)
                {
                    if (paramInfos[i].ParameterType.IsByRef)
                    {
                        gen.Emit(OpCodes.Ldarg_1);
                        gen.Emit(OpCodes.Ldc_I4_Opt, i);
                        gen.Emit(OpCodes.Ldloc_Opt, i);
                        gen.Emit(OpCodes.Box_Opt, locals[i].LocalType);
                        gen.Emit(OpCodes.Stelem_Ref);
                    }
                }
            }

            if (result != null)
            {
                gen.Emit(OpCodes.Ldloc, result);
                gen.Emit(OpCodes.Box_Opt, result.LocalType);
            }
            else
            {
                gen.Emit(OpCodes.Ldnull);
            }
            gen.Emit(OpCodes.Ret);

            return (Exec)method.CreateDelegate(typeof(Exec));
        }

        public static MethodInfo MakeGenericMethodIfAvailable(this MethodInfo mi, params Type[] typeArguments)
        {
            if (mi == null)
                throw new ArgumentNullException("mi");

            return mi.IsGenericMethod ? mi.MakeGenericMethod(typeArguments) : mi;
        }

        public static bool IsDynamicMethod(this MethodInfo mi)
        {
            if (mi == null)
                throw new ArgumentNullException("mi");

            return mi.GetType().Name == DynamicMethodMixin.RTDynamicMethodProxy.Type.Name;
        }

        public static IntPtr GetFunctionPointer(this MethodInfo mi)
        {
            if (mi.IsDynamicMethod())
            {
                var dynMethod = new DynamicMethodMixin.RTDynamicMethodProxy(mi).Get_m_owner();
                var handle = dynMethod.GetMethodDescriptor();
                return handle.GetFunctionPointer();
            }

            return mi.MethodHandle.GetFunctionPointer();
        }
    }
}
