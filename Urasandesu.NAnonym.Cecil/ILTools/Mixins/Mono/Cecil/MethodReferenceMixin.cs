using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.System.Collections.Generic;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class MethodReferenceMixin
    {
        public static bool Equivalent(this MethodReference x, MethodBase y)
        {
            bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
            equals = equals && x.Name == y.Name;
            equals = equals && x.Parameters.Equivalent(y.GetParameters());
            return equals;
        }

        public static MethodReference DuplicateWithoutBody(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.DuplicateWithoutBody();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.DuplicateWithoutBody();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static MethodReference Duplicate(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.Duplicate();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.Duplicate();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static MethodInfo ToMethodInfo(this MethodReference source)
        {
            return source.Resolve().ToMethodInfo();
        }

        public static ConstructorInfo ToConstructorInfo(this MethodReference methodRef)
        {
            return methodRef.Resolve().ToConstructorInfo();
        }
    }
}
