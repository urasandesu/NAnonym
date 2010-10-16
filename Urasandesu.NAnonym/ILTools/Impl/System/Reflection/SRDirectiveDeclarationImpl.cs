using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRDirectiveDeclarationImpl : IDirectiveDeclaration
    {
        OpCode opcode;
        object operand;

        public SRDirectiveDeclarationImpl(OpCode opcode) { this.opcode = opcode; }
        public SRDirectiveDeclarationImpl(OpCode opcode, byte arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, ConstructorInfo con) { this.opcode = opcode; this.operand = con; }
        public SRDirectiveDeclarationImpl(OpCode opcode, double arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, FieldInfo field) { this.opcode = opcode; this.operand = field; }
        public SRDirectiveDeclarationImpl(OpCode opcode, float arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, int arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, ILabelDeclaration label) { this.opcode = opcode; this.operand = label; }
        public SRDirectiveDeclarationImpl(OpCode opcode, ILabelDeclaration[] labels) { this.opcode = opcode; this.operand = labels; }
        public SRDirectiveDeclarationImpl(OpCode opcode, ILocalDeclaration local) { this.opcode = opcode; this.operand = local; }
        public SRDirectiveDeclarationImpl(OpCode opcode, long arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, MethodInfo meth) { this.opcode = opcode; this.operand = meth; }
        public SRDirectiveDeclarationImpl(OpCode opcode, sbyte arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, short arg) { this.opcode = opcode; this.operand = arg; }
        public SRDirectiveDeclarationImpl(OpCode opcode, string str) { this.opcode = opcode; this.operand = str; }
        public SRDirectiveDeclarationImpl(OpCode opcode, Type cls) { this.opcode = opcode; this.operand = cls; }
        public SRDirectiveDeclarationImpl(OpCode opcode, IConstructorDeclaration constructorDecl) { this.opcode = opcode; this.operand = constructorDecl; }
        public SRDirectiveDeclarationImpl(OpCode opcode, IParameterDeclaration parameterDecl) { this.opcode = opcode; this.operand = parameterDecl; }
        public SRDirectiveDeclarationImpl(OpCode opcode, IFieldDeclaration fieldDecl) { this.opcode = opcode; this.operand = fieldDecl; }
        public SRDirectiveDeclarationImpl(OpCode opcode, IPortableScopeItem scopeItem) { this.opcode = opcode; this.operand = scopeItem; }

        #region IDirectiveDeclaration メンバ

        public OpCode OpCode
        {
            get { return opcode; }
        }

        public object Operand
        {
            get { return operand; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} {1}", OpCode, Operand);
        }
    }
}
