/* 
 * File: ReflectiveMethodBodyDecorator.cs
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

using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools
{
    public abstract class ReflectiveMethodBodyDecorator : IMethodBodyGenerator
    {
        protected readonly ReflectiveMethodBaseDecorator methodDecorator;
        protected readonly ReadOnlyCollection<ILocalDeclaration> localDecls;
        protected readonly ReadOnlyCollection<IDirectiveDeclaration> directiveDecls;
        public ReflectiveMethodBodyDecorator(ReflectiveMethodBaseDecorator methodDecorator)
        {
            this.methodDecorator = methodDecorator;
            localDecls = new ReadOnlyCollection<ILocalDeclaration>(methodDecorator.ExpressiveGenerator.Locals.TransformEnumerateOnly(local => (ILocalDeclaration)local));
            directiveDecls = new ReadOnlyCollection<IDirectiveDeclaration>(methodDecorator.ExpressiveGenerator.Directives.TransformEnumerateOnly(directive => (IDirectiveDeclaration)directive));
        }

        public virtual ReflectiveMethodBaseDecorator MethodDecorator { get { return methodDecorator; } }
        public virtual IMethodBaseGenerator Method { get { return MethodDecorator; } }
        public abstract ReflectiveILOperationDecorator ILOperationDecorator { get; }
        public virtual IILOperator ILOperator { get { return ILOperationDecorator; } }
        public virtual ReadOnlyCollection<ILocalGenerator> Locals { get { return methodDecorator.ExpressiveGenerator.Locals; } }
        public virtual ReadOnlyCollection<IDirectiveGenerator> Directives { get { return methodDecorator.ExpressiveGenerator.Directives; } }
        public virtual ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            return methodDecorator.ExpressiveGenerator.AddLocal(localGen);
        }

        IMethodBaseDeclaration IMethodBodyDeclaration.Method { get { return MethodDecorator; } }

        ReadOnlyCollection<ILocalDeclaration> IMethodBodyDeclaration.Locals
        {
            get { return localDecls; }
        }

        ReadOnlyCollection<IDirectiveDeclaration> IMethodBodyDeclaration.Directives
        {
            get { return directiveDecls; }
        }
    }
}
