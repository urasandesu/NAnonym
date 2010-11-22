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
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCILOperatorImpl : UNI::IILOperator
    {
        readonly MethodDefinition methodDef;
        readonly MC::Cil.MethodBody bodyDef;
        readonly IILProcessor il;
        readonly ModuleDefinition moduleDef;

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
            il.Emit(opcode.ToCecil());
        }

        public void Emit(UNI::OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(con));
        }

        public void Emit(UNI::OpCode opcode, double arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(field));
        }

        public void Emit(UNI::OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, int arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration label)
        {
            il.Emit(opcode.ToCecil(), ((MCLabelDeclarationImpl)label).Target);
        }

        public void Emit(UNI::OpCode opcode, UNI::ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, UNI::ILocalDeclaration local)
        {
            il.Emit(opcode.ToCecil(), ((MCLocalGeneratorImpl)local).VariableDef);
        }

        public void Emit(UNI::OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(meth));
        }

        public void Emit(UNI::OpCode opcode, sbyte arg)
        {
            il.Emit(opcode.ToCecil(), arg);
        }

        public void Emit(UNI::OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UNI::OpCode opcode, string str)
        {
            il.Emit(opcode.ToCecil(), str);
        }

        public void Emit(UNI::OpCode opcode, Type cls)
        {
            il.Emit(opcode.ToCecil(), moduleDef.Import(cls));
        }

        public void Emit(UNI::OpCode opcode, UNI::IConstructorDeclaration constructorDecl)
        {
            il.Emit(opcode.ToCecil(), ((MCConstructorDeclarationImpl)constructorDecl).ConstructorRef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IMethodDeclaration methodDecl)
        {
            il.Emit(opcode.ToCecil(), ((MCMethodDeclarationImpl)methodDecl).MethodDef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode.ToCecil(), ((MCParameterGeneratorImpl)parameterDecl).ParameterDef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IFieldDeclaration fieldDecl)
        {
            il.Emit(opcode.ToCecil(), ((MCFieldDeclarationImpl)fieldDecl).FieldRef);
        }

        public void Emit(UNI::OpCode opcode, UNI::IPortableScopeItem scopeItem)
        {
            il.Emit(opcode.ToCecil(), ((MCPortableScopeItemImpl)scopeItem).FieldDef);
        }

        public void SetLabel(UNI::ILabelDeclaration loc)
        {
            il.Append(((MCLabelDeclarationImpl)loc).Target);
        }
    }
}
