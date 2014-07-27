/* 
 * File: ConstructorInfoMixin.cs
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
    public static class ConstructorInfoMixin
    {
        public static Exec GetCreationDelegate(this ConstructorInfo ctorInfo)
        {
            if (ctorInfo == null)
                throw new ArgumentNullException("ctorInfo");

            var ctor = new DynamicMethod("Ctor", typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = ctor.GetILGenerator();
            var paramInfos = ctorInfo.GetParameters();
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

            var result = gen.DeclareLocal(ctorInfo.DeclaringType);
            gen.Emit(OpCodes.Newobj, ctorInfo);
            gen.Emit(OpCodes.Stloc, result);

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

            gen.Emit(OpCodes.Ldloc, result);
            gen.Emit(OpCodes.Ret);

            return (Exec)ctor.CreateDelegate(typeof(Exec));
        }


        public static Exec GetPlacementDelegate(this ConstructorInfo ctorInfo)
        {
            if (ctorInfo == null)
                throw new ArgumentNullException("ctorInfo");

            var ctor = new DynamicMethod("Ctor", typeof(object), new Type[] { typeof(object), typeof(object[]) }, ctorInfo.Module, true);
            var gen = ctor.GetILGenerator();
            var paramInfos = ctorInfo.GetParameters();
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

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Castclass, ctorInfo.DeclaringType);

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

            gen.Emit(OpCodes.Call, ctorInfo);

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

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ret);

            return (Exec)ctor.CreateDelegate(typeof(Exec));
        }
    }
}
