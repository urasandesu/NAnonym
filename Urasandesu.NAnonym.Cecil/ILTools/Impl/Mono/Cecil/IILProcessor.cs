/* 
 * File: IILProcessor.cs
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
    interface IILProcessor
    {
        MCC::MethodBody Body { get; }

        void Append(Instruction instruction);
        Instruction Create(MCC::OpCode opcode);
        Instruction Create(MCC::OpCode opcode, byte value);
        Instruction Create(MCC::OpCode opcode, CallSite site);
        Instruction Create(MCC::OpCode opcode, double value);
        Instruction Create(MCC::OpCode opcode, FieldReference field);
        Instruction Create(MCC::OpCode opcode, float value);
        Instruction Create(MCC::OpCode opcode, Instruction target);
        Instruction Create(MCC::OpCode opcode, Instruction[] targets);
        Instruction Create(MCC::OpCode opcode, int value);
        Instruction Create(MCC::OpCode opcode, long value);
        Instruction Create(MCC::OpCode opcode, MethodReference method);
        Instruction Create(MCC::OpCode opcode, ParameterDefinition parameter);
        Instruction Create(MCC::OpCode opcode, sbyte value);
        Instruction Create(MCC::OpCode opcode, string value);
        Instruction Create(MCC::OpCode opcode, TypeReference type);
        Instruction Create(MCC::OpCode opcode, VariableDefinition variable);
        void Emit(MCC::OpCode opcode);
        void Emit(MCC::OpCode opcode, byte value);
        void Emit(MCC::OpCode opcode, CallSite site);
        void Emit(MCC::OpCode opcode, double value);
        void Emit(MCC::OpCode opcode, FieldReference field);
        void Emit(MCC::OpCode opcode, float value);
        void Emit(MCC::OpCode opcode, Instruction target);
        void Emit(MCC::OpCode opcode, Instruction[] targets);
        void Emit(MCC::OpCode opcode, int value);
        void Emit(MCC::OpCode opcode, long value);
        void Emit(MCC::OpCode opcode, MethodReference method);
        void Emit(MCC::OpCode opcode, ParameterDefinition parameter);
        void Emit(MCC::OpCode opcode, sbyte value);
        void Emit(MCC::OpCode opcode, string value);
        void Emit(MCC::OpCode opcode, TypeReference type);
        void Emit(MCC::OpCode opcode, VariableDefinition variable);
        void InsertAfter(Instruction target, Instruction instruction);
        void InsertBefore(Instruction target, Instruction instruction);
        void Remove(Instruction instruction);
        void Replace(Instruction target, Instruction instruction);
    }
}

