using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MC = Mono.Cecil;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.Mixins.System.Reflection
{
    public static class MethodAttributesMixin
    {
        public static MC::MethodAttributes ToMethodAttributes(this SR::MethodAttributes attribute)
        {
            return (MC::MethodAttributes)(int)attribute;
        }
    }
}
