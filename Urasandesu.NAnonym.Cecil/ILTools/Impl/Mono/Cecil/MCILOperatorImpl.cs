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
            return (MCLocalGeneratorImpl)variableDef;
        }

        public UN::ILTools.ILocalGenerator AddLocal(Type localType)
        {
            var variableDef = new VariableDefinition(moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return (MCLocalGeneratorImpl)variableDef;
        }

        public UN::ILTools.ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public UN::ILTools.ILabelGenerator AddLabel()
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opCdoe)
        {
            il.Emit(opCdoe.Cast());
        }

        public void Emit(UN::ILTools.OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode.Cast(), moduleDef.Import(con));
        }

        public void Emit(UN::ILTools.OpCode opcode, double arg)
        {
            il.Emit(opcode.Cast(), arg);
        }

        public void Emit(UN::ILTools.OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode.Cast(), moduleDef.Import(field));
        }

        public void Emit(UN::ILTools.OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, int arg)
        {
            il.Emit(opcode.Cast(), arg);
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
            il.Emit(opcode.Cast(), (VariableDefinition)(MCLocalGeneratorImpl)local);
        }

        public void Emit(UN::ILTools.OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode.Cast(), moduleDef.Import(meth));
        }

        public void Emit(UN::ILTools.OpCode opcode, sbyte arg)
        {
            il.Emit(opcode.Cast(), arg);
        }

        public void Emit(UN::ILTools.OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(UN::ILTools.OpCode opcode, string str)
        {
            il.Emit(opcode.Cast(), str);
        }

        public void Emit(UN::ILTools.OpCode opcode, Type cls)
        {
            il.Emit(opcode.Cast(), moduleDef.Import(cls));
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IConstructorDeclaration constructorDecl)
        {
            il.Emit(opcode.Cast(), (MethodReference)(MCConstructorDeclarationImpl)constructorDecl);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode.Cast(), (ParameterDefinition)(MCParameterGeneratorImpl)parameterDecl);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IFieldDeclaration fieldDecl)
        {
            il.Emit(opcode.Cast(), (FieldReference)(MCFieldDeclarationImpl)fieldDecl);
        }

        public void Emit(UN::ILTools.OpCode opcode, UN::ILTools.IPortableScopeItem scopeItem)
        {
            il.Emit(opcode.Cast(), (FieldReference)(MCPortableScopeItemImpl)scopeItem);
        }

        public void SetLabel(UN::ILTools.ILabelDeclaration loc)
        {
            throw new NotImplementedException();
        }
    }
}
