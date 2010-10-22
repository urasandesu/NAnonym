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

        public void EvalEmitDirectives(string addedILGeneratorName, ReadOnlyCollection<IDirectiveGenerator> directives)
        {
            foreach (var directive in directives)
            {
                if (directive.ClrOperand == null && directive.RawOperand == null)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr())));
                else if (directive.ClrOperand is byte)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((byte)directive.ClrOperand)));
                else if (directive.ClrOperand is ConstructorInfo)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((ConstructorInfo)directive.ClrOperand)));
                else if (directive.ClrOperand is double)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((double)directive.ClrOperand)));
                else if (directive.ClrOperand is FieldInfo)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((FieldInfo)directive.ClrOperand)));
                else if (directive.ClrOperand is float)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((float)directive.ClrOperand)));
                else if (directive.ClrOperand is int)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((int)directive.ClrOperand)));
                else if (directive.ClrOperand is long)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((long)directive.ClrOperand)));
                else if (directive.ClrOperand is MethodInfo)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((MethodInfo)directive.ClrOperand)));
                else if (directive.ClrOperand is sbyte)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((sbyte)directive.ClrOperand)));
                else if (directive.ClrOperand is short)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((short)directive.ClrOperand)));
                else if (directive.ClrOperand is string)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((string)directive.ClrOperand)));
                else if (directive.ClrOperand is Type)
                    gen.Eval(_ => _.Extract<ILGenerator>(addedILGeneratorName).Emit(_.Expand(directive.OpCode.ToClr(), typeof(SRE::OpCodes)), _.Expand((Type)directive.ClrOperand)));
                else
                    throw new NotSupportedException();
            }
        }
    }
}
