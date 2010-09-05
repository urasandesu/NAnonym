using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using SR = System.Reflection;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCTypeGeneratorImpl : MCTypeDeclarationImpl, ITypeGenerator
    {
        //[NonSerialized]
        //bool deserialized;

        public MCTypeGeneratorImpl(TypeDefinition typeDef)
            : base(typeDef)
        {
        }

        public static implicit operator MCTypeGeneratorImpl(TypeDefinition typeDef)
        {
            return new MCTypeGeneratorImpl(typeDef);
        }

        public static implicit operator TypeDefinition(MCTypeGeneratorImpl typeGen)
        {
            return typeGen.TypeDef;
        }

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldDef = new FieldDefinition(fieldName, (MC::FieldAttributes)attributes, TypeDef.Module.Import(type));
            TypeDef.Fields.Add(fieldDef);
            return (MCFieldGeneratorImpl)fieldDef;
        }

        //[OnDeserialized]
        //internal new void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        base.OnDeserialized(context);
        //    }
        //}

        //public override void OnDeserialization(object sender)
        //{
        //    base.OnDeserialization(sender);
        //}


    }
}
