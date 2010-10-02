using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Mixins.System.Reflection
{
    public static class FieldInfoMixin
    {
        public static BindingFlags ExportBinding(this FieldInfo fieldInfo)
        {
            BindingFlags bindingAttr = BindingFlags.Default;

            if (fieldInfo.IsPublic)
            {
                bindingAttr |= BindingFlags.Public;
            }
            else
            {
                bindingAttr |= BindingFlags.NonPublic;
            }

            if (fieldInfo.IsStatic)
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
