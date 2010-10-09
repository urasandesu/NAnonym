using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class TargetMethodInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo OldMethod { get; set; }
        public MethodInfo NewMethod { get; set; }
        public Type DelegateType { get; set; }

        public TargetMethodInfo()
        {
        }

        public TargetMethodInfo(SetupMode mode, MethodInfo oldMethod, MethodInfo newMethod)
            : this(mode, oldMethod, newMethod, null)
        {
        }

        public TargetMethodInfo(SetupMode mode, MethodInfo oldMethod, MethodInfo newMethod, Type delegateType)
        {
            Mode = mode;
            OldMethod = oldMethod;
            NewMethod = newMethod;
            DelegateType = delegateType;
        }

        public override bool Equals(object obj)
        {
            return this.EqualsNullable(obj, that => that.OldMethod);
        }

        public override int GetHashCode()
        {
            return OldMethod.GetHashCodeOrDefault();
        }
    }
}
