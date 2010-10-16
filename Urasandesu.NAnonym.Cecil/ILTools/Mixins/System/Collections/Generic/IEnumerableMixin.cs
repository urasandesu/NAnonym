using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.System.Collections.Generic
{
    public static class IEnumerableMixin
    {
        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<Type> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(Type),
                (firstItem, secondItem) =>
                {
                    return firstItem.ParameterType.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterInfo> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterInfo),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterDefinition> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterDefinition),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }
    }
}
