using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    sealed class SRILOperatorImpl : IILOperator
    {
        readonly ILGenerator ilGenerator;

        List<SRDirectiveGeneratorImpl> directives;
        ReadOnlyCollection<IDirectiveGenerator> readonlyDirectives;

        public SRILOperatorImpl(ILGenerator ilGenerator)
        {
            this.ilGenerator = ilGenerator;
            directives = new List<SRDirectiveGeneratorImpl>();
            readonlyDirectives = new ReadOnlyCollection<IDirectiveGenerator>(directives.TransformEnumerateOnly(directiveGen => (IDirectiveGenerator)directiveGen));
        }

        public static implicit operator SRILOperatorImpl(ILGenerator ilGenerator)
        {
            return new SRILOperatorImpl(ilGenerator);
        }

        public static implicit operator ILGenerator(SRILOperatorImpl il)
        {
            return il.ilGenerator;
        }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        public ILocalGenerator AddLocal(string name, Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            throw new NotImplementedException();
        }

        public ILocalGenerator AddLocal(Type localType, bool pinned)
        {
            throw new NotImplementedException();
        }

        public ILabelGenerator AddLabel()
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode)
        {
            ilGenerator.Emit(opcode.Cast());
            directives.Add(new SRDirectiveGeneratorImpl(opcode));
        }

        // TODO: OpCode 以外は explicit にしたほうが良さそう。

        public void Emit(OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, double arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, int arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILabelDeclaration label)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILabelDeclaration[] labels)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ILocalDeclaration local)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            ilGenerator.Emit(opcode.Cast(), meth);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, meth));
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, string str)
        {
            ilGenerator.Emit(opcode.Cast(), str);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, str));
        }

        public void Emit(OpCode opcode, Type cls)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            ilGenerator.Emit(opcode.Cast(), (ConstructorInfo)(SRConstructorDeclarationImpl)constructorDecl);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, constructorDecl));
        }

        public void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IFieldDeclaration fieldDecl)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, IPortableScopeItem scopeItem)
        {
            throw new NotImplementedException();
        }

        public void SetLabel(ILabelDeclaration loc)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return readonlyDirectives; }
        }
    }
}
