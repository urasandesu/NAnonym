using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class TargetInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo OldMethod { get; set; }
        public MethodInfo NewMethod { get; set; }

        public TargetInfo()
        {
        }

        public TargetInfo(SetupMode mode, MethodInfo oldMethod, MethodInfo newMethod)
        {
            Mode = mode;
            OldMethod = oldMethod;
            NewMethod = newMethod;
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
