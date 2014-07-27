/* 
 * File: PropertyInfoMixin.cs
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
    public static class PropertyInfoMixin
    {
        public static Exec GetGetterDelegate(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            var getter = new DynamicMethod("Get_" + propertyInfo.Name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = getter.GetILGenerator();

            var propertyGetterInfo = propertyInfo.GetGetMethod(true);
            if (!propertyGetterInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            }

            if (!propertyGetterInfo.IsStatic)
                gen.Emit(OpCodes.Callvirt, propertyGetterInfo);
            else
                gen.Emit(OpCodes.Call, propertyGetterInfo);

            gen.Emit(OpCodes.Box_Opt, propertyInfo.PropertyType);
            gen.Emit(OpCodes.Ret);
            return (Exec)getter.CreateDelegate(typeof(Exec));
        }


        public static Exec GetSetterDelegate(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            var setter = new DynamicMethod("Set_" + propertyInfo.Name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = setter.GetILGenerator();

            var propertySetterInfo = propertyInfo.GetSetMethod(true);
            if (!propertySetterInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            }

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Opt, propertyInfo.PropertyType);

            if (!propertySetterInfo.IsStatic)
                gen.Emit(OpCodes.Callvirt, propertySetterInfo);
            else
                gen.Emit(OpCodes.Call, propertySetterInfo);

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Ret);
            return (Exec)setter.CreateDelegate(typeof(Exec));
        }
    }
}
