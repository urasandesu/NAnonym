/* 
 * File: FieldInfoMixin.cs
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
    public static class FieldInfoMixin
    {
        public static Exec GetGetterDelegate(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            var getter = new DynamicMethod("Get_" + fieldInfo.Name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = getter.GetILGenerator();

            if (!fieldInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
            }

            if (!fieldInfo.IsStatic)
                gen.Emit(OpCodes.Ldfld, fieldInfo);
            else
                gen.Emit(OpCodes.Ldsfld, fieldInfo);

            gen.Emit(OpCodes.Box_Opt, fieldInfo.FieldType);
            gen.Emit(OpCodes.Ret);
            return (Exec)getter.CreateDelegate(typeof(Exec));
        }


        public static Exec GetSetterDelegate(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            var setter = new DynamicMethod("Set_" + fieldInfo.Name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = setter.GetILGenerator();

            if (!fieldInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
            }

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Opt, fieldInfo.FieldType);

            if (!fieldInfo.IsStatic)
                gen.Emit(OpCodes.Stfld, fieldInfo);
            else
                gen.Emit(OpCodes.Stsfld, fieldInfo);

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Ret);
            return (Exec)setter.CreateDelegate(typeof(Exec));
        }
    }
}
