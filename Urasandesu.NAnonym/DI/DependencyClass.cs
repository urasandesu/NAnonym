using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyClass : MarshalByRefObject
    {
        DependencyClass modified;

        internal HashSet<TargetMethodInfo> TargetMethodInfoSet { get; private set; }

        public DependencyClass()
        {
            TargetMethodInfoSet = new HashSet<TargetMethodInfo>();
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
