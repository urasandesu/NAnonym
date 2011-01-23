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
        ReflectiveMethodDesigner gen;
        public ExpressiveGeneratorMacro(ReflectiveMethodDesigner gen)
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
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes))));
                else if (operand is byte)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((byte)operand)));
                else if (operand is ConstructorInfo)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((ConstructorInfo)operand)));
                else if (operand is double)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((double)operand)));
                else if (operand is FieldInfo)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((FieldInfo)operand)));
                else if (operand is float)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((float)operand)));
                else if (operand is int)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((int)operand)));
                else if (operand is long)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((long)operand)));
                else if (operand is MethodInfo)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((MethodInfo)operand)));
                else if (operand is sbyte)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((sbyte)operand)));
                else if (operand is short)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((short)operand)));
                else if (operand is string)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((string)operand)));
                else if (operand is Type)
                    gen.Eval(() => Dsl.Load<ILGenerator>(ilGenName).Emit(Dsl.ConstMember(opcode, typeof(SRE::OpCodes)), Dsl.Extract((Type)operand)));
                else
                    throw new NotSupportedException();
            }
        }
    }
}

