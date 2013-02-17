/* 
 * File: TypeMixin.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;
using OpCodes = Urasandesu.NAnonym.Reflection.Emit.OpCodes;

namespace Urasandesu.NAnonym.Mixins.System
{   
    public static class TypeMixin
    {
        public static T ForciblyNew<T>(params object[] args)
        {
            return (T)typeof(T).ForciblyNew(args);
        }

        public static object ForciblyNew(this Type t, params object[] args)
        {
            if (t == null)
                return null;
            
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance;
            var binder = default(Binder);
            var types = args == null || args.Length == 0 ?
                            Type.EmptyTypes :
                            args.Select(_ => _.GetType()).ToArray();
            var modifiers = default(ParameterModifier[]);
            var ctor = t.GetConstructor(bindingAttr, binder, types, modifiers);
            return ctor.Invoke(args);
        }

        // TODO: GetConstructorDelegate の Generic 版。

        public static Work GetConstructorDelegate(this Type t, Type[] types)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance;
            var binder = default(Binder);
            var modifiers = default(ParameterModifier[]);
            return t.GetConstructorDelegate(bindingAttr, binder, types, modifiers);
        }

        public static Work GetConstructorDelegate(this Type t, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var ctorInfo = t.GetConstructor(bindingAttr, binder, types, modifiers);
            if (ctorInfo == null)
                return null;

            var ctor = new DynamicMethod("Ctor", typeof(object), new Type[] { typeof(object[]) }, true);
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
                    gen.Emit(OpCodes.Ldarg_0);
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
                        gen.Emit(OpCodes.Ldarg_0);
                        gen.Emit(OpCodes.Ldc_I4_Opt, i);
                        gen.Emit(OpCodes.Ldloc_Opt, i);
                        gen.Emit(OpCodes.Box_Opt, locals[i].LocalType);
                        gen.Emit(OpCodes.Stelem_Ref);
                    }
                }
            }

            gen.Emit(OpCodes.Ldloc, result);
            gen.Emit(OpCodes.Ret);

            return (Work)ctor.CreateDelegate(typeof(Work));
        }

        // TODO: GetFieldGetterDelegate の Generic 版。

        public static Exec GetFieldGetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance | 
                              BindingFlags.Static;
            return t.GetFieldGetterDelegate(name, bindingAttr);
        }

        public static Exec GetFieldGetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var fieldInfo = t.GetField(name, bindingAttr);
            if (fieldInfo == null)
                return null;

            var getter = new DynamicMethod("Get_" + name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = getter.GetILGenerator();

            if (!fieldInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, t);
            }

            if (!fieldInfo.IsStatic)
                gen.Emit(OpCodes.Ldfld, fieldInfo);
            else
                gen.Emit(OpCodes.Ldsfld, fieldInfo);

            gen.Emit(OpCodes.Box_Opt, fieldInfo.FieldType);
            gen.Emit(OpCodes.Ret);
            return (Exec)getter.CreateDelegate(typeof(Exec));
        }

        public static Effect GetFieldSetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance | 
                              BindingFlags.Static;
            return t.GetFieldSetterDelegate(name, bindingAttr);
        }

        public static Effect GetFieldSetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                return null;

            var fieldInfo = t.GetField(name, bindingAttr);
            if (fieldInfo == null)
                return null;

            var setter = new DynamicMethod("Set_" + name, typeof(void), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = setter.GetILGenerator();

            if (!fieldInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, t);
            }
            
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Opt, fieldInfo.FieldType);

            if (!fieldInfo.IsStatic)
                gen.Emit(OpCodes.Stfld, fieldInfo);
            else
                gen.Emit(OpCodes.Stsfld, fieldInfo);
            
            gen.Emit(OpCodes.Ret);
            return (Effect)setter.CreateDelegate(typeof(Effect));
        }

        public static Exec GetMethodDelegate(this Type t, string name, Type[] types)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance | 
                              BindingFlags.Static;
            var binder = default(Binder);
            var modifiers = default(ParameterModifier[]);
            return t.GetMethodDelegate(name, bindingAttr, binder, types, modifiers);
        }

        public static Exec GetMethodDelegate(this Type t, string name, 
            BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var methodInfo = t.GetMethod(name, bindingAttr, binder, types, modifiers);
            if (methodInfo == null)
                return null;

            var method = new DynamicMethod("Invoke_" + name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
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
                gen.Emit(OpCodes.Castclass, t);
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

            if (methodInfo.ReturnType != typeof(void))
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

        public static Exec GetPropertyGetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance | 
                              BindingFlags.Static;
            return t.GetPropertyGetterDelegate(name, bindingAttr);
        }

        public static Exec GetPropertyGetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var propertyInfo = t.GetProperties(bindingAttr).FirstOrDefault(_ => _.Name == name);
            if (propertyInfo == null)
                return null;

            var getter = new DynamicMethod("Get_" + name, typeof(object), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = getter.GetILGenerator();

            var propertyGetterInfo = propertyInfo.GetGetMethod(true);
            if (!propertyGetterInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, t);
            }

            if (!propertyGetterInfo.IsStatic)
                gen.Emit(OpCodes.Callvirt, propertyGetterInfo);
            else
                gen.Emit(OpCodes.Call, propertyGetterInfo);

            gen.Emit(OpCodes.Box_Opt, propertyInfo.PropertyType);
            gen.Emit(OpCodes.Ret);
            return (Exec)getter.CreateDelegate(typeof(Exec));
        }

        public static Effect GetPropertySetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance | 
                              BindingFlags.Static;
            return t.GetPropertySetterDelegate(name, bindingAttr);
        }

        public static Effect GetPropertySetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                return null;

            var propertyInfo = t.GetProperties(bindingAttr).FirstOrDefault(_ => _.Name == name);
            if (propertyInfo == null)
                return null;

            var setter = new DynamicMethod("Set_" + name, typeof(void), new Type[] { typeof(object), typeof(object[]) }, true);
            var gen = setter.GetILGenerator();

            var propertySetterInfo = propertyInfo.GetSetMethod(true);
            if (!propertySetterInfo.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Castclass, t);
            }

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Opt, propertyInfo.PropertyType);

            if (!propertySetterInfo.IsStatic)
                gen.Emit(OpCodes.Callvirt, propertySetterInfo);
            else
                gen.Emit(OpCodes.Call, propertySetterInfo);
            
            gen.Emit(OpCodes.Ret);
            return (Effect)setter.CreateDelegate(typeof(Effect));
        }

        public static object Default(this Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

        public static Type ElementIfExist(this Type t)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            return t.GetElementType() ?? t;
        }

        public static Type ElementIfByRef(this Type t)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            return t.IsByRef ? t.GetElementType() : t;
        }

        public static bool IsSerializable(this Type t)
        {
            if (t == null)
                return true;

            if (t.HasCustomAttributes<SerializableAttribute>())
                return true;

            if (t.EnumerateBaseType().Where(HasCustomAttributes<SerializableAttribute>).
                                      Where(HasInterface<ISerializable>).
                                      Any())
                return true;

            return false;
        }

        public static bool HasCustomAttributes<TAttribute>(this Type t) where TAttribute : Attribute
        {
            if (t == null)
                return false;

            return t.GetCustomAttributes(typeof(TAttribute), false).Any();
        }

        public static bool HasInterface<TInterface>(this Type t) where TInterface : class
        {
            if (!HasInterfaceImpl<TInterface>.IsInterface)
                return false;

            return t.GetInterface(typeof(TInterface).FullName) != null;
        }

        class HasInterfaceImpl<TInterface>
        {
            static bool ms_isInterface = typeof(TInterface).IsInterface;
            public static bool IsInterface { get { return ms_isInterface; } }
        }

        public static IEnumerable<Type> EnumerateBaseType(this Type t)
        {
            if (t == null)
                yield break;

            yield return t.BaseType;
            foreach (var baseType in EnumerateBaseType(t.BaseType))
            {
                yield return baseType;
            }
        }
    }
}
