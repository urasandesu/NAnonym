using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Impl;
//using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCILOperatorImpl : UN::ILTools.IILOperator
    {
        readonly MethodDefinition methodDef;
        readonly MC::Cil.MethodBody bodyDef;
        readonly ILProcessor il;
        readonly ModuleDefinition moduleDef;

        public static implicit operator MCILOperatorImpl(ILProcessor il)
        {
            return new MCILOperatorImpl(il);
        }

        public MCILOperatorImpl(ILProcessor il)
        {
            this.il = il;
            bodyDef = il.Body;
            methodDef = bodyDef.Method;
            moduleDef = methodDef.Module;
        }

        public object Source
        {
            get { return methodDef; }
        }

        public UN::ILTools.ILocalGenerator AddLocal(string name, Type localType)
        {
            var variableDef = new VariableDefinition(name, moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UN::ILTools.ILocalGenerator AddLocal(Type localType)
        {
            var variableDef = new VariableDefinition(moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return new MCLocalGeneratorImpl(variableDef);
        }

        public UN::ILTools.ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public UN::ILTools.ILabelGenerator AddLabel()
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode)
        {
            il.Emit(opcode.ToMcc());
        }

        public void Emit(UN::ILTools.OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(con));
        }

        public void Emit(UN::ILTools.OpCode opcode, double arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UN::ILTools.OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(field));
        }

        public void Emit(UN::ILTools.OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, int arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.ILabelDeclaration label)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.ILocalDeclaration local)
        {
            il.Emit(opcode.ToMcc(), ((MCLocalGeneratorImpl)local).VariableDef);
        }

        public void Emit(UN::ILTools.OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(meth));
        }

        public void Emit(UN::ILTools.OpCode opcode, sbyte arg)
        {
            il.Emit(opcode.ToMcc(), arg);
        }

        public void Emit(UN::ILTools.OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, string str)
        {
            il.Emit(opcode.ToMcc(), str);
        }

        public void Emit(UN::ILTools.OpCode opcode, Type cls)
        {
            il.Emit(opcode.ToMcc(), moduleDef.Import(cls));
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IConstructorDeclaration constructorDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCConstructorDeclarationImpl)constructorDecl).ConstructorRef);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCParameterGeneratorImpl)parameterDecl).ParameterDef);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IFieldDeclaration fieldDecl)
        {
            il.Emit(opcode.ToMcc(), ((MCFieldDeclarationImpl)fieldDecl).FieldRef);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IPortableScopeItem scopeItem)
        {
            il.Emit(opcode.ToMcc(), ((MCPortableScopeItemImpl)scopeItem).FieldDef);
        }

        public void SetLabel(UN::ILTools.ILabelDeclaration loc)
        {
            throw new NotImplementedException();
        }
    }
}
