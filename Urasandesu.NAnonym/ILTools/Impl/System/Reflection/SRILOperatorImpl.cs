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

        SRMethodBodyGeneratorImpl methodBodyGen;

        public SRILOperatorImpl(ILGenerator ilGenerator, SRMethodBodyGeneratorImpl methodBodyGen)
        {
            this.ilGenerator = ilGenerator;
            directives = new List<SRDirectiveGeneratorImpl>();
            readonlyDirectives = new ReadOnlyCollection<IDirectiveGenerator>(directives.TransformEnumerateOnly(directiveGen => (IDirectiveGenerator)directiveGen));
            this.methodBodyGen = methodBodyGen;
        }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        public ILocalGenerator AddLocal(string name, Type localType)
        {
            var localBuilder = ilGenerator.DeclareLocal(localType);
            var localGen = new SRLocalGeneratorImpl(name, localBuilder);
            methodBodyGen.LocalGens.Add(localGen);
            return localGen;
        }

        public ILocalGenerator AddLocal(Type localType)
        {
            var localBuilder = ilGenerator.DeclareLocal(localType);
            var localGen = new SRLocalGeneratorImpl(localBuilder);
            methodBodyGen.LocalGens.Add(localGen);
            return localGen;
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
            ilGenerator.Emit(opcode.ToSre());
            directives.Add(new SRDirectiveGeneratorImpl(opcode));
        }

        public void Emit(OpCode opcode, byte arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            ilGenerator.Emit(opcode.ToSre(), con);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, con));
        }

        public void Emit(OpCode opcode, double arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            ilGenerator.Emit(opcode.ToSre(), field);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, field));
        }

        public void Emit(OpCode opcode, float arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, int arg)
        {
            ilGenerator.Emit(opcode.ToSre(), arg);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, arg));
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
            ilGenerator.Emit(opcode.ToSre(), ((SRLocalGeneratorImpl)local).LocalBuilder);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, local));
        }

        public void Emit(OpCode opcode, long arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            ilGenerator.Emit(opcode.ToSre(), meth);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, meth));
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            ilGenerator.Emit(opcode.ToSre(), arg);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, arg));
        }

        public void Emit(OpCode opcode, short arg)
        {
            throw new NotImplementedException();
        }

        public void Emit(OpCode opcode, string str)
        {
            ilGenerator.Emit(opcode.ToSre(), str);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, str));
        }

        public void Emit(OpCode opcode, Type cls)
        {
            ilGenerator.Emit(opcode.ToSre(), cls);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, cls));
        }

        public void Emit(OpCode opcode, IConstructorDeclaration constructorDecl)
        {
            ilGenerator.Emit(opcode.ToSre(), ((SRConstructorDeclarationImpl)constructorDecl).ConstructorInfo);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, constructorDecl));
        }

        public void Emit(OpCode opcode, IParameterDeclaration parameterDecl)
        {
            ilGenerator.Emit(opcode.ToSre(), ((SRParameterDeclarationImpl)parameterDecl).Position);
            directives.Add(new SRDirectiveGeneratorImpl(opcode, parameterDecl));
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
