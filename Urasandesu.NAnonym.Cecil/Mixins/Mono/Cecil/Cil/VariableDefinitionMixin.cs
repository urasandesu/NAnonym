using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil.Cil
{
    public static class VariableDefinitionMixin
    {
        public static VariableDefinition Duplicate(this VariableDefinition source)
        {
            var destination = new VariableDefinition(source.Name, source.VariableType);
            return destination;
        }
    }
}
