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
using Urasandesu.NAnonym.CREUtilities.Impl;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities
{
    sealed class MCILOperatorImpl : IILOperator
    {
        readonly MethodDefinition methodDef;
        readonly MC.Cil.MethodBody bodyDef;
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

        public ILocalGenerator AddLocal(string name, Type localType)
        {
            var variableDef = new VariableDefinition(name, moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return (MCLocalGeneratorImpl)variableDef;
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            var variableDef = new VariableDefinition(moduleDef.Import(localType));
            bodyDef.Variables.Add(variableDef);
            return (MCLocalGeneratorImpl)variableDef;
        }

        public ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public ILabelGenerator AddLabel()
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opCdoe)
        {
            il.Emit(opCdoe);
        }

        public void Emit(OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            il.Emit(opcode, moduleDef.Import(con));
        }

        public void Emit(OpCode opcode, double arg)
        {
            il.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            il.Emit(opcode, moduleDef.Import(field));
        }

        public void Emit(OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, int arg)
        {
            il.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, ILabelDeclaration label)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILocalGenerator local)
        {
            il.Emit(opcode, (VariableDefinition)(MCLocalGeneratorImpl)local);
        }

        public void Emit(OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            il.Emit(opcode, moduleDef.Import(meth));
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            il.Emit(opcode, arg);
        }

        public void Emit(OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, string str)
        {
            il.Emit(opcode, str);
        }

        public void Emit(OpCode opcode, Type cls)
        {
            il.Emit(opcode, moduleDef.Import(cls));
        }

        public void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            il.Emit(opcode, (MethodReference)(MCConstructorDeclarationImpl)constructorDecl);
        }

        public void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            il.Emit(opcode, (ParameterDefinition)(MCParameterGeneratorImpl)parameterDecl);
        }

        public void Emit(OpCode opcode, IFieldDeclaration fieldDecl)
        {
            il.Emit(opcode, (FieldReference)(MCFieldDeclarationImpl)fieldDecl);
        }

        public void SetLabel(ILabelDeclaration loc)
        {
            throw new NotImplementedException();
        }
    }
}
