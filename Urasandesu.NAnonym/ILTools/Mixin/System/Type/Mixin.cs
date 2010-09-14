using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools
{
    public static partial class Mixin
    {
        public static TypeDefinition ToTypeDef(this Type type)
        {
            var assemblyDef = GlobalAssemblyResolver.Instance.Resolve(type.Assembly.FullName);
            return (TypeDefinition)assemblyDef.MainModule.LookupToken(type.MetadataToken);
        }

        public static GenericInstanceType ToTypeGen(this Type type)
        {
            var genericInstanceType = new GenericInstanceType(type.ToTypeDef());
            type.GetGenericArguments().Select(_ => _.ToTypeRef()).AddRangeTo(genericInstanceType.GenericArguments);
            return genericInstanceType;
        }

        public static ArrayType ToTypeArray(this Type type)
        {
            var arrayType = new ArrayType(type.GetElementType().ToTypeRef(), type.GetArrayRank());
            return arrayType;
        }

        public static TypeReference ToTypeRef(this Type type)
        {
            if (type.IsArray)
            {
                return type.ToTypeArray();
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                return type.ToTypeGen();
            }
            else
            {
                return type.ToTypeDef();
            }
        }
    }
}
