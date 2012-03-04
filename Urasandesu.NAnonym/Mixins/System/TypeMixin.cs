using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class TypeMixin
    {
        public static T ForciblyNew<T>(params object[] args)
        {
            var bindingAttr = BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance;
            var binder = default(Binder);
            var types = args == null || args.Length == 0 ?
                            Type.EmptyTypes :
                            args.Select(_ => _.GetType()).ToArray();
            var modifiers = default(ParameterModifier[]);
            var ctor = typeof(T).GetConstructor(bindingAttr, binder, types, modifiers);
            return (T)ctor.Invoke(args);
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
