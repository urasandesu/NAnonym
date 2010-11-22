using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyClass : MarshalByRefObject
    {
        DependencyClass modified;

        internal HashSet<InjectionFieldInfo> FieldSet { get; private set; }
        internal HashSet<InjectionMethodInfo> MethodSet { get; private set; }

        public DependencyClass()
        {
            FieldSet = new HashSet<InjectionFieldInfo>();
            MethodSet = new HashSet<InjectionMethodInfo>();
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
