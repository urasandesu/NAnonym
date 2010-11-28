/* 
 * File: InstructionMixin.cs
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
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil
{
    public static class InstructionMixin
    {
        public static Instruction Duplicate(this Instruction source)
        {
            byte? byteOperand = null;
            CallSite callSiteOperand = null;
            double? doubleOperand = null;
            FieldReference fieldReferenceOperand = null;
            float? floatOperand = null;
            Instruction instructionOperand = null;
            Instruction[] instructionArrayOperand = null;
            int? intOperand = null;
            long? longOperand = null;
            MethodReference methodReferenceOperand = null;
            ParameterDefinition parameterDefinitionOperand = null;
            sbyte? sbyteOperand = null;
            string stringOperand = null;
            TypeReference typeReferenceOperand = null;
            VariableDefinition variableDefinitionOperand = null;

            if ((byteOperand = source.Operand as byte?) != null) return Instruction.Create(source.OpCode, byteOperand.Value);
            else if ((callSiteOperand = source.Operand as CallSite) != null) return Instruction.Create(source.OpCode, callSiteOperand);
            else if ((doubleOperand = source.Operand as double?) != null) return Instruction.Create(source.OpCode, doubleOperand.Value);
            else if ((fieldReferenceOperand = source.Operand as FieldReference) != null) return Instruction.Create(source.OpCode, fieldReferenceOperand);
            else if ((floatOperand = source.Operand as float?) != null) return Instruction.Create(source.OpCode, floatOperand.Value);
            else if ((instructionOperand = source.Operand as Instruction) != null) return Instruction.Create(source.OpCode, instructionOperand);
            else if ((instructionArrayOperand = source.Operand as Instruction[]) != null) return Instruction.Create(source.OpCode, instructionArrayOperand);
            else if ((intOperand = source.Operand as int?) != null) return Instruction.Create(source.OpCode, intOperand.Value);
            else if ((longOperand = source.Operand as long?) != null) return Instruction.Create(source.OpCode, longOperand.Value);
            else if ((methodReferenceOperand = source.Operand as MethodReference) != null) return Instruction.Create(source.OpCode, methodReferenceOperand);
            else if ((parameterDefinitionOperand = source.Operand as ParameterDefinition) != null) return Instruction.Create(source.OpCode, parameterDefinitionOperand);
            else if ((sbyteOperand = source.Operand as sbyte?) != null) return Instruction.Create(source.OpCode, sbyteOperand.Value);
            else if ((stringOperand = source.Operand as string) != null) return Instruction.Create(source.OpCode, stringOperand);
            else if ((typeReferenceOperand = source.Operand as TypeReference) != null) return Instruction.Create(source.OpCode, typeReferenceOperand);
            else if ((variableDefinitionOperand = source.Operand as VariableDefinition) != null) return Instruction.Create(source.OpCode, variableDefinitionOperand);
            else return Instruction.Create(source.OpCode);
        }
    }
}

