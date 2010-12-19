/* 
 * File: MCDirectiveDeclarationImpl.cs
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
using Urasandesu.NAnonym.ILTools;
using MCC = Mono.Cecil.Cil;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil;


namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCDirectiveDeclarationImpl : IDirectiveDeclaration
    {
        MCC::Instruction instruction;
        object nanonymOperand;
        object clrOperand;

        public MCDirectiveDeclarationImpl(MCC::Instruction instruction)
        {
            this.instruction = instruction;
            if (instruction.Operand == null)
            {
                nanonymOperand = null;
                clrOperand = null;
            }
            else if (instruction.Operand is byte)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is CallSite) throw new NotImplementedException();
            else if (instruction.Operand is double)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is FieldReference)
            {
                var fieldRef = (FieldReference)instruction.Operand;
                nanonymOperand = new MCFieldDeclarationImpl(fieldRef);
                clrOperand = fieldRef.ToFieldInfo();
            }
            else if (instruction.Operand is float)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is MCC::Instruction)
            {
                nanonymOperand = new MCDirectiveGeneratorImpl((MCC::Instruction)instruction.Operand);
                clrOperand = null;
            }
            else if (instruction.Operand is MCC::Instruction[])
            {
                nanonymOperand = ((MCC::Instruction[])instruction.Operand).Select(_instruction => new MCDirectiveGeneratorImpl(_instruction)).ToArray();
                clrOperand = null;
            }
            else if (instruction.Operand is int)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is long)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is MethodReference)
            {
                var methodRef = (MethodReference)instruction.Operand;
                if (methodRef.Name == ".ctor")
                {
                    nanonymOperand = new MCConstructorDeclarationImpl(methodRef);
                    clrOperand = methodRef.ToConstructorInfo();
                }
                else
                {
                    nanonymOperand = new MCMethodDeclarationImpl(methodRef);
                    clrOperand = methodRef.ToMethodInfo();
                }
            }
            else if (instruction.Operand is ParameterDefinition)
            {
                var parameterDef = (ParameterDefinition)instruction.Operand;
                nanonymOperand = new MCParameterGeneratorImpl(parameterDef);
                clrOperand = parameterDef.ToParameterInfo();
            }
            else if (instruction.Operand is sbyte)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is string)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is TypeReference)
            {
                var typeRef = (TypeReference)instruction.Operand;
                nanonymOperand = new MCTypeDeclarationImpl(typeRef);
                clrOperand = typeRef.ToType();
            }
            else if (instruction.Operand is MCC::VariableDefinition)
            {
                nanonymOperand = new MCLocalGeneratorImpl((MCC::VariableDefinition)instruction.Operand);
                clrOperand = null;
            }
            else throw new NotSupportedException();
        }

        public OpCode OpCode
        {
            get { return instruction.OpCode.ToNAnonym(); }
        }

        public object RawOperand
        {
            get { return instruction.Operand; }
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

