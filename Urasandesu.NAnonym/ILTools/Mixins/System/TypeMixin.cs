using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Mixins.System
{
    public static partial class TypeMixin
    {
        public static MethodInfo GetMethod(this Type type, MethodInfo methodInfo)
        {
            return type.GetMethod(
                            methodInfo.Name,
                            methodInfo.ExportBinding(),
                            null,
                            methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray(),
                            null);
        }
    }
}
