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
        public MethodInfo SourceMethodInfo { get; set; }
        public MethodInfo DestinationMethodInfo { get; set; }

        public TargetInfo()
        {
        }

        public TargetInfo(SetupMode mode, MethodInfo sourceMethodInfo, MethodInfo destinationMethodInfo)
        {
            Mode = mode;
            SourceMethodInfo = sourceMethodInfo;
            DestinationMethodInfo = destinationMethodInfo;
        }

        public override bool Equals(object obj)
        {
            return this.EqualsNullable(obj, that => that.SourceMethodInfo);
        }

        public override int GetHashCode()
        {
            return SourceMethodInfo.GetHashCodeOrDefault();
        }
    }
}
