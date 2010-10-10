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
