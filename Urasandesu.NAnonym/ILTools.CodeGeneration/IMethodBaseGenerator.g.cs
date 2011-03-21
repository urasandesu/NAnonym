/* 
 * File: IMethodBaseGeneratorDecorator.g.cs
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
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools
{
    public abstract class IMethodBaseGeneratorDecorator : IMethodBaseGenerator
    {
        protected readonly IMethodBaseGenerator source;
        public IMethodBaseGeneratorDecorator(IMethodBaseGenerator source)
        {
            this.source = source;
        }

        public virtual IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo) { return source.AddPortableScopeItem(fieldInfo); }
        public virtual IMethodBaseGenerator ExpressBody(Action<ReflectiveMethodDesigner> bodyExpression) { return source.ExpressBody(bodyExpression); }
        public virtual IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression, ITypeDeclaration returnType) { return source.ExpressBody2(bodyExpression, returnType); }
        public virtual IParameterGenerator AddParameter(Int32 position, ParameterAttributes attributes, String parameterName) { return source.AddParameter(position, attributes, parameterName); }
        public virtual PortableScope CarryPortableScope() { return source.CarryPortableScope(); }
        public virtual IMethodBodyGenerator Body 
		{
			get { return source.Body; }
		}
        public virtual ITypeGenerator DeclaringType 
		{
			get { return source.DeclaringType; }
		}
        public virtual ReadOnlyCollection<IParameterGenerator> Parameters 
		{
			get { return source.Parameters; }
		}
        public virtual IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, Object value) { return source.NewPortableScopeItem(itemRawData, value); }
        IMethodBodyDeclaration IMethodBaseDeclaration.Body 
		{
			get { return Body; }
		}
        ReadOnlyCollection<IParameterDeclaration> parameters;
        ReadOnlyCollection<IParameterDeclaration> IMethodBaseDeclaration.Parameters 
		{
			get 
			{ 
				if (parameters == null)
				{
					parameters = new ReadOnlyCollection<IParameterDeclaration>(Parameters.TransformEnumerateOnly(_ => (IParameterDeclaration)_));
				}
				return parameters; 
			}
		}
        public virtual Boolean IsStatic 
		{
			get { return source.IsStatic; }
		}
        public virtual String Name 
		{
			get { return source.Name; }
		}
        public virtual Object Source 
		{
			get { return source.Source; }
		}
        ITypeDeclaration IMemberDeclaration.DeclaringType 
		{
			get { return DeclaringType; }
		}
        public virtual void OnDeserialized(StreamingContext context) { source.OnDeserialized(context); }
        void IManuallyDeserializable.OnDeserialized(StreamingContext context) { OnDeserialized(context); }
        String IMemberDeclaration.Name 
		{
			get { return Name; }
		}
        Object IMemberDeclaration.Source 
		{
			get { return Source; }
		}
	}
}
