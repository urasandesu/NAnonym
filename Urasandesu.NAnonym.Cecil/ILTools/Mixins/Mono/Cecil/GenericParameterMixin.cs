using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class GenericParameterMixin
    {
        public static GenericParameter Duplicate(this GenericParameter source, IGenericParameterProvider owner)
        {
            var destination = new GenericParameter(source.Name, owner);
            return destination;
        }
    }
}
