using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Mixins.System.Reflection
{
    public static class MethodInfoMixin
    {
        public static BindingFlags ExportBinding(this MethodInfo methodInfo)
        {
            BindingFlags bindingAttr = BindingFlags.Default;

            if (methodInfo.IsPublic)
            {
                bindingAttr |= BindingFlags.Public;
            }
            else
            {
                bindingAttr |= BindingFlags.NonPublic;
            }

            if (methodInfo.IsStatic)
            {
                bindingAttr |= BindingFlags.Static;
            }
            else
            {
                bindingAttr |= BindingFlags.Instance;
            }

            return bindingAttr;
        }
    }
}
