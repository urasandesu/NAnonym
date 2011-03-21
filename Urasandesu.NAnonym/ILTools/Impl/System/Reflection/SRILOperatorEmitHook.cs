/* 
 * File: SRILOperatorEmitHook.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
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
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;
using System.Collections.Generic;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRILOperatorEmitHook : IILOperatorDecorator
    {
        int localIndex;
        int labelIndex;
        string ilName;
        ReflectiveMethodDesigner2 gen;

        Dictionary<ConstructorLocalCacheKey, ILocalGenerator> constructorLocalCaches;
        Dictionary<MethodLocalCacheKey, ILocalGenerator> methodLocalCaches;
        Dictionary<FieldLocalCacheKey, ILocalGenerator> fieldLocalCaches;

        public SRILOperatorEmitHook(IILOperator source, string ilName, ReflectiveMethodDesigner2 gen)
            : base(source)
        {
            this.ilName = ilName;
            this.gen = gen;

            constructorLocalCaches = new Dictionary<ConstructorLocalCacheKey, ILocalGenerator>();
            methodLocalCaches = new Dictionary<MethodLocalCacheKey, ILocalGenerator>();
            fieldLocalCaches = new Dictionary<FieldLocalCacheKey, ILocalGenerator>();
        }

        public override ILocalGenerator AddLocal(String name, Type localType)
        {
            var local = new SRLocalEmitHook(name, localType.ToTypeDecl(), localIndex++);
            gen.Eval(() => Dsl.Store<LocalBuilder>(local.Name).As(Dsl.Load<ILGenerator>(ilName).DeclareLocal(Dsl.Extract(localType))));
            return local;
        }

        public override ILocalGenerator AddLocal(String name, ITypeDeclaration localType)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            if ((srimpl = localType as SRTypeDeclarationImpl) == null)
                throw new NotSupportedException();

            var local = new SRLocalEmitHook(name, localType, localIndex++);
            gen.Eval(() => Dsl.Store<LocalBuilder>(local.Name).As(Dsl.Load<ILGenerator>(ilName).DeclareLocal(Dsl.Extract(srimpl.type))));
            return local;
        }

        public override ILocalGenerator AddLocal(Type localType)
        {
            var local = new SRLocalEmitHook(localType.ToTypeDecl(), localIndex++);
            gen.Eval(() => Dsl.Store<LocalBuilder>(local.Name).As(Dsl.Load<ILGenerator>(ilName).DeclareLocal(Dsl.Extract(localType))));
            return local;
        }

        public override ILocalGenerator AddLocal(ITypeDeclaration localType)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            if ((srimpl = localType as SRTypeDeclarationImpl) == null)
                throw new NotSupportedException();

            var local = new SRLocalEmitHook(localType, localIndex++);
            gen.Eval(() => Dsl.Store<LocalBuilder>(local.Name).As(Dsl.Load<ILGenerator>(ilName).DeclareLocal(Dsl.Extract(srimpl.type))));
            return local;
        }

        public override ILocalGenerator AddLocal(Type localType, Boolean pinned)
        {
            return source.AddLocal(localType, pinned);
        }

        public override ILabelGenerator AddLabel()
        {
            var label = new SRLabelEmitHook(labelIndex++);
            gen.Eval(() => Dsl.Store<Label>(label.Name).As(Dsl.Load<ILGenerator>(ilName).DefineLabel()));
            return label;
        }

        public override void Emit(OpCode opcode)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes))));
        }

        public override void Emit(OpCode opcode, Byte arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, ConstructorInfo con)
        {
            var declaringType = con.DeclaringType;
            var bindingAttr = con.ExportBinding();
            var paramTypes = con.ParameterTypes();
            var key = new ConstructorLocalCacheKey(declaringType, bindingAttr, paramTypes);
            if (!constructorLocalCaches.ContainsKey(key))
                constructorLocalCaches.Add(key, new SRLocalEmitHook(typeof(ConstructorInfo).ToTypeDecl(), localIndex++));
            var local = constructorLocalCaches[key];
            gen.Eval(() => Dsl.Store<ConstructorInfo>(local.Name).As(Dsl.Extract(declaringType).GetConstructor(Dsl.Extract(bindingAttr), null, Dsl.Extract(paramTypes), null)));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<ConstructorInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, Double arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, FieldInfo field)
        {
            var name = field.Name;
            var declaringType = field.DeclaringType;
            var bindingAttr = field.ExportBinding();
            var key = new FieldLocalCacheKey(name, declaringType, bindingAttr);
            if (!fieldLocalCaches.ContainsKey(key))
                fieldLocalCaches.Add(key, new SRLocalEmitHook(typeof(FieldInfo).ToTypeDecl(), localIndex++));
            var local = fieldLocalCaches[key];
            gen.Eval(() => Dsl.Store<FieldInfo>(local.Name).As(Dsl.Extract(declaringType).GetField(Dsl.Extract(name), Dsl.Extract(bindingAttr))));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<FieldInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, Single arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, Int32 arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, ILabelDeclaration label)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<Label>(label.Name)));
        }

        public override void Emit(OpCode opcode, ILabelDeclaration[] labels)
        {
            source.Emit(opcode, labels);
        }

        public override void Emit(OpCode opcode, ILocalDeclaration local)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<LocalBuilder>(local.Name)));
        }

        public override void Emit(OpCode opcode, Int64 arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, MethodInfo meth)
        {
            var name = meth.Name;
            var declaringType = meth.DeclaringType;
            var bindingAttr = meth.ExportBinding();
            var paramTypes = meth.ParameterTypes();
            var key = new MethodLocalCacheKey(name, declaringType, bindingAttr, paramTypes);
            if (!methodLocalCaches.ContainsKey(key))
                methodLocalCaches.Add(key, new SRLocalEmitHook(typeof(MethodInfo).ToTypeDecl(), localIndex++));
            var local = methodLocalCaches[key];
            gen.Eval(() => Dsl.Store<MethodInfo>(local.Name).As(Dsl.Extract(declaringType).GetMethod(Dsl.Extract(name), Dsl.Extract(bindingAttr), null, Dsl.Extract(paramTypes), null)));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<MethodInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, SByte arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, Int16 arg)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(arg)));
        }

        public override void Emit(OpCode opcode, String str)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(str)));
        }

        public override void Emit(OpCode opcode, Type cls)
        {
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(cls)));
        }

        public override void Emit(OpCode opcode, ITypeDeclaration type)
        {
            var srimpl = default(SRTypeDeclarationImpl);
            if ((srimpl = type as SRTypeDeclarationImpl) == null)
                throw new NotSupportedException();

            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(srimpl.type)));
        }

        public override void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            var srimpl = default(SRConstructorDeclarationImpl);
            if ((srimpl = constructorDecl as SRConstructorDeclarationImpl) == null)
                throw new NotSupportedException();

            var declaringType = srimpl.ConstructorInfo.DeclaringType;
            var bindingAttr = srimpl.ConstructorInfo.ExportBinding();
            var paramTypes = srimpl.ConstructorInfo.ParameterTypes();
            var key = new ConstructorLocalCacheKey(declaringType, bindingAttr, paramTypes);
            if (!constructorLocalCaches.ContainsKey(key))
                constructorLocalCaches.Add(key, new SRLocalEmitHook(typeof(ConstructorInfo).ToTypeDecl(), localIndex++));
            var local = constructorLocalCaches[key];
            gen.Eval(() => Dsl.Store<ConstructorInfo>(local.Name).As(Dsl.Extract(declaringType).GetConstructor(Dsl.Extract(bindingAttr), null, Dsl.Extract(paramTypes), null)));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<ConstructorInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, IMethodDeclaration methodDecl)
        {
            var srimpl = default(SRMethodDeclarationImpl);
            if ((srimpl = methodDecl as SRMethodDeclarationImpl) == null)
                throw new NotSupportedException();

            var name = srimpl.MethodInfo.Name;
            var declaringType = srimpl.MethodInfo.DeclaringType;
            var bindingAttr = srimpl.MethodInfo.ExportBinding();
            var paramTypes = srimpl.MethodInfo.ParameterTypes();
            var key = new MethodLocalCacheKey(name, declaringType, bindingAttr, paramTypes);
            if (!methodLocalCaches.ContainsKey(key))
                methodLocalCaches.Add(key, new SRLocalEmitHook(typeof(MethodInfo).ToTypeDecl(), localIndex++));
            var local = methodLocalCaches[key];
            gen.Eval(() => Dsl.Store<MethodInfo>(local.Name).As(Dsl.Extract(declaringType).GetMethod(Dsl.Extract(name), Dsl.Extract(bindingAttr), null, Dsl.Extract(paramTypes), null)));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<MethodInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            var position = (short)parameterDecl.Position;
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Extract(position)));
        }

        public override void Emit(OpCode opcode, IFieldDeclaration fieldDecl)
        {
            var srimpl = default(SRFieldDeclarationImpl);
            if ((srimpl = fieldDecl as SRFieldDeclarationImpl) == null)
                throw new NotSupportedException();

            var name = srimpl.FieldInfo.Name;
            var declaringType = srimpl.FieldInfo.DeclaringType;
            var bindingAttr = srimpl.FieldInfo.ExportBinding();
            var key = new FieldLocalCacheKey(name, declaringType, bindingAttr);
            if (!fieldLocalCaches.ContainsKey(key))
                fieldLocalCaches.Add(key, new SRLocalEmitHook(typeof(FieldInfo).ToTypeDecl(), localIndex++));
            var local = fieldLocalCaches[key];
            gen.Eval(() => Dsl.Store<FieldInfo>(local.Name).As(Dsl.Extract(declaringType).GetField(Dsl.Extract(name), Dsl.Extract(bindingAttr))));
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).Emit(Dsl.ConstMember(opcode.ToClr(), typeof(SRE::OpCodes)), Dsl.Load<FieldInfo>(local.Name)));
        }

        public override void Emit(OpCode opcode, IPortableScopeItem scopeItem)
        {
            source.Emit(opcode, scopeItem);
        }

        public override void SetLabel(ILabelDeclaration loc)
        {
            var label = (SRLabelEmitHook)loc;
            gen.Eval(() => Dsl.Load<ILGenerator>(ilName).MarkLabel(Dsl.Load<Label>(label.Name)));
        }

        public override Object Source
        {
            get { return source.Source; }
        }


    

        struct ConstructorLocalCacheKey : IEqualityComparer<ConstructorLocalCacheKey>
        {
            public ConstructorLocalCacheKey(Type declaringType, BindingFlags bindingAttr, Type[] parameterTypes)
            {
                this.declaringType = declaringType;
                this.bindingFlags = bindingAttr;
                this.parameterTypes = parameterTypes;
            }

            Type declaringType;
            public Type DeclaringType { get { return declaringType; } }
            BindingFlags bindingFlags;
            public BindingFlags BindingFlags { get { return bindingFlags; } }
            Type[] parameterTypes;
            public Type[] ParameterTypes { get { return parameterTypes; } }

            public override bool Equals(object obj)
            {
                if (!(obj is ConstructorLocalCacheKey)) return false;
                return Equals(this, (ConstructorLocalCacheKey)obj);
            }

            public override int GetHashCode()
            {
                return GetHashCode(this);
            }

            public bool Equals(ConstructorLocalCacheKey x, ConstructorLocalCacheKey y)
            {
                var equals = true;
                equals = equals && x.DeclaringType == y.DeclaringType;
                equals = equals && x.BindingFlags == y.BindingFlags;
                equals = equals && x.ParameterTypes.Equivalent(y.ParameterTypes);
                return equals;
            }

            public int GetHashCode(ConstructorLocalCacheKey obj)
            {
                var hashCode = 0;
                hashCode = hashCode ^ obj.DeclaringType.NullableGetHashCode();
                hashCode = hashCode ^ obj.BindingFlags.GetHashCode();
                hashCode = hashCode ^ obj.ParameterTypes.GetAggregatedHashCodeOrDefault();
                return hashCode;
            }
        }

        struct MethodLocalCacheKey : IEqualityComparer<MethodLocalCacheKey>
        {
            public MethodLocalCacheKey(string name, Type declaringType, BindingFlags bindingAttr, Type[] parameterTypes)
            {
                this.name = name;
                this.declaringType = declaringType;
                this.bindingFlags = bindingAttr;
                this.parameterTypes = parameterTypes;
            }

            string name;
            public string Name { get { return name; } }
            Type declaringType;
            public Type DeclaringType { get { return declaringType; } }
            BindingFlags bindingFlags;
            public BindingFlags BindingFlags { get { return bindingFlags; } }
            Type[] parameterTypes;
            public Type[] ParameterTypes { get { return parameterTypes; } }

            public override bool Equals(object obj)
            {
                if (!(obj is MethodLocalCacheKey)) return false;
                return Equals(this, (MethodLocalCacheKey)obj);
            }

            public override int GetHashCode()
            {
                return GetHashCode(this);
            }

            public bool Equals(MethodLocalCacheKey x, MethodLocalCacheKey y)
            {
                var equals = true;
                equals = equals && x.Name == y.Name;
                equals = equals && x.DeclaringType == y.DeclaringType;
                equals = equals && x.BindingFlags == y.BindingFlags;
                equals = equals && x.ParameterTypes.Equivalent(y.ParameterTypes);
                return equals;
            }

            public int GetHashCode(MethodLocalCacheKey obj)
            {
                var hashCode = 0;
                hashCode = hashCode ^ obj.Name.NullableGetHashCode();
                hashCode = hashCode ^ obj.DeclaringType.NullableGetHashCode();
                hashCode = hashCode ^ obj.BindingFlags.GetHashCode();
                hashCode = hashCode ^ obj.ParameterTypes.GetAggregatedHashCodeOrDefault();
                return hashCode;
            }
        }

        struct FieldLocalCacheKey : IEqualityComparer<FieldLocalCacheKey>
        {
            public FieldLocalCacheKey(string name, Type declaringType, BindingFlags bindingAttr)
            {
                this.name = name;
                this.declaringType = declaringType;
                this.bindingFlags = bindingAttr;
            }

            string name;
            public string Name { get { return name; } }
            Type declaringType;
            public Type DeclaringType { get { return declaringType; } }
            BindingFlags bindingFlags;
            public BindingFlags BindingFlags { get { return bindingFlags; } }

            public override bool Equals(object obj)
            {
                if (!(obj is FieldLocalCacheKey)) return false;
                return Equals(this, (FieldLocalCacheKey)obj);
            }

            public override int GetHashCode()
            {
                return GetHashCode(this);
            }

            public bool Equals(FieldLocalCacheKey x, FieldLocalCacheKey y)
            {
                var equals = true;
                equals = equals && x.Name == y.Name;
                equals = equals && x.DeclaringType == y.DeclaringType;
                equals = equals && x.BindingFlags == y.BindingFlags;
                return equals;
            }

            public int GetHashCode(FieldLocalCacheKey obj)
            {
                var hashCode = 0;
                hashCode = hashCode ^ obj.Name.NullableGetHashCode();
                hashCode = hashCode ^ obj.DeclaringType.NullableGetHashCode();
                hashCode = hashCode ^ obj.BindingFlags.GetHashCode();
                return hashCode;
            }
        }
    }
}
