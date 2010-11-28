/* 
 * File: SRDirectiveGeneratorImpl.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRDirectiveGeneratorImpl : SRDirectiveDeclarationImpl, IDirectiveGenerator
    {
        public SRDirectiveGeneratorImpl(OpCode opcode) : base(opcode) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, byte arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ConstructorInfo con) : base(opcode, con) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, double arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, FieldInfo field) : base(opcode, field) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, float arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, int arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILabelDeclaration label) : base(opcode, label) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILabelDeclaration[] labels) : base(opcode, labels) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILocalDeclaration local) : base(opcode, local) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, long arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, MethodInfo meth) : base(opcode, meth) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, sbyte arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, short arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, string str) : base(opcode, str) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, Type cls) : base(opcode, cls) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IConstructorDeclaration constructorDecl) : base(opcode, constructorDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IMethodDeclaration methodDecl) : base(opcode, methodDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IParameterDeclaration parameterDecl) : base(opcode, parameterDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IFieldDeclaration fieldDecl) : base(opcode, fieldDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IPortableScopeItem scopeItem) : base(opcode, scopeItem) { }
    }
}

