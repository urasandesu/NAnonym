/* 
 * File: ReflectiveMethodBaseDecorator.cs
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
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools
{
    public abstract class ReflectiveMethodBaseDecorator : IMethodBaseGenerator
    {
        protected readonly ReadOnlyCollection<IParameterDeclaration> parameterDecls;
        protected readonly ReflectiveMethodDesigner gen;
        public ReflectiveMethodBaseDecorator(ReflectiveMethodDesigner gen)
        {
            this.gen = gen;
            parameterDecls = new ReadOnlyCollection<IParameterDeclaration>(gen.Method.Parameters.TransformEnumerateOnly(parameter => (IParameterDeclaration)parameter));
        }

        public ReflectiveMethodDesigner ExpressiveGenerator { get { return gen; } }
        public abstract ReflectiveMethodBodyDecorator BodyDecorator { get; }
        public virtual IMethodBodyGenerator Body { get { return BodyDecorator; } }
        public virtual ITypeGenerator DeclaringType { get { return gen.Method.DeclaringType; } }
        public virtual ReadOnlyCollection<IParameterGenerator> Parameters { get { return gen.Method.Parameters; } }
        public virtual IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            return gen.Method.AddPortableScopeItem(fieldInfo);
        }
        public virtual IMethodBaseGenerator ExpressBody(Action<ReflectiveMethodDesigner> bodyExpression)
        {
            return gen.Method.ExpressBody(bodyExpression);
        }
        public virtual IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return gen.Method.AddParameter(position, attributes, parameterName);
        }
        public virtual PortableScope CarryPortableScope()
        {
            return gen.Method.CarryPortableScope();
        }
        IMethodBodyDeclaration IMethodBaseDeclaration.Body { get { return BodyDecorator; } }
        ITypeDeclaration IMemberDeclaration.DeclaringType { get { return gen.Method.DeclaringType; } }
        ReadOnlyCollection<IParameterDeclaration> IMethodBaseDeclaration.Parameters { get { return parameterDecls; } }
        public virtual IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value)
        {
            return gen.Method.NewPortableScopeItem(itemRawData, value);
        }
        public virtual string Name { get { return gen.Method.Name; } }
        public virtual object Source { get { return gen.Method.Source; } }
        public virtual void OnDeserialized(StreamingContext context)
        {
            gen.Method.OnDeserialized(context);
        }


        public bool IsPublic
        {
            get { throw new NotImplementedException(); }
        }
    }
}
