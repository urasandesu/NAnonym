using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class GenericInstanceMethodMixin
    {
        public static GenericInstanceMethod DuplicateWithoutBody(this GenericInstanceMethod source)
        {
            throw new NotImplementedException();
        }

        public static GenericInstanceMethod Duplicate(this GenericInstanceMethod source)
        {
            var destination = new GenericInstanceMethod(source.ElementMethod.Duplicate());
            //source.genericpa
            throw new NotImplementedException();
        }
    }
}
