/* 
 * File: MCTypeDeclarationImpl.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCTypeDeclarationImpl : MCMemberDeclarationImpl, ITypeDeclaration
    {
        [NonSerialized]
        internal TypeReference typeRef;

        [NonSerialized]
        TypeDefinition typeDef;

        [NonSerialized]
        ITypeDeclaration baseTypeDecl;

        string typeFullName;

        IModuleDeclaration moduleDecl;

        [NonSerialized]
        ReadOnlyCollection<IConstructorDeclaration> constructors;

        [NonSerialized]
        ReadOnlyCollection<IMethodDeclaration> methods;

        int lastMethodsCount = -1;

        [NonSerialized]
        ReadOnlyCollection<IFieldDeclaration> fields;
        
        public MCTypeDeclarationImpl(TypeReference typeRef)
            : this(typeRef, null)
        {
        }

        public MCTypeDeclarationImpl(TypeReference typeRef, IModuleDeclaration moduleDecl)
            : base(typeRef)
        {
            Initialize(typeRef, moduleDecl);
        }

        void Initialize(TypeReference typeRef, IModuleDeclaration moduleDecl)
        {
            this.typeRef = typeRef;
            typeDef = typeRef.Resolve();
            typeFullName = typeDef.FullName;
            this.moduleDecl = moduleDecl;
            fields = new ReadOnlyCollection<IFieldDeclaration>(typeDef.Fields.TransformEnumerateOnly(fieldDef => (IFieldDeclaration)new MCFieldGeneratorImpl(fieldDef)));
            InitializeMembersIfNecessary(this);
        }

        void InitializeMembersIfNecessary(MCTypeDeclarationImpl @this)
        {
            if (lastMethodsCount < 0)
            {
                @this.constructors = new ReadOnlyCollection<IConstructorDeclaration>(new IConstructorDeclaration[] { });
                @this.methods = new ReadOnlyCollection<IMethodDeclaration>(new IMethodDeclaration[] { });
                lastMethodsCount = 0;
            }

            if (lastMethodsCount != typeDef.Methods.Count)
            {
                var constructors = typeDef.Methods.Where(methodDef => methodDef.Name == ".ctor").ToArray();
                @this.constructors = new ReadOnlyCollection<IConstructorDeclaration>(
                    constructors.TransformEnumerateOnly(constructorDef => (IConstructorDeclaration)new MCConstructorGeneratorImpl(constructorDef)));

                var methods = typeDef.Methods.Where(methodDef => methodDef.Name != ".ctor").ToArray();
                @this.methods = new ReadOnlyCollection<IMethodDeclaration>(
                    methods.TransformEnumerateOnly(methodDef => (IMethodDeclaration)new MCMethodGeneratorImpl(methodDef)));
            }
        }

        public string FullName
        {
            get { return typeRef.FullName; }
        }

        public string AssemblyQualifiedName
        {
            get { return typeRef.FullName + ", " + typeRef.Module.Assembly.FullName; }
        }

        public ITypeDeclaration BaseType
        {
            get 
            {
                if (baseTypeDecl == null)
                {
                    baseTypeDecl = typeDef.BaseType == null || typeRef.Equivalent(typeof(object)) ? null : new MCTypeDeclarationImpl(typeDef.BaseType, moduleDecl);
                }
                return baseTypeDecl; 
            }
        }

        public IModuleDeclaration Module 
        { 
            get 
            {
                if (moduleDecl == null)
                {
                    moduleDecl = new MCModuleGeneratorImpl(typeRef.Module);
                }
                return moduleDecl; 
            } 
        }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            // TODO: 本当は SR::BindingFlags.Default が正しい。修正。
            // MEMO: System.Object..ctor をそのまま参照させると、自身のコンストラクタ呼び出しに変換されてしまう？？
            if (typeRef.Equivalent(typeof(object)))
            {
                return new MCConstructorDeclarationImpl(typeRef.Module.Import(
                    typeof(object).GetConstructor(SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, null, types, null)));
            }
            else
            {
                return new MCConstructorDeclarationImpl(typeDef.GetConstructor(
                    SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, types));
            }
        }

        protected TypeDefinition TypeDef { get { return typeDef; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var moduleDecl = (MCModuleDeclarationImpl)this.moduleDecl;
            moduleDecl.OnDeserialized(context);
            var moduleDef = moduleDecl.ModuleDef;
            var typeDef = moduleDef.Types.First(type => type.FullName == typeFullName);
            Initialize(typeDef, moduleDecl);
            base.OnDeserializedManually(context);
        }

        public IFieldDeclaration[] GetFields(BindingFlags attr)
        {
            return typeDef.GetFields(attr).Select(fieldDef => (IFieldDeclaration)(MCFieldGeneratorImpl)fieldDef).ToArray();
        }

        public IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldDef = typeDef.GetFieldOrDefault(name, bindingAttr);
            return fieldDef == null ? null : new MCFieldGeneratorImpl(fieldDef);
        }

        public ReadOnlyCollection<IFieldDeclaration> Fields
        {
            get { return fields; }
        }

        public ReadOnlyCollection<IConstructorDeclaration> Constructors
        {
            get
            {
                InitializeMembersIfNecessary(this);
                return constructors;
            }
        }

        public ReadOnlyCollection<IMethodDeclaration> Methods
        {
            get 
            {
                InitializeMembersIfNecessary(this);
                return methods;
            }
        }

        public new Type Source
        {
            get { return typeDef.ToType(); }
        }


        public bool IsValueType
        {
            get { return typeDef.IsValueType; }
        }


        public bool IsAssignableFrom(ITypeDeclaration that)
        {
            var mcthat = default(MCTypeDeclarationImpl);
            if ((mcthat = that as MCTypeDeclarationImpl) != null)
            {
                return typeDef.IsAssignableFrom(mcthat.typeDef);
            }
            else
            {
                if (FullName == that.FullName)
                {
                    return true;
                }
                else if (IsAssignableFrom(that.BaseType))
                {
                    return true;
                }
                else
                {
                    return that.Interfaces.Any(_ => FullName == _.FullName);
                }
            }
        }

        public ReadOnlyCollection<ITypeDeclaration> Interfaces
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDeclaration MakeArrayType()
        {
            throw new NotImplementedException();
        }

        public ITypeDeclaration GetElementType()
        {
            throw new NotImplementedException();
        }
    }
}

