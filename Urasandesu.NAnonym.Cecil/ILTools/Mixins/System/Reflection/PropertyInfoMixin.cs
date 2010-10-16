using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.System.Reflection
{
    public static class PropertyInfoMixin
    {
        public static bool IsStatic(this PropertyInfo source)
        {
            return (source.CanRead && source.GetGetMethod().IsStatic) || (source.CanWrite && source.GetSetMethod().IsStatic);
        }
    }
}
