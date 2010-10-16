using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class ParameterDefinitionMixin
    {
        public static bool Equivalent(this ParameterDefinition x, ParameterInfo y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static bool Equivalent(this ParameterDefinition x, ParameterDefinition y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static ParameterDefinition Duplicate(this ParameterDefinition source)
        {
            var destination = new ParameterDefinition(source.Name, source.Attributes, source.ParameterType);
            return destination;
        }

        public static ParameterInfo ToParameterInfo(this ParameterDefinition source)
        {
            throw new NotImplementedException();
        }
    }
}
