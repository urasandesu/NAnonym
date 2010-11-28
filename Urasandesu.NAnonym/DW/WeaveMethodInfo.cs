using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Linq.Expressions;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.DW
{
    public class WeaveMethodInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo Source { get; set; }
        public MethodInfo Destination { get; set; }
        public Type DelegateType { get; set; }
        public MethodBodyWeaveBuilderType DestinationType { get; private set; }

        public WeaveMethodInfo()
        {
        }

        public WeaveMethodInfo(SetupMode mode, MethodInfo source, MethodInfo destination)
            : this(mode, source, destination, null)
        {
        }

        public WeaveMethodInfo(SetupMode mode, MethodInfo source, MethodInfo destination, Type delegateType)
        {
            Mode = mode;
            Source = source;
            Destination = destination;
            DelegateType = delegateType;

            DestinationType = MethodBodyWeaveBuilderType.None;
            if (TypeAnalyzer.IsAnonymous(destination))
            {
                DestinationType |= MethodBodyWeaveBuilderType.Anonymous;
            }

            if (destination.IsStatic)
            {
                DestinationType |= MethodBodyWeaveBuilderType.Static;
            }
            else
            {
                DestinationType |= MethodBodyWeaveBuilderType.Instance;
            }

            if (delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithBase<>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithBase<,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithBase<,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithBase<,,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(FuncWithBase<,,,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithBase)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithBase<>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithBase<,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithBase<,,>)) ||
                delegateType.EquivalentWithoutGenericArguments(typeof(ActionWithBase<,,,>)))
            {
                DestinationType |= MethodBodyWeaveBuilderType.Base;
            }
        }

        public override bool Equals(object obj)
        {
            return this.EqualsNullable(obj, that => that.Source);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCodeOrDefault();
        }
    }
}
