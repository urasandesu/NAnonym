/* 
 * File: IILOperatorDecorator.g.cs
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
    public abstract class IILOperatorDecorator : IILOperator
    {
        protected readonly IILOperator source;
        public IILOperatorDecorator(IILOperator source)
        {
            this.source = source;
        }

        public virtual ILocalGenerator AddLocal(String name, Type localType) { return source.AddLocal(name, localType); }
        public virtual ILocalGenerator AddLocal(String name, ITypeDeclaration localType) { return source.AddLocal(name, localType); }
        public virtual ILocalGenerator AddLocal(Type localType) { return source.AddLocal(localType); }
        public virtual ILocalGenerator AddLocal(ITypeDeclaration localType) { return source.AddLocal(localType); }
        public virtual ILocalGenerator AddLocal(Type localType, Boolean pinned) { return source.AddLocal(localType, pinned); }
        public virtual ILabelGenerator AddLabel() { return source.AddLabel(); }
        public virtual void Emit(OpCode opcode) { source.Emit(opcode); }
        public virtual void Emit(OpCode opcode, Byte arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, ConstructorInfo con) { source.Emit(opcode, con); }
        public virtual void Emit(OpCode opcode, Double arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, FieldInfo field) { source.Emit(opcode, field); }
        public virtual void Emit(OpCode opcode, Single arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, Int32 arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, ILabelDeclaration label) { source.Emit(opcode, label); }
        public virtual void Emit(OpCode opcode, ILabelDeclaration[] labels) { source.Emit(opcode, labels); }
        public virtual void Emit(OpCode opcode, ILocalDeclaration local) { source.Emit(opcode, local); }
        public virtual void Emit(OpCode opcode, Int64 arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, MethodInfo meth) { source.Emit(opcode, meth); }
        public virtual void Emit(OpCode opcode, SByte arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, Int16 arg) { source.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, String str) { source.Emit(opcode, str); }
        public virtual void Emit(OpCode opcode, Type cls) { source.Emit(opcode, cls); }
        public virtual void Emit(OpCode opcode, ITypeDeclaration type) { source.Emit(opcode, type); }
        public virtual void Emit(OpCode opcode, IConstructorDeclaration constructorDecl) { source.Emit(opcode, constructorDecl); }
        public virtual void Emit(OpCode opcode, IMethodDeclaration methodDecl) { source.Emit(opcode, methodDecl); }
        public virtual void Emit(OpCode opcode, IParameterDeclaration parameterDecl) { source.Emit(opcode, parameterDecl); }
        public virtual void Emit(OpCode opcode, IFieldDeclaration fieldDecl) { source.Emit(opcode, fieldDecl); }
        public virtual void Emit(OpCode opcode, IPortableScopeItem scopeItem) { source.Emit(opcode, scopeItem); }
        public virtual void SetLabel(ILabelDeclaration loc) { source.SetLabel(loc); }
        public virtual Object Source 
		{
			get { return source.Source; }
		}
	}
}
