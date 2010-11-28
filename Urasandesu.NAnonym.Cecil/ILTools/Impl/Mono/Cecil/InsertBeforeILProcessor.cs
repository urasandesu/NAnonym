/* 
 * File: InsertBeforeILProcessor.cs
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
 

using Mono.Cecil;
using Mono.Cecil.Cil;
using MCC = Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class InsertBeforeILProcessor : NormalILProcessor
    {
        Instruction target;
        public InsertBeforeILProcessor(ILProcessor il, Instruction target)
            : base(il)
        {
            this.target = target;
        }

        public override void Append(Instruction instruction)
        {
            InsertBefore(target, instruction);
        }

        public override void Emit(MCC::OpCode opcode)
        {
            InsertBefore(target, Create(opcode));
        }

        public override void Emit(MCC::OpCode opcode, byte value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, CallSite site)
        {
            InsertBefore(target, Create(opcode, site));
        }

        public override void Emit(MCC::OpCode opcode, double value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, FieldReference field)
        {
            InsertBefore(target, Create(opcode, field));
        }

        public override void Emit(MCC::OpCode opcode, float value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, Instruction target)
        {
            InsertBefore(this.target, Create(opcode, target));
        }

        public override void Emit(MCC::OpCode opcode, Instruction[] targets)
        {
            InsertBefore(target, Create(opcode, targets));
        }

        public override void Emit(MCC::OpCode opcode, int value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, long value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, MethodReference method)
        {
            InsertBefore(target, Create(opcode, method));
        }

        public override void Emit(MCC::OpCode opcode, ParameterDefinition parameter)
        {
            InsertBefore(target, Create(opcode, parameter));
        }

        public override void Emit(MCC::OpCode opcode, sbyte value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, string value)
        {
            InsertBefore(target, Create(opcode, value));
        }

        public override void Emit(MCC::OpCode opcode, TypeReference type)
        {
            InsertBefore(target, Create(opcode, type));
        }

        public override void Emit(MCC::OpCode opcode, VariableDefinition variable)
        {
            InsertBefore(target, Create(opcode, variable));
        }
    }
}

