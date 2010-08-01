using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    sealed class MCFieldGeneratorImpl : MCFieldDeclarationImpl, IFieldGenerator
    {
        readonly FieldDefinition fieldDef;
        public MCFieldGeneratorImpl(FieldDefinition fieldDef)
            : base(fieldDef)
        {
            this.fieldDef = fieldDef;
        }

        public static implicit operator MCFieldGeneratorImpl(FieldDefinition fieldDef)
        {
            return new MCFieldGeneratorImpl(fieldDef);
        }

        public static implicit operator FieldDefinition(MCFieldGeneratorImpl fieldDecl)
        {
            return fieldDecl.fieldDef;
        }
    }
}
