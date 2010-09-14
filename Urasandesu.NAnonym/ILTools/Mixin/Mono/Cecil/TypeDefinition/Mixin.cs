using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public static partial class Mixin
    {
        public static MethodDefinition GetMethod(this TypeDefinition type, string name, BindingFlags bindingAttr, Type[] types)
        {
            var binder = new Binder(type, name, bindingAttr, types);
            return type.Methods.First(method => binder.BindToMethod(method));
        }

        public static FieldDefinition GetField(this TypeDefinition type, string name, BindingFlags bindingAttr)
        {
            var binder = new Binder(type, name, bindingAttr);
            return type.Fields.First(field => binder.BindToField(field));
        }

        public static FieldDefinition[] GetFields(this TypeDefinition type, BindingFlags bindingAttr)
        {
            var binder = new Binder(type, bindingAttr);
            return type.Fields.Where(field => binder.BindToField(field)).ToArray();
        }

        public static PropertyDefinition GetProperty(this TypeDefinition type, string name, BindingFlags bindingAttr)
        {
            var binder = new Binder(type, name, bindingAttr);
            return type.Properties.First(property => binder.BindToProperty(property));
        }

        public static MethodDefinition GetConstructor(this TypeDefinition type, BindingFlags bindingAttr, Type[] types)
        {
            var binder = new Binder(type, bindingAttr, types);
            return type.Methods.First(method => binder.BindToConstructor(method));
        }

        // 一通り作成したら Base クラス、Default 化。
        private class Binder
        {
            // あとで readonly 化
            TypeDefinition type;
            string name;
            BindingFlags bindingAttr;
            Type[] types;

            public Binder(TypeDefinition type, string name, BindingFlags bindingAttr)
                : this(type, name, bindingAttr, new Type[] { })
            {
            }

            public Binder(TypeDefinition type, BindingFlags bindingAttr)
                : this(type, bindingAttr, new Type[] { })
            {
            }

            public Binder(TypeDefinition type, BindingFlags bindingAttr, Type[] types)
                : this(type, string.Empty, bindingAttr, types)
            {
            }

            public Binder(TypeDefinition type, string name, BindingFlags bindingAttr, Type[] types)
            {
                this.type = type;
                this.name = (bindingAttr & BindingFlags.IgnoreCase) == BindingFlags.IgnoreCase ? name.ToLower() : name;
                this.bindingAttr =
                    bindingAttr == BindingFlags.Default ?
                        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                            BindingFlags.InvokeMethod | BindingFlags.ExactBinding :
                        bindingAttr;
                this.types = types;
            }

            public bool BindToMethod(MethodDefinition method)
            {
                bool success =
                    this.name ==
                    ((bindingAttr & BindingFlags.IgnoreCase) == BindingFlags.IgnoreCase ?
                        method.Name.ToLower() :
                        method.Name);
                success &= method.Parameters.Equivalent(types);

                if (success && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
                {
                    success |= method.DeclaringType.Equivalent(type);
                }

                if (success && (bindingAttr & BindingFlags.Instance) == BindingFlags.Instance)
                {
                    success |= method.HasThis;
                }

                if (success && (bindingAttr & BindingFlags.Static) == BindingFlags.Static)
                {
                    success |= method.IsStatic;
                }

                if (success && (bindingAttr & BindingFlags.Public) == BindingFlags.Public)
                {
                    success |= method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic)
                {
                    success |= !method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
                {
                    success |= !method.DeclaringType.Equals(type);
                }

                if (success && (bindingAttr & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod)
                {
                    success |= true;
                }

                if (success && (bindingAttr & BindingFlags.CreateInstance) == BindingFlags.CreateInstance)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetField) == BindingFlags.GetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetField) == BindingFlags.SetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetProperty) == BindingFlags.GetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetProperty) == BindingFlags.SetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType)
                {
                    // 実装されていません。
                }

                if (success && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                return success;
            }

            public bool BindToConstructor(MethodDefinition method)
            {
                bool success = method.Name == ".cctor" || method.Name == ".ctor";
                success &= method.Parameters.Equivalent(types);

                if (success && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
                {
                    success |= method.DeclaringType.Equivalent(type);
                }

                if (success && (bindingAttr & BindingFlags.Instance) == BindingFlags.Instance)
                {
                    success |= method.HasThis;
                }

                if (success && (bindingAttr & BindingFlags.Static) == BindingFlags.Static)
                {
                    success |= method.IsStatic;
                }

                if (success && (bindingAttr & BindingFlags.Public) == BindingFlags.Public)
                {
                    success |= method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic)
                {
                    success |= !method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
                {
                    success |= !method.DeclaringType.Equals(type);
                }

                if (success && (bindingAttr & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod)
                {
                    success |= true;
                }

                if (success && (bindingAttr & BindingFlags.CreateInstance) == BindingFlags.CreateInstance)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetField) == BindingFlags.GetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetField) == BindingFlags.SetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetProperty) == BindingFlags.GetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetProperty) == BindingFlags.SetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType)
                {
                    // 実装されていません。
                }

                if (success && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                return success;
            }

            public bool BindToField(FieldDefinition method)
            {
                bool success =
                    this.name == string.Empty ||
                    this.name ==
                    ((bindingAttr & BindingFlags.IgnoreCase) == BindingFlags.IgnoreCase ?
                        method.Name.ToLower() :
                        method.Name);

                if (success && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
                {
                    success |= method.DeclaringType.Equivalent(type);
                }

                if (success && (bindingAttr & BindingFlags.Instance) == BindingFlags.Instance)
                {
                    success |= !method.IsStatic;
                }

                if (success && (bindingAttr & BindingFlags.Static) == BindingFlags.Static)
                {
                    success |= method.IsStatic;
                }

                if (success && (bindingAttr & BindingFlags.Public) == BindingFlags.Public)
                {
                    success |= method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic)
                {
                    success |= !method.IsPublic;
                }

                if (success && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
                {
                    success |= !method.DeclaringType.Equals(type);
                }

                if (success && (bindingAttr & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod)
                {
                    success |= true;
                }

                if (success && (bindingAttr & BindingFlags.CreateInstance) == BindingFlags.CreateInstance)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetField) == BindingFlags.GetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetField) == BindingFlags.SetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetProperty) == BindingFlags.GetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetProperty) == BindingFlags.SetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType)
                {
                    // 実装されていません。
                }

                if (success && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                return success;
            }

            public bool BindToProperty(PropertyDefinition property)
            {
                bool success =
                    this.name ==
                    ((bindingAttr & BindingFlags.IgnoreCase) == BindingFlags.IgnoreCase ?
                        property.Name.ToLower() :
                        property.Name);

                if (success && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
                {
                    success |= property.DeclaringType.Equivalent(type);
                }

                if (success && (bindingAttr & BindingFlags.Instance) == BindingFlags.Instance)
                {
                    success |= property.HasThis;
                }

                if (success && (bindingAttr & BindingFlags.Static) == BindingFlags.Static)
                {
                    success |= !property.HasThis;
                }

                if (success && (bindingAttr & BindingFlags.Public) == BindingFlags.Public)
                {
                    success |= (property.GetMethod.IsPublic || property.SetMethod.IsPublic);
                }

                if (success && (bindingAttr & BindingFlags.NonPublic) == BindingFlags.NonPublic)
                {
                    success |= !(property.GetMethod.IsPublic || property.SetMethod.IsPublic);
                }

                if (success && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
                {
                    success |= !property.DeclaringType.Equals(type);
                }

                if (success && (bindingAttr & BindingFlags.InvokeMethod) == BindingFlags.InvokeMethod)
                {
                    success |= true;
                }

                if (success && (bindingAttr & BindingFlags.CreateInstance) == BindingFlags.CreateInstance)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetField) == BindingFlags.GetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetField) == BindingFlags.SetField)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.GetProperty) == BindingFlags.GetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.SetProperty) == BindingFlags.SetProperty)
                {
                    success |= false;
                }

                if (success && (bindingAttr & BindingFlags.PutDispProperty) == BindingFlags.PutDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.PutRefDispProperty) == BindingFlags.PutRefDispProperty)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                if (success && (bindingAttr & BindingFlags.ExactBinding) == BindingFlags.ExactBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.SuppressChangeType) == BindingFlags.SuppressChangeType)
                {
                    // 実装されていません。
                }

                if (success && (bindingAttr & BindingFlags.OptionalParamBinding) == BindingFlags.OptionalParamBinding)
                {
                    // この場合は必要なさそう。
                }

                if (success && (bindingAttr & BindingFlags.IgnoreReturn) == BindingFlags.IgnoreReturn)
                {
                    // Cecil で COM オブジェクトサポートしてるのか？？
                }

                return success;
            }
        }
    }
}
