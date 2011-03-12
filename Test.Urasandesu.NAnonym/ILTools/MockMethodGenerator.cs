/* 
 * File: MockMethodGenerator.cs
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

namespace Test.Urasandesu.NAnonym.ILTools
{
    class MockMethodGenerator : IMethodGenerator
    {
        public MockMethodGenerator()
        {
            ReturnTypeProvider = () => default(ITypeGenerator);
            BodyProvider = () => new MockMethodBodyGenerator();
        }

        public Func<ITypeGenerator> ReturnTypeProvider { get; set; }
        public Func<IMethodBodyGenerator> BodyProvider { get; set; }

        public ITypeGenerator ReturnType
        {
            get { return ReturnTypeProvider(); }
        }

        ITypeDeclaration IMethodDeclaration.ReturnType
        {
            get { throw new NotImplementedException(); }
        }

        IMethodBodyDeclaration IMethodBaseDeclaration.Body
        {
            get { return Body; }
        }

        public ReadOnlyCollection<IParameterDeclaration> Parameters
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

        public ITypeDeclaration DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public IMethodBodyGenerator Body
        {
            get { return BodyProvider(); }
        }

        ITypeGenerator IMethodBaseGenerator.DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        ReadOnlyCollection<IParameterGenerator> IMethodBaseGenerator.Parameters
        {
            get { throw new NotImplementedException(); }
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
    }
}
