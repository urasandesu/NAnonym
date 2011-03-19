/* 
 * File: MockTypeGenerator.cs
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
using System.Reflection;
using System.Runtime.Serialization;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.ILTools
{
    class MockTypeGenerator : ITypeGenerator
    {
        ITypeDeclaration type;
        public MockTypeGenerator(Type type)
            : this(type.ToTypeDecl())
        {
        }

        public MockTypeGenerator(ITypeDeclaration type)
        {
            this.type = type;
        }

        public IFieldGenerator AddField(string fieldName, Type type, FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        public IMethodGenerator AddMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public IMethodGenerator AddMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IFieldGenerator> Fields
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IConstructorGenerator> Constructors
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IMethodGenerator> Methods
        {
            get { throw new NotImplementedException(); }
        }

        public IModuleGenerator Module
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeGenerator AddInterfaceImplementation(Type interfaceType)
        {
            throw new NotImplementedException();
        }

        public ITypeGenerator SetParent(Type parentType)
        {
            throw new NotImplementedException();
        }

        public void AddDefaultConstructor()
        {
            throw new NotImplementedException();
        }

        public IConstructorGenerator AddConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public string AssemblyQualifiedName
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDeclaration BaseType
        {
            get { throw new NotImplementedException(); }
        }

        IModuleDeclaration ITypeDeclaration.Module
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IFieldDeclaration> ITypeDeclaration.Fields
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IConstructorDeclaration> ITypeDeclaration.Constructors
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IMethodDeclaration> ITypeDeclaration.Methods
        {
            get { throw new NotImplementedException(); }
        }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            throw new NotImplementedException();
        }

        public Type Source
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsValueType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAssignableFrom(ITypeDeclaration that)
        {
            return type.IsAssignableFrom(that);
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

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        object IMemberDeclaration.Source
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDeclaration DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return type.Equals(obj);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public override string ToString()
        {
            return type.ToString();
        }


        public bool EqualsWithoutGenericArguments(ITypeDeclaration that)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IPropertyDeclaration> Properties
        {
            get { throw new NotImplementedException(); }
        }


        public bool IsAssignableExplicitlyFrom(ITypeDeclaration that)
        {
            throw new NotImplementedException();
        }
    }
}
