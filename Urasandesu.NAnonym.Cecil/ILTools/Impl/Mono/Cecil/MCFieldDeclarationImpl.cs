using System;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCFieldDeclarationImpl : MCMemberDeclarationImpl, UNI::IFieldDeclaration
    {
        readonly FieldReference fieldRef;
        public MCFieldDeclarationImpl(FieldReference fieldRef)
            : base(fieldRef)
        {
            this.fieldRef = fieldRef;
        }

        internal FieldReference FieldRef { get { return fieldRef; } }

        public Type FieldType
        {
            get { return fieldRef.FieldType.ToType(); }
        }
    }
}
