using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using MCC = Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Impl;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCILOperatorImpl : UNI::IILOperator
    {
        readonly MethodDefinition methodDef;
        readonly MC::Cil.MethodBody bodyDef;
        readonly IILProcessor il;
        readonly ModuleDefinition moduleDef;

        //public static implicit operator MCILOperatorImpl(ILProcessor il)
        //{
        //    return new MCILOperatorImpl(il);
        //}

        public MCILOperatorImpl(ILProcessor il)
            : this(il, ILEmitMode.Normal, null)
        {
        }

        public MCILOperatorImpl(ILProcessor il, ILEmitMode mode, Instruction target)
        {
            switch (mode)
            {
                case ILEmitMode.Normal:
                    this.il = new NormalILProcessor(il);
                    break;
                case ILEmitMode.InsertBefore:
                    this.il = new InsertBeforeILProcessor(il, target);
                    break;
                case ILEmitMode.InsertAfter:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException();
            }
            bodyDef = il.Body;
            methodDef = bodyDef.Method;
            moduleDef = methodDef.Module;
        }

        public object Source
        {
            get { return methodDef; }
        }

        public UNI::ILocalGenerator AddLocal(string name, Type localType)
        {
            var variableDef = new VariableDefinition(name, moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(Type localType)
        {
            var variableDef = new VariableDefinition(moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UNI::ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public UNI::ILabelGenerator AddLabel()
        {
            var target = Instruction.Create(MCC::OpCodes.Nop);
            return new MCLabelGeneratorImpl(target);
        }

        public void Emit(UNI::OpCode opcode)
        {
            il.Emit(opcode.ToMcc());
        }

        public void Emit(UNI::OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(con));
        }

        public void Emit(UNI::OpCode opcode, double arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UNI::OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(field));
        }

        public void Emit(UNI::OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, int arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration label)
        {
            il.Emit(opcode.ToMcc(), ((MCLabelDeclarationImpl)label).Target);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, UNI::ILocalDeclaration local)
        {
            il.Emit(opcode.ToMcc(), ((MCLocalGeneratorImpl)local).VariableDef);
        }

        public void Emit(UNI::OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(meth));
        }

        public void Emit(UNI::OpCode opcode, sbyte arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UNI::OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, string str)
        {
            il.Emit(opcode.ToMcc(), str);
        }

        public void Emit(UNI::OpCode opcode, Type cls)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(cls));
        }

        public void Emit(UNI::OpCode opcode, UNI::IConstructorDeclaration constructorDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCConstructorDeclarationImpl)constructorDecl).ConstructorRef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCParameterGeneratorImpl)parameterDecl).ParameterDef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IFieldDeclaration fieldDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCFieldDeclarationImpl)fieldDecl).FieldRef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IPortableScopeItem scopeItem)
        {
            il.Emit(opcode.ToMcc(), ((MCPortableScopeItemImpl)scopeItem).FieldDef);
        }

        public void SetLabel(UNI::ILabelDeclaration loc)
        {
            il.Append(((MCLabelDeclarationImpl)loc).Target);
        }
    }

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

        public void Append(Instruction instruction)
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

    class InsertBeforeILProcessor : NormalILProcessor
    {
        Instruction target;
        public InsertBeforeILProcessor(ILProcessor il, Instruction target)
            : base(il)
        {
            this.target = target;
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
            InsertBefore(target, Create(opcode, target));
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
