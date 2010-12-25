/* 
 * File: ExpressiveGeneratorMacro.cs
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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools
{
    public class ExpressiveGeneratorMacro
    {
        ExpressiveGenerator gen;
        public ExpressiveGeneratorMacro(ExpressiveGenerator gen)
        {
            this.gen = gen;
        }

        public void EvalEmitDirectives(string ilGenName, ReadOnlyCollection<IDirectiveGenerator> directives)
        {
            foreach (var directive in directives)
            {
                var opcode = directive.OpCode.ToClr();
                var operand = directive.ClrOperand;
 
                if (operand == null && directive.RawOperand == null)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes))));
                else if (operand is byte)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((byte)operand)));
                else if (operand is ConstructorInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((ConstructorInfo)operand)));
                else if (operand is double)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((double)operand)));
                else if (operand is FieldInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((FieldInfo)operand)));
                else if (operand is float)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((float)operand)));
                else if (operand is int)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((int)operand)));
                else if (operand is long)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((long)operand)));
                else if (operand is MethodInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((MethodInfo)operand)));
                else if (operand is sbyte)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((sbyte)operand)));
                else if (operand is short)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((short)operand)));
                else if (operand is string)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((string)operand)));
                else if (operand is Type)
                    gen.Eval(_ => _.Ld<ILGenerator>(ilGenName).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((Type)operand)));
                else
                    throw new NotSupportedException();
            }
        }
    }
}

