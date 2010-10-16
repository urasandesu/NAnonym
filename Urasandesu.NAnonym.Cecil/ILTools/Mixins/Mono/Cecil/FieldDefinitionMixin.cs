using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class FieldDefinitionMixin
    {
        public static bool Equivalent(this FieldDefinition x, FieldInfo y)
        {
            return x.Name == y.Name && x.FieldType.Equivalent(y.FieldType);
        }

        public static FieldDefinition Duplicate(this FieldDefinition source)
        {
            var destination = new FieldDefinition(source.Name, source.Attributes, source.FieldType);
            return destination;
        }
    }
}
