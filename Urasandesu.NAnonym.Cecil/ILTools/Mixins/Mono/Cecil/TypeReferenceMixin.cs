using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
//using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil
{
    public static class TypeReferenceMixin
    {
        public static bool Equivalent(this TypeReference x, Type y)
        {
            // HACK: 暫定。x が配列の場合（例えば Type[]）、配列ではない型に解決されてしまう。本来であれば、ArrayType で Resolve が override されているべきにも思うが・・・？
            switch (x.Scope.MetadataScopeType)
            {
                case MetadataScopeType.AssemblyNameReference:
                    var assemblyNameReference = x.Scope as AssemblyNameReference;
                    return assemblyNameReference.FullName == y.Assembly.FullName && x.GetFullName() == y.FullName;
                case MetadataScopeType.ModuleDefinition:
                    var moduleDefinition = x.Scope as ModuleDefinition;
                    return moduleDefinition.Assembly.Name.FullName == y.Assembly.FullName && x.GetFullName() == y.FullName;
                case MetadataScopeType.ModuleReference:
                default:
                    var resolvedX = x.Resolve();
                    return resolvedX.Module.Assembly.Name.FullName == y.Assembly.FullName && resolvedX.GetFullName() == y.FullName;
            }
        }

        public static bool Equivalent(this TypeReference x, TypeReference y)
        {
            return x.Module.Assembly.Name.FullName == y.Module.Assembly.Name.FullName && x.FullName == y.FullName;
        }

        public static IEnumerable<MethodDefinition> GetMethodDefs(this TypeReference typeRef)
        {
            var typeDef = default(TypeDefinition);
            var typeSpec = default(TypeSpecification);
            if ((typeDef = typeRef as TypeDefinition) != null)
            {
                return typeDef.Methods;
            }
            else if ((typeSpec = typeRef as TypeSpecification) != null)
            {
                return typeSpec.GetMethodDefs();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static Type ToType(this TypeReference source)
        {
            return Type.GetType(source.GetAssemblyQualifiedName());
        }

        public static string GetFullName(this TypeReference source)
        {
            if (source.IsGenericInstance)
            {
                var genericSource = (GenericInstanceType)source;
                return genericSource.FullName.Substring(0, genericSource.FullName.IndexOf('<')) + 
                    string.Format("[{0}]", string.Join(",", genericSource.GenericArguments.Select(
                        argType => string.Format("[{0}]", argType.GetAssemblyQualifiedName())).ToArray()));
            }
            else
            {
                return source.FullName.Replace("/", "+");
            }
        }

        public static string GetAssemblyQualifiedName(this TypeReference source)
        {
            var resolvedSource = source.Resolve();
            return resolvedSource.FullName + ", " + resolvedSource.Module.Assembly.Name.FullName;
        }
    }
}
