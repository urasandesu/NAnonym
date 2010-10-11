using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    public class TargetMethodInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo OldMethod { get; set; }
        public MethodInfo NewMethod { get; set; }
        public Type DelegateType { get; set; }
        public NewMethodType NewMethodType { get; private set; }

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

            NewMethodType = NewMethodType.None;
            if (TypeAnalyzer.IsAnonymous(newMethod))
            {
                NewMethodType |= NewMethodType.Anonymous;
            }

            if (newMethod.IsStatic)
            {
                NewMethodType |= NewMethodType.Static;
            }
            else
            {
                NewMethodType |= NewMethodType.Instance;
            }
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

    [Flags]
    public enum NewMethodType
    {
        None = 0,
        Anonymous = 1,
        Instance = 2,
        Static = 4,
        AnonymousInstance = Anonymous | Instance,
        AnonymousStatic = Anonymous | Static,
    }
}
