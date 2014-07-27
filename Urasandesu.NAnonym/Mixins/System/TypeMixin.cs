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
using System.Runtime.Serialization;
using Urasandesu.NAnonym.Mixins.System.Reflection;

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

        public static Exec GetMemberDelegate(this Type t, string name, Type[] types)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance |
                              BindingFlags.Static;
            return t.GetGetMemberDelegate(name, bindingAttr, types);
        }

        public static Exec GetGetMemberDelegate(this Type t, string name, BindingFlags bindingAttr, Type[] types)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var memberInfos = t.GetMembers(bindingAttr);
            if (memberInfos == null || memberInfos.Length == 0)
                return null;

            var factories = memberInfos.Select(_ => CreateMemberDelegateFactory(_, name, types)).
                                        Where(_ => _.IsTarget()).
                                        ToArray();
            if (1 < factories.Length)
                throw new AmbiguousMatchException("");  // TODO: 

            if (factories.Length == 0)
                return null;

            return factories[0].CreateDelegate();
        }

        static MemberDelegateFactory CreateMemberDelegateFactory(MemberInfo memberInfo, string name, Type[] types)
        {
            {
                var ctorInfo = memberInfo as ConstructorInfo;
                if (ctorInfo != null)
                    return new ConstructorDelegateFactory(ctorInfo, name, types);
            }

            {
                var fieldInfo = memberInfo as FieldInfo;
                if (fieldInfo != null)
                    return new FieldDelegateFactory(fieldInfo, name, types);
            }

            {
                var methodInfo = memberInfo as MethodInfo;
                if (methodInfo != null)
                    return new MethodDelegateFactory(methodInfo, name, types);
            }

            {
                var propInfo = memberInfo as PropertyInfo;
                if (propInfo != null)
                    return new PropertyDelegateFactory(propInfo, name, types);
            }

            throw new NotImplementedException(string.Format("MemberInfo.GetType(): {0}", memberInfo.GetType()));
        }

        abstract class MemberDelegateFactory
        {
            protected readonly MemberInfo m_memberInfo;
            protected readonly string m_name;
            protected readonly Type[] m_types;

            public MemberDelegateFactory(MemberInfo memberInfo, string name, Type[] types)
            {
                m_memberInfo = memberInfo;
                m_name = name;
                m_types = types;
            }

            public abstract bool IsTarget();
            public abstract Exec CreateDelegate();
        }

        class FieldDelegateFactory : MemberDelegateFactory
        {
            public FieldDelegateFactory(FieldInfo fieldInfo, string name, Type[] types)
                : base(fieldInfo, name, types)
            { }

            FieldInfo Member { get { return (FieldInfo)m_memberInfo; } }

            public override bool IsTarget()
            {
                return Member.Name == m_name;
            }

            public override Exec CreateDelegate()
            {
                var getter = default(Exec);
                var setter = default(Exec);
                return new Exec((target, args) =>
                {
                    if (args == null || args.Length == 0)
                    {
                        if (getter == null)
                            getter = Member.GetGetterDelegate();
                        return getter(target, args);
                    }
                    else
                    {
                        if (setter == null)
                            setter = Member.GetSetterDelegate();
                        return setter(target, args);
                    }
                });
            }
        }

        abstract class MethodBaseDelegateFactory : MemberDelegateFactory
        {
            public MethodBaseDelegateFactory(MethodBase methodBase, string name, Type[] types)
                : base(methodBase, name, types)
            { }

            protected MethodBase Member { get { return (MethodBase)m_memberInfo; } }

            public override bool IsTarget()
            {
                var result = true;
                result &= Member.Name == m_name;
                result &= Member.GetParameters().Select(_ => _.ParameterType).SequenceEqual(m_types);
                return result;
            }
        }

        class ConstructorDelegateFactory : MethodBaseDelegateFactory
        {
            public ConstructorDelegateFactory(ConstructorInfo ctorInfo, string name, Type[] types)
                : base(ctorInfo, name, types)
            { }

            new ConstructorInfo Member { get { return (ConstructorInfo)base.Member; } }

            public override Exec CreateDelegate()
            {
                var create = default(Exec);
                var place = default(Exec);
                return new Exec((target, args) =>
                {
                    if (target == null)
                    {
                        if (create == null)
                            create = Member.GetCreationDelegate();
                        return create(target, args);
                    }
                    else
                    {
                        if (place == null)
                            place = Member.GetPlacementDelegate();
                        return place(target, args);
                    }
                });
            }
        }

        class MethodDelegateFactory : MethodBaseDelegateFactory
        {
            public MethodDelegateFactory(MethodInfo methodInfo, string name, Type[] types)
                : base(methodInfo, name, types)
            { }

            new MethodInfo Member { get { return (MethodInfo)base.Member; } }

            public override Exec CreateDelegate()
            {
                return Member.GetDelegate();
            }
        }

        class PropertyDelegateFactory : MemberDelegateFactory
        {
            public PropertyDelegateFactory(PropertyInfo propInfo, string name, Type[] types)
                : base(propInfo, name, types)
            { }

            PropertyInfo Member { get { return (PropertyInfo)m_memberInfo; } }

            public override bool IsTarget()
            {
                return Member.Name == m_name;
            }

            public override Exec CreateDelegate()
            {
                var getter = default(Exec);
                var setter = default(Exec);
                return new Exec((target, args) =>
                {
                    if (args == null || args.Length == 0)
                    {
                        if (getter == null)
                            getter = Member.GetGetterDelegate();
                        return getter(target, args);
                    }
                    else
                    {
                        if (setter == null)
                            setter = Member.GetSetterDelegate();
                        return setter(target, args);
                    }
                });
            }
        }

        // TODO: GetConstructorDelegate の Generic 版。

        public static Exec GetConstructorDelegate(this Type t, Type[] types)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance;
            var binder = default(Binder);
            var modifiers = default(ParameterModifier[]);
            return t.GetConstructorDelegate(bindingAttr, binder, types, modifiers);
        }

        public static Exec GetConstructorDelegate(this Type t, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            var ctorInfo = t.GetConstructor(bindingAttr, binder, types, modifiers);
            if (ctorInfo == null)
                return null;

            return ctorInfo.GetCreationDelegate();
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

            return fieldInfo.GetGetterDelegate();
        }

        public static Exec GetFieldSetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance |
                              BindingFlags.Static;
            return t.GetFieldSetterDelegate(name, bindingAttr);
        }

        public static Exec GetFieldSetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                return null;

            var fieldInfo = t.GetField(name, bindingAttr);
            if (fieldInfo == null)
                return null;

            return fieldInfo.GetSetterDelegate();
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

            return methodInfo.GetDelegate();
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

            return propertyInfo.GetGetterDelegate();
        }

        public static Exec GetPropertySetterDelegate(this Type t, string name)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance |
                              BindingFlags.Static;
            return t.GetPropertySetterDelegate(name, bindingAttr);
        }

        public static Exec GetPropertySetterDelegate(this Type t, string name, BindingFlags bindingAttr)
        {
            if (t == null)
                return null;

            var propertyInfo = t.GetProperties(bindingAttr).FirstOrDefault(_ => _.Name == name);
            if (propertyInfo == null)
                return null;

            return propertyInfo.GetSetterDelegate();
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
