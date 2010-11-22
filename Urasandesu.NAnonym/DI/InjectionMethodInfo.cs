using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Linq.Expressions;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.DI
{
    public class InjectionMethodInfo
    {
        public SetupMode Mode { get; set; }
        public MethodInfo Source { get; set; }
        public MethodInfo Destination { get; set; }
        public Type DelegateType { get; set; }
        public MethodBodyInjectionBuilderType DestinationType { get; private set; }

        public InjectionMethodInfo()
        {
        }

        public InjectionMethodInfo(SetupMode mode, MethodInfo source, MethodInfo destination)
            : this(mode, source, destination, null)
        {
        }

        public InjectionMethodInfo(SetupMode mode, MethodInfo source, MethodInfo destination, Type delegateType)
        {
            Mode = mode;
            Source = source;
            Destination = destination;
            DelegateType = delegateType;

            DestinationType = MethodBodyInjectionBuilderType.None;
            if (TypeAnalyzer.IsAnonymous(destination))
            {
                DestinationType |= MethodBodyInjectionBuilderType.Anonymous;
            }

            if (destination.IsStatic)
            {
                DestinationType |= MethodBodyInjectionBuilderType.Static;
            }
            else
            {
                DestinationType |= MethodBodyInjectionBuilderType.Instance;
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
                DestinationType |= MethodBodyInjectionBuilderType.Base;
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
