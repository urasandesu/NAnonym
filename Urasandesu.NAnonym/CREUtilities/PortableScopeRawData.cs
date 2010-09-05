using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.CREUtilities;

namespace Urasandesu.NAnonym.CREUtilities
{
    [Serializable]
    public sealed class PortableScopeRawData
    {
        public PortableScopeRawData(string typeAssemblyQualifiedName, string methodName, string[] parameterTypeFullNames)
        {
            TypeAssemblyQualifiedName = typeAssemblyQualifiedName;
            MethodName = methodName;
            ParameterTypeFullNames = parameterTypeFullNames;
        }

        public PortableScopeRawData(IMethodBaseDeclaration methodDecl)
            : this(methodDecl.DeclaringType.AssemblyQualifiedName,
                   methodDecl.Name,
                   methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray())
        {
        }

        public string TypeAssemblyQualifiedName { get; private set; }
        public string MethodName { get; private set; }
        public string[] ParameterTypeFullNames { get; private set; }

        public static bool operator ==(PortableScopeRawData x, PortableScopeRawData y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PortableScopeRawData x, PortableScopeRawData y)
        {
            return !(x == y);
        }

        public bool Equals(PortableScopeRawData that)
        {
            bool equals = TypeAssemblyQualifiedName == that.TypeAssemblyQualifiedName;
            equals = equals && MethodName == that.MethodName;
            equals = equals && ParameterTypeFullNames.Equivalent(that.ParameterTypeFullNames);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is PortableScopeRawData)) return false;

            var that = (PortableScopeRawData)obj;

            return Equals(that);
        }

        public override int GetHashCode()
        {
            int typeFullNameHashCode = TypeAssemblyQualifiedName.GetHashCodeOrDefault();
            int methodNameHashCode = MethodName.GetHashCodeOrDefault();
            int parameterTypeFullNamesHashCode = ParameterTypeFullNames.GetAggregatedHashCodeOrDefault();

            return typeFullNameHashCode ^ methodNameHashCode ^ parameterTypeFullNamesHashCode;
        }
    }
}
