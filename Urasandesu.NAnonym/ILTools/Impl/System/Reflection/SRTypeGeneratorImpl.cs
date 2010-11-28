/* 
 * File: SRTypeGeneratorImpl.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        List<FieldBuilder> fieldBuilders;
        List<IFieldGenerator> listFields;
        ReadOnlyCollection<IFieldGenerator> fields;

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : this(typeBuilder, new FieldBuilder[] { })
        {
        }

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
            : base(typeBuilder)
        {
            this.fieldBuilders = new List<FieldBuilder>(fieldBuilders);
            listFields = new List<IFieldGenerator>();
            listFields.AddRange(fieldBuilders.Select(fieldBuilder => (IFieldGenerator)new SRFieldGeneratorImpl(fieldBuilder)));
            fields = new ReadOnlyCollection<IFieldGenerator>(listFields.TransformEnumerateOnly(fieldGen => (IFieldGenerator)fieldGen));
        }

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldBuilder = Source.DefineField(fieldName, type, attributes);
            var fieldGen = new SRFieldGeneratorImpl(fieldBuilder);
            listFields.Add(fieldGen);
            return fieldGen;
        }

        public IMethodGenerator AddMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodBuilder = Source.DefineMethod(name, attributes, returnType, parameterTypes);
            return new SRMethodGeneratorImpl(methodBuilder);
        }

        public override IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldBuilder = fieldBuilders.FirstOrDefault(_fieldBuilder => _fieldBuilder.Name == name && (_fieldBuilder.ExportBinding() & bindingAttr) != 0);
            return fieldBuilder == null ? null : new SRFieldGeneratorImpl(fieldBuilder);
        }

        internal FieldBuilder[] FieldBuilders { get { return fieldBuilders.ToArray(); } }
        internal new TypeBuilder Source { get { return (TypeBuilder)base.Source; } }

        public new ReadOnlyCollection<IFieldGenerator> Fields
        {
            get { return fields; }
        }

        public new IModuleGenerator Module
        {
            get { return base.Module as IModuleGenerator; }
        }

        public ITypeGenerator AddInterfaceImplementation(Type interfaceType)
        {
            Source.AddInterfaceImplementation(interfaceType);
            return this;
        }

        public IConstructorGenerator AddConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
        {
            var constructorBuilder = Source.DefineConstructor(attributes, callingConvention, parameterTypes);
            var constructorGen = new SRConstructorGeneratorImpl(this, constructorBuilder);
            return constructorGen;
        }

        public IMethodGenerator AddMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            var methodBuilder = Source.DefineMethod(name, attributes, callingConvention, returnType, parameterTypes);
            var methodGen = new SRMethodGeneratorImpl(this, methodBuilder);
            return methodGen;
        }

        public ITypeGenerator SetParent(Type parentType)
        {
            Source.SetParent(parentType);
            return this;
        }

        public new ReadOnlyCollection<IConstructorGenerator> Constructors
        {
            get { throw new NotImplementedException(); }
        }

        public new ReadOnlyCollection<IMethodGenerator> Methods
        {
            get { throw new NotImplementedException(); }
        }

        public IMethodGenerator AddMethod(IMethodGenerator methodGen)
        {
            throw new NotImplementedException();
        }
    }
}

