/* 
 * File: IMethodBodyGeneratorDecorator.g.cs
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
    public abstract class IMethodBodyGeneratorDecorator : IMethodBodyGenerator
    {
        protected readonly IMethodBodyGenerator source;
        public IMethodBodyGeneratorDecorator(IMethodBodyGenerator source)
        {
            this.source = source;
        }

        public virtual ILocalGenerator AddLocal(ILocalGenerator localGen) { return source.AddLocal(localGen); }
        public virtual IMethodBaseGenerator Method 
		{
			get { return source.Method; }
		}
        public virtual IILOperator ILOperator 
		{
			get { return source.ILOperator; }
		}
        public virtual ReadOnlyCollection<ILocalGenerator> Locals 
		{
			get { return source.Locals; }
		}
        public virtual ReadOnlyCollection<IDirectiveGenerator> Directives 
		{
			get { return source.Directives; }
		}
        IMethodBaseDeclaration IMethodBodyDeclaration.Method 
		{
			get { return Method; }
		}
        ReadOnlyCollection<ILocalDeclaration> locals;
        ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals 
		{
			get 
			{ 
				if (locals == null)
				{
					locals = new ReadOnlyCollection<ILocalDeclaration>(Locals.TransformEnumerateOnly(_ => (ILocalDeclaration)_));
				}
				return locals; 
			}
		}
        ReadOnlyCollection<IDirectiveDeclaration> directives;
        ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives 
		{
			get 
			{ 
				if (directives == null)
				{
					directives = new ReadOnlyCollection<IDirectiveDeclaration>(Directives.TransformEnumerateOnly(_ => (IDirectiveDeclaration)_));
				}
				return directives; 
			}
		}
	}
}
