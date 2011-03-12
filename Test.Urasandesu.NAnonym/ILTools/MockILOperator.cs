/* 
 * File: MockILOperator.cs
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
using Urasandesu.NAnonym.ILTools;

namespace Test.Urasandesu.NAnonym.ILTools
{
    class MockILOperator : IILOperator
    {
        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        public ILocalGenerator AddLocal(string name, Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(string name, ITypeDeclaration localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public ILabelGenerator AddLabel()
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, double arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, int arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILabelDeclaration label)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILocalDeclaration local)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, string str)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, Type cls)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IMethodDeclaration methodDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IFieldDeclaration fieldDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IPortableScopeItem scopeItem)
        {
            throw new NotImplementedException();
        }

        public void SetLabel(ILabelDeclaration loc)
        {
            throw new NotImplementedException();
        }
    }
}
