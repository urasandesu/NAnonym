using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using SR = System.Reflection;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    sealed class MCTypeGeneratorImpl : MCTypeDeclarationImpl, ITypeGenerator
    {
        readonly TypeDefinition typeDef;
        readonly ModuleDefinition moduleDef;
        public MCTypeGeneratorImpl(TypeDefinition typeDef)
            : base(typeDef)
        {
            this.typeDef = typeDef;
            moduleDef = typeDef.Module;
        }

        public static implicit operator MCTypeGeneratorImpl(TypeDefinition typeDef)
        {
            return new MCTypeGeneratorImpl(typeDef);
        }

        public static implicit operator TypeDefinition(MCTypeGeneratorImpl typeGen)
        {
            return typeGen.typeDef;
        }

        public IFieldDeclaration AddField(string fieldName, Type type, SR.FieldAttributes attributes)
        {
            var fieldDef = new FieldDefinition(fieldName, (MC.FieldAttributes)attributes, moduleDef.Import(type));
            typeDef.Fields.Add(fieldDef);
            return (MCFieldGeneratorImpl)fieldDef;
        }
    }
}
