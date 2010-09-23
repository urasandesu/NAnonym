using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym
{
    public static partial class Mixin
    {
        public static Type[] ParameterTypes(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public static string[] ParameterNames(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Select(parameter => parameter.Name).ToArray();
        }
    }
}
