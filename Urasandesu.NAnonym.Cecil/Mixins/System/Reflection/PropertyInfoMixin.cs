using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.Mixins.System.Reflection
{
    public static class PropertyInfoMixin
    {
        public static bool IsStatic(this PropertyInfo source)
        {
            return (source.CanRead && source.GetGetMethod().IsStatic) || (source.CanWrite && source.GetSetMethod().IsStatic);
        }
    }
}
