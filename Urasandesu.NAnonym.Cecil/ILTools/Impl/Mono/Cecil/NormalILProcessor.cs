using Mono.Cecil;
using Mono.Cecil.Cil;
using MCC = Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class NormalILProcessor : IILProcessor
    {
        ILProcessor il;
        public NormalILProcessor(ILProcessor il)
        {
            this.il = il;
        }

        #region IILProcessor メンバ

        public MCC::MethodBody Body
        {
            get { return il.Body; }
        }

        public virtual void Append(Instruction instruction)
        {
            il.Append(instruction);
        }

        public Instruction Create(MCC::OpCode opcode)
        {
            return il.Create(opcode);
        }

        public Instruction Create(MCC::OpCode opcode, byte value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, CallSite site)
        {
            return il.Create(opcode, site);
        }

        public Instruction Create(MCC::OpCode opcode, double value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, FieldReference field)
        {
            return il.Create(opcode, field);
        }

        public Instruction Create(MCC::OpCode opcode, float value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, Instruction target)
        {
            return il.Create(opcode, target);
        }

        public Instruction Create(MCC::OpCode opcode, Instruction[] targets)
        {
            return il.Create(opcode, targets);
        }

        public Instruction Create(MCC::OpCode opcode, int value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, long value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, MethodReference method)
        {
            return il.Create(opcode, method);
        }

        public Instruction Create(MCC::OpCode opcode, ParameterDefinition parameter)
        {
            return il.Create(opcode, parameter);
        }

        public Instruction Create(MCC::OpCode opcode, sbyte value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, string value)
        {
            return il.Create(opcode, value);
        }

        public Instruction Create(MCC::OpCode opcode, TypeReference type)
        {
            return il.Create(opcode, type);
        }

        public Instruction Create(MCC::OpCode opcode, VariableDefinition variable)
        {
            return il.Create(opcode, variable);
        }

        public virtual void Emit(MCC::OpCode opcode)
        {
            il.Emit(opcode);
        }

        public virtual void Emit(MCC::OpCode opcode, byte value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, CallSite site)
        {
            il.Emit(opcode, site);
        }

        public virtual void Emit(MCC::OpCode opcode, double value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, FieldReference field)
        {
            il.Emit(opcode, field);
        }

        public virtual void Emit(MCC::OpCode opcode, float value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, Instruction target)
        {
            il.Emit(opcode, target);
        }

        public virtual void Emit(MCC::OpCode opcode, Instruction[] targets)
        {
            il.Emit(opcode, targets);
        }

        public virtual void Emit(MCC::OpCode opcode, int value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, long value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, MethodReference method)
        {
            il.Emit(opcode, method);
        }

        public virtual void Emit(MCC::OpCode opcode, ParameterDefinition parameter)
        {
            il.Emit(opcode, parameter);
        }

        public virtual void Emit(MCC::OpCode opcode, sbyte value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, string value)
        {
            il.Emit(opcode, value);
        }

        public virtual void Emit(MCC::OpCode opcode, TypeReference type)
        {
            il.Emit(opcode, type);
        }

        public virtual void Emit(MCC::OpCode opcode, VariableDefinition variable)
        {
            il.Emit(opcode, variable);
        }

        public void InsertAfter(Instruction target, Instruction instruction)
        {
            il.InsertAfter(target, instruction);
        }

        public void InsertBefore(Instruction target, Instruction instruction)
        {
            il.InsertBefore(target, instruction);
        }

        public void Remove(Instruction instruction)
        {
            il.Remove(instruction);
        }

        public void Replace(Instruction target, Instruction instruction)
        {
            il.Replace(target, instruction);
        }

        #endregion
    }
}
