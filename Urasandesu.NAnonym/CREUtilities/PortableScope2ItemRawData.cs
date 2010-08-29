using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.CREUtilities
{
    [Serializable]
    public struct PortableScope2RawData
    {
        public PortableScope2RawData(IMethodBaseDeclaration methodDecl)
            : this()
        {
            TypeAssemblyQualifiedName = methodDecl.DeclaringType.AssemblyQualifiedName;
            MethodName = methodDecl.Name;
            ParameterTypeFullNames = methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray();
        }

        public string TypeAssemblyQualifiedName { get; private set; }
        public string MethodName { get; private set; }
        public string[] ParameterTypeFullNames { get; private set; }

        public static bool operator ==(PortableScope2RawData x, PortableScope2RawData y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PortableScope2RawData x, PortableScope2RawData y)
        {
            return !(x == y);
        }

        public bool Equals(PortableScope2RawData that)
        {
            bool equals = TypeAssemblyQualifiedName == that.TypeAssemblyQualifiedName;
            equals = equals && MethodName == that.MethodName;
            equals = equals && ParameterTypeFullNames.Equivalent(that.ParameterTypeFullNames);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is PortableScope2RawData)) return false;

            var that = (PortableScope2RawData)obj;

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

    [Serializable]
    public struct PortableScope2ItemRawData
    {
        public PortableScope2ItemRawData(PortableScope2RawData rawData, string localName)
            : this()
        {
            RawData = rawData;
            LocalName = localName;
        }

        public PortableScope2ItemRawData(IMethodBaseDeclaration methodDecl, string localName)
        {
            throw new NotSupportedException();
        }

        public PortableScope2RawData RawData { get; private set; }
        public string LocalName { get; private set; }

        string fieldName;
        public string FieldName
        {
            get 
            {
                if (this.fieldName == null)
                {
                    var fieldName = new StringBuilder();
                    fieldName.Append(RawData.MethodName);
                    fieldName.Append(PortableScope2.NameDelimiter);
                    fieldName.Append(string.Join(
                        PortableScope2.NameDelimiter,
                        RawData.ParameterTypeFullNames.Select(
                            ParameterTypeFullName =>
                                ParameterTypeFullName.Replace(PortableScope2.MemberAccessOperator, PortableScope2.AltMemberAccessOperator)
                        ).ToArray()));
                    fieldName.Append(PortableScope2.NameDelimiter);
                    fieldName.Append(LocalName);
                    this.fieldName = fieldName.ToString();
                }

                return this.fieldName;
            }
        }

        public static bool operator ==(PortableScope2ItemRawData x, PortableScope2ItemRawData y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PortableScope2ItemRawData x, PortableScope2ItemRawData y)
        {
            return !(x == y);
        }

        public bool Equals(PortableScope2ItemRawData that)
        {
            bool equals = RawData == that.RawData;
            equals = equals && LocalName == that.LocalName;
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is PortableScope2ItemRawData)) return false;

            var that = (PortableScope2ItemRawData)obj;

            return Equals(that);
        }

        public override int GetHashCode()
        {
            // HACK: このクラスは Immutable なので HashCode はキャッシュ可能。
            int rawDataHashCode = RawData.GetHashCode();
            int localNameHashCode = LocalName.GetHashCodeOrDefault();

            return rawDataHashCode ^ localNameHashCode;
        }
    }
}
