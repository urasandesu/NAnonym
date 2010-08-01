using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCFieldDeclarationImpl : MCMemberDeclarationImpl, IFieldDeclaration
    {
        readonly FieldReference fieldRef;
        public MCFieldDeclarationImpl(FieldReference fieldRef)
            : base(fieldRef)
        {
            this.fieldRef = fieldRef;
        }

        public static explicit operator MCFieldDeclarationImpl(FieldReference fieldRef)
        {
            return new MCFieldDeclarationImpl(fieldRef);
        }

        public static explicit operator FieldReference(MCFieldDeclarationImpl fieldDecl)
        {
            return fieldDecl.fieldRef;
        }
    }
}
