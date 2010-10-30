using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools
{
    public class ExpressiveMethodBodyGeneratorMacro
    {
        ExpressiveMethodBodyGenerator gen;
        public ExpressiveMethodBodyGeneratorMacro(ExpressiveMethodBodyGenerator gen)
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
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes))));
                else if (operand is byte)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((byte)operand)));
                else if (operand is ConstructorInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((ConstructorInfo)operand)));
                else if (operand is double)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((double)operand)));
                else if (operand is FieldInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((FieldInfo)operand)));
                else if (operand is float)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((float)operand)));
                else if (operand is int)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((int)operand)));
                else if (operand is long)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((long)operand)));
                else if (operand is MethodInfo)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((MethodInfo)operand)));
                else if (operand is sbyte)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((sbyte)operand)));
                else if (operand is short)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((short)operand)));
                else if (operand is string)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((string)operand)));
                else if (operand is Type)
                    gen.Eval(_ => _.Ld<ILGenerator>(_.X(ilGenName)).Emit(_.Cm(opcode, typeof(SRE::OpCodes)), _.X((Type)operand)));
                else
                    throw new NotSupportedException();
            }
        }
    }
}
