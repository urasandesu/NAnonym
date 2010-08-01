using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCTypeDeclarationImpl : MCMemberDeclarationImpl, ITypeDeclaration
    {
        readonly TypeReference typeRef;
        public MCTypeDeclarationImpl(TypeReference typeRef)
            : base(typeRef)
        {
            this.typeRef = typeRef;
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

        public ITypeDeclaration BaseType
        {
            get { throw new NotImplementedException(); }
        }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            throw new NotImplementedException();
        }
    }
}
