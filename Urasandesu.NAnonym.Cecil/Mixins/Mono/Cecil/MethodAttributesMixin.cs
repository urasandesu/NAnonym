using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MC = Mono.Cecil;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class MethodAttributesMixin
    {
        public static bool Equivalent(this MC::MethodAttributes x, SR::MethodAttributes y)
        {
            return (int)x == (int)y;
        }
    }
}
