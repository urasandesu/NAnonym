using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

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

        public static Delegate GetConstructorDelegate(this Type t, Type[] types)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance;
            var binder = default(Binder);
            var modifiers = default(ParameterModifier[]);
            return t.GetConstructorDelegate(bindingAttr, binder, types, modifiers);
        }

        public static Delegate GetConstructorDelegate(this Type t, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            if (t == null)
                return null;

            var ctorInfo = t.GetConstructor(bindingAttr, binder, types, modifiers);
            if (ctorInfo == null)
                return null;

            var @params = ctorInfo.GetParameters().Select(_ => Expression.Parameter(_.ParameterType, _.Name)).ToArray();
            return Expression.Lambda(Expression.New(ctorInfo, @params), @params).Compile();
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
