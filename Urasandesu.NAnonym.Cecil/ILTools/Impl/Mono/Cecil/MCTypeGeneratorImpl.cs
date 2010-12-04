/* 
 * File: MCTypeGeneratorImpl.cs
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
using Mono.Cecil;
using Urasandesu.NAnonym.Linq;
using MC = Mono.Cecil;
using SR = System.Reflection;
using UNI = Urasandesu.NAnonym.ILTools;


namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCTypeGeneratorImpl : MCTypeDeclarationImpl, UNI::ITypeGenerator
    {
        [NonSerialized]
        ReadOnlyCollection<UNI::IFieldGenerator> fields;

        [NonSerialized]
        ReadOnlyCollection<UNI::IConstructorGenerator> constructors;

        [NonSerialized]
        ReadOnlyCollection<UNI::IMethodGenerator> methods;

        public MCTypeGeneratorImpl(TypeDefinition typeDef)
            : base(typeDef)
        {
            fields = new ReadOnlyCollection<UNI::IFieldGenerator>(base.Fields.TransformEnumerateOnly(fieldDecl => (UNI::IFieldGenerator)fieldDecl));
            constructors = new ReadOnlyCollection<UNI::IConstructorGenerator>(base.Constructors.TransformEnumerateOnly(constructorDecl => (UNI::IConstructorGenerator)constructorDecl));
            methods = new ReadOnlyCollection<UNI::IMethodGenerator>(base.Methods.TransformEnumerateOnly(methodDecl => (UNI::IMethodGenerator)methodDecl));
        }

        public MCTypeGeneratorImpl(TypeDefinition typeDef, UNI::IModuleDeclaration moduleDecl)
            : base(typeDef, moduleDecl)
        {
            fields = new ReadOnlyCollection<UNI::IFieldGenerator>(base.Fields.TransformEnumerateOnly(fieldDecl => (UNI::IFieldGenerator)fieldDecl));
            constructors = new ReadOnlyCollection<UNI::IConstructorGenerator>(base.Constructors.TransformEnumerateOnly(constructorDecl => (UNI::IConstructorGenerator)constructorDecl));
            methods = new ReadOnlyCollection<UNI::IMethodGenerator>(base.Methods.TransformEnumerateOnly(methodDecl => (UNI::IMethodGenerator)methodDecl));
        }

        internal new TypeDefinition TypeDef
        {
            get { return base.TypeDef; }
        }

        public UNI::IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldDef = new FieldDefinition(fieldName, (MC::FieldAttributes)attributes, TypeDef.Module.Import(type));
            TypeDef.Fields.Add(fieldDef);
            return (MCFieldGeneratorImpl)fieldDef;
        }

        public UNI::IMethodGenerator AddMethod(string name, SR::MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodDef = new MethodDefinition(name, (MethodAttributes)attributes, TypeDef.Module.Import(returnType));
            TypeDef.Methods.Add(methodDef);
            parameterTypes.ForEach(parameterType => methodDef.Parameters.Add(new ParameterDefinition(TypeDef.Module.Import(parameterType))));
            return new MCMethodGeneratorImpl(methodDef);
        }

        public new ReadOnlyCollection<UNI::IFieldGenerator> Fields
        {
            get { return fields; }
        }

        public new UNI::IModuleGenerator Module
        {
            get { return base.Module as UNI::IModuleGenerator; }
        }

        public UNI::IMethodGenerator AddMethod(string name, SR::MethodAttributes attributes, SR::CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public UNI::ITypeGenerator AddInterfaceImplementation(Type interfaceType)
        {
            throw new NotImplementedException();
        }

        public UNI::IConstructorGenerator AddConstructor(SR::MethodAttributes attributes, SR::CallingConventions callingConvention, Type[] parameterTypes)
        {
            var constructorDef = new MethodDefinition(".ctor", (MethodAttributes)attributes, TypeDef.Module.Import(typeof(void)));
            //constructorDef.CallingConvention = callingConvention.ToCecil();   // TODO: うまく動いてなさそう？
            parameterTypes.Select(parameterType => new ParameterDefinition(TypeDef.Module.Import(parameterType))).AddRangeTo(constructorDef.Parameters);
            TypeDef.Methods.Add(constructorDef);
            return new MCConstructorGeneratorImpl(constructorDef);
        }

        public UNI::ITypeGenerator SetParent(Type parentType)
        {
            throw new NotImplementedException();
        }

        public new ReadOnlyCollection<UNI::IConstructorGenerator> Constructors
        {
            get { return constructors; }
        }

        public new ReadOnlyCollection<UNI::IMethodGenerator> Methods
        {
            get { return methods; }
        }

        public UNI::IMethodGenerator AddMethod(UNI::IMethodGenerator methodGen)
        {
            throw new NotImplementedException();
        }
    }
}

