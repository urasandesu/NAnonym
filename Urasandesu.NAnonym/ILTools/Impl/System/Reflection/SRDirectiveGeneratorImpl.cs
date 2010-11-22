using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRDirectiveGeneratorImpl : SRDirectiveDeclarationImpl, IDirectiveGenerator
    {
        public SRDirectiveGeneratorImpl(OpCode opcode) : base(opcode) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, byte arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ConstructorInfo con) : base(opcode, con) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, double arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, FieldInfo field) : base(opcode, field) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, float arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, int arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILabelDeclaration label) : base(opcode, label) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILabelDeclaration[] labels) : base(opcode, labels) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, ILocalDeclaration local) : base(opcode, local) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, long arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, MethodInfo meth) : base(opcode, meth) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, sbyte arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, short arg) : base(opcode, arg) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, string str) : base(opcode, str) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, Type cls) : base(opcode, cls) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IConstructorDeclaration constructorDecl) : base(opcode, constructorDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IMethodDeclaration methodDecl) : base(opcode, methodDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IParameterDeclaration parameterDecl) : base(opcode, parameterDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IFieldDeclaration fieldDecl) : base(opcode, fieldDecl) { }
        public SRDirectiveGeneratorImpl(OpCode opcode, IPortableScopeItem scopeItem) : base(opcode, scopeItem) { }
    }
}
