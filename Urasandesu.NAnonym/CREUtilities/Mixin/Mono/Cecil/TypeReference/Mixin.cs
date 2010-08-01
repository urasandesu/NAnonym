using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities
{
    public static partial class Mixin
    {

        public static bool Equivalent(this TypeReference x, Type y)
        {
            // HACK: 暫定。x が配列の場合（例えば Type[]）、配列ではない型に解決されてしまう。本来であれば、ArrayType で Resolve が override されているべきにも思うが・・・？
            switch (x.Scope.MetadataScopeType)
            {
                case MetadataScopeType.AssemblyNameReference:
                    var assemblyNameReference = x.Scope as AssemblyNameReference;
                    return assemblyNameReference.FullName == y.Assembly.FullName && x.FullName == y.FullName;
                case MetadataScopeType.ModuleDefinition:
                    var moduleDefinition = x.Scope as ModuleDefinition;
                    return moduleDefinition.Assembly.Name.FullName == y.Assembly.FullName && x.FullName == y.FullName;
                case MetadataScopeType.ModuleReference:
                default:
                    var resolvedX = x.Resolve();
                    return resolvedX.Module.Assembly.Name.FullName == y.Assembly.FullName && resolvedX.FullName == y.FullName;
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
    }
}
