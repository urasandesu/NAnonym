/* 
 * File: MockMethodBaseGenerator.cs
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
using System.Collections.Generic;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.ILTools
{
    class MockMethodBaseGenerator : IMethodBaseGenerator
    {
        public Collection<IParameterGenerator> Parameters { get; private set; }

        public MockMethodBaseGenerator()
        {
            BodyProvider = () => new MockMethodBodyGenerator();
            Parameters = new Collection<IParameterGenerator>();
            var readonlyParameters = new ReadOnlyCollection<IParameterGenerator>(Parameters);
            ParametersProvider = () => readonlyParameters;
        }

        public Func<IMethodBodyGenerator> BodyProvider { get; set; }
        public Func<ReadOnlyCollection<IParameterGenerator>> ParametersProvider { get; set; }

        public IMethodBodyGenerator Body
        {
            get { return BodyProvider(); }
        }

        public ITypeGenerator DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IParameterGenerator> IMethodBaseGenerator.Parameters
        {
            get { return ParametersProvider(); }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator ExpressBody(Action<ReflectiveMethodDesigner> bodyExpression)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression)
        {
            throw new NotImplementedException();
        }

        public IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            throw new NotImplementedException();
        }

        public PortableScope CarryPortableScope()
        {
            throw new NotImplementedException();
        }

        IMethodBodyDeclaration IMethodBaseDeclaration.Body
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IParameterDeclaration> IMethodBaseDeclaration.Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        ITypeDeclaration IMemberDeclaration.DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }


        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression, ITypeDeclaration returnType)
        {
            throw new NotImplementedException();
        }


        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
        }
    }

    class MockParameterGenerator : IParameterGenerator
    {
        public MockParameterGenerator(Type parameterType)
        {
            var _parameterType = new MockTypeGenerator(parameterType.ToTypeDecl());
            ParameterTypeProvider = () => _parameterType;
        }

        public Func<ITypeGenerator> ParameterTypeProvider { get; set; }

        public ITypeGenerator ParameterType
        {
            get { return ParameterTypeProvider(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public int Position
        {
            get { throw new NotImplementedException(); }
        }

        ITypeDeclaration IParameterDeclaration.ParameterType
        {
            get { return ParameterType; }
        }
    }
}
