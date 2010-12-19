/* 
 * File: SRDirectiveDeclarationImpl.cs
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
    class SRDirectiveDeclarationImpl : IDirectiveDeclaration
    {
        OpCode opcode;
        object rawOperand;
        object nanonymOperand;
        object clrOperand;

        public SRDirectiveDeclarationImpl(OpCode opcode)
        {
            this.opcode = opcode;
            rawOperand = null;
            nanonymOperand = null;
            clrOperand = null;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, byte arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, ConstructorInfo con)
        {
            this.opcode = opcode; 
            rawOperand = con;
            nanonymOperand = new SRConstructorDeclarationImpl(con);
            clrOperand = con;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, double arg)
        {
            this.opcode = opcode; 
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, FieldInfo field)
        {
            this.opcode = opcode;
            rawOperand = field;
            nanonymOperand = new SRFieldDeclarationImpl(field);
            clrOperand = field;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, float arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, int arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, ILabelDeclaration label)
        {
            this.opcode = opcode;
            rawOperand = ((SRLabelGeneratorImpl)label).Label;
            nanonymOperand = label;
            clrOperand = ((SRLabelGeneratorImpl)label).Label;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, ILabelDeclaration[] labels)
        {
            this.opcode = opcode;
            rawOperand = labels.Select(label => ((SRLabelGeneratorImpl)label).Label).ToArray();
            nanonymOperand = labels;
            clrOperand = labels.Select(label => ((SRLabelGeneratorImpl)label).Label).ToArray();
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, ILocalDeclaration local)
        {
            this.opcode = opcode;
            rawOperand = ((SRLocalGeneratorImpl)local).LocalBuilder;
            nanonymOperand = local;
            clrOperand = ((SRLocalGeneratorImpl)local).LocalBuilder;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, long arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, MethodInfo meth)
        {
            this.opcode = opcode;
            rawOperand = meth;
            nanonymOperand = new SRMethodDeclarationImpl(meth);
            clrOperand = meth;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, sbyte arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, short arg)
        {
            this.opcode = opcode;
            rawOperand = arg;
            nanonymOperand = arg;
            clrOperand = arg;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, string str)
        {
            this.opcode = opcode;
            rawOperand = str;
            nanonymOperand = str;
            clrOperand = str;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, Type cls)
        {
            this.opcode = opcode;
            rawOperand = cls;
            nanonymOperand = cls;
            clrOperand = cls;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            this.opcode = opcode;
            rawOperand = ((SRConstructorDeclarationImpl)constructorDecl).ConstructorInfo;
            nanonymOperand = constructorDecl;
            clrOperand = ((SRConstructorDeclarationImpl)constructorDecl).ConstructorInfo;
        }

        public SRDirectiveDeclarationImpl(OpCode opcode, IMethodDeclaration methodDecl)
        {
            this.opcode = opcode;
            rawOperand = ((SRMethodDeclarationImpl)methodDecl).MethodInfo;
            nanonymOperand = methodDecl;
            clrOperand = ((SRMethodDeclarationImpl)methodDecl).MethodInfo;
        }
        
        public SRDirectiveDeclarationImpl(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            this.opcode = opcode;
            rawOperand = ((SRParameterGeneratorImpl)parameterDecl).ParameterBuilder;
            nanonymOperand = parameterDecl;
            clrOperand = ((SRParameterGeneratorImpl)parameterDecl).ParameterBuilder;
        }
        
        public SRDirectiveDeclarationImpl(OpCode opcode, IFieldDeclaration fieldDecl)
        {
            this.opcode = opcode;
            rawOperand = ((SRFieldGeneratorImpl)fieldDecl).FieldInfo;
            nanonymOperand = fieldDecl;
            clrOperand = ((SRFieldGeneratorImpl)fieldDecl).FieldInfo;
        }
        
        public SRDirectiveDeclarationImpl(OpCode opcode, IPortableScopeItem scopeItem)
        {
            throw new NotImplementedException();
        }

        public OpCode OpCode
        {
            get { return opcode; }
        }

        public object RawOperand
        {
            get { return rawOperand; }
        }

        public object NAnonymOperand
        {
            get { return nanonymOperand; }
        }

        public object ClrOperand
        {
            get { return clrOperand; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", OpCode, RawOperand);
        }
    }
}

