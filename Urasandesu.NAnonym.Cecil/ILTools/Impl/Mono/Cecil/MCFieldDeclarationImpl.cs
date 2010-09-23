using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCFieldDeclarationImpl : MCMemberDeclarationImpl, UN::ILTools.IFieldDeclaration
    {
        readonly FieldReference fieldRef;
        public MCFieldDeclarationImpl(FieldReference fieldRef)
            : base(fieldRef)
        {
            this.fieldRef = fieldRef;
        }

        internal FieldReference FieldRef { get { return fieldRef; } }
    }
}
