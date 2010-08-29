using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCTypeDeclarationImpl : MCMemberDeclarationImpl, ITypeDeclaration
    {
        readonly TypeReference typeRef;

        readonly TypeDefinition typeDef;
        readonly Type type;
        readonly ITypeDeclaration baseTypeDecl;
        public MCTypeDeclarationImpl(TypeReference typeRef)
            : base(typeRef)
        {
            this.typeRef = typeRef;
            typeDef = typeRef as TypeDefinition;
            // MEMO: typeRef.Resolve() で TypeDefinition に変換するって想定だったっぽい。なるほど。
            if (typeDef == null)
            {
                type = Type.GetType(typeRef.FullName);
                baseTypeDecl = typeRef.Equivalent(typeof(object)) ?
                    null : (MCTypeDeclarationImpl)typeRef.Module.Import(type.BaseType);
            }
            else
            {
                type = null;
                baseTypeDecl = (MCTypeDeclarationImpl)typeDef.BaseType;
            }
        }

        public static explicit operator MCTypeDeclarationImpl(TypeReference typeRef)
        {
            return new MCTypeDeclarationImpl(typeRef);
        }

        public static explicit operator TypeReference(MCTypeDeclarationImpl typeDecl)
        {
            return typeDecl.typeRef;
        }

        public string FullName
        {
            get { return typeRef.FullName; }
        }

        public string AssemblyQualifiedName
        {
            get { return typeRef.FullName + ", " + typeRef.Module.Assembly.FullName; }
        }

        public ITypeDeclaration BaseType
        {
            get { return baseTypeDecl; }
        }

        public IModuleDeclaration Module { get { throw new NotImplementedException(); } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            if (typeDef == null)
            {
                return (MCConstructorDeclarationImpl)typeRef.Module.Import(
                    type.GetConstructor(SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, null, types, null));
            }
            else
            {
                return (MCConstructorDeclarationImpl)typeDef.GetConstructor(SR::BindingFlags.Default, types);
            }
        }
    }

}
