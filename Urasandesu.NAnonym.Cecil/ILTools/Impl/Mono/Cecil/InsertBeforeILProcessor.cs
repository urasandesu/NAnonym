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
