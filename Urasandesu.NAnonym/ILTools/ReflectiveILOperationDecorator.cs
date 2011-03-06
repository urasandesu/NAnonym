/* 
 * File: ReflectiveILOperationDecorator.cs
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

namespace Urasandesu.NAnonym.ILTools
{
    public abstract class ReflectiveILOperationDecorator : IILOperator
    {
        protected readonly ReflectiveMethodBodyDecorator bodyDecorator;
        public ReflectiveILOperationDecorator(ReflectiveMethodBodyDecorator bodyDecorator)
        {
            this.bodyDecorator = bodyDecorator;
        }

        public ReflectiveMethodDesigner Gen { get { return bodyDecorator.MethodDecorator.ExpressiveGenerator; } }

        public virtual object Source { get { return Gen.ILOperator.Source; } }
        public virtual ILocalGenerator AddLocal(string name, Type localType) { return Gen.ILOperator.AddLocal(name, localType); }
        public virtual ILocalGenerator AddLocal(Type localType) { return Gen.ILOperator.AddLocal(localType); }
        public virtual ILocalGenerator AddLocal(Type localType, bool pinned) { return Gen.ILOperator.AddLocal(localType, pinned); }
        public virtual ILabelGenerator AddLabel() { return Gen.ILOperator.AddLabel(); }
        public virtual void Emit(OpCode opcode) { Gen.ILOperator.Emit(opcode); }
        public virtual void Emit(OpCode opcode, byte arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, ConstructorInfo con) { Gen.ILOperator.Emit(opcode, con); }
        public virtual void Emit(OpCode opcode, double arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, FieldInfo field) { Gen.ILOperator.Emit(opcode, field); }
        public virtual void Emit(OpCode opcode, float arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, int arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, ILabelDeclaration label) { Gen.ILOperator.Emit(opcode, label); }
        public virtual void Emit(OpCode opcode, ILabelDeclaration[] labels) { Gen.ILOperator.Emit(opcode, labels); }
        public virtual void Emit(OpCode opcode, ILocalDeclaration local) { Gen.ILOperator.Emit(opcode, local); }
        public virtual void Emit(OpCode opcode, long arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, MethodInfo meth) { Gen.ILOperator.Emit(opcode, meth); }
        public virtual void Emit(OpCode opcode, sbyte arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, short arg) { Gen.ILOperator.Emit(opcode, arg); }
        public virtual void Emit(OpCode opcode, string str) { Gen.ILOperator.Emit(opcode, str); }
        public virtual void Emit(OpCode opcode, Type cls) { Gen.ILOperator.Emit(opcode, cls); }
        public virtual void Emit(OpCode opcode, IConstructorDeclaration constructorDecl) { Gen.ILOperator.Emit(opcode, constructorDecl); }
        public virtual void Emit(OpCode opcode, IMethodDeclaration methodDecl) { Gen.ILOperator.Emit(opcode, methodDecl); }
        public virtual void Emit(OpCode opcode, IParameterDeclaration parameterDecl) { Gen.ILOperator.Emit(opcode, parameterDecl); }
        public virtual void Emit(OpCode opcode, IFieldDeclaration fieldDecl) { Gen.ILOperator.Emit(opcode, fieldDecl); }
        public virtual void Emit(OpCode opcode, IPortableScopeItem scopeItem) { Gen.ILOperator.Emit(opcode, scopeItem); }
        public virtual void SetLabel(ILabelDeclaration loc) { Gen.ILOperator.SetLabel(loc); }


        public ILocalGenerator AddLocal(string name, ITypeDeclaration localType)
        {
            throw new NotImplementedException();
        }
    }
}
