using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    public abstract class DependencyClass : MarshalByRefObject
    {
        DependencyClass modified;

        internal HashSet<WeaveFieldInfo> FieldSet { get; private set; }
        internal HashSet<WeaveMethodInfo> MethodSet { get; private set; }

        public DependencyClass()
        {
            FieldSet = new HashSet<WeaveFieldInfo>();
            MethodSet = new HashSet<WeaveMethodInfo>();
        }

        internal void Register()
        {
            modified = OnRegister();
            if (modified != null)
            {
                modified.Register();
            }
        }

        protected virtual DependencyClass OnRegister()
        {
            return null;
        }

        internal void Load()
        {
            OnLoad(modified);
            if (modified != null)
            {
                modified.Load();
            }
        }

        protected virtual void OnLoad(DependencyClass modified)
        {
        }
    }
}
