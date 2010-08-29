using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    class SRTypeDeclarationImpl : SRMemberDeclarationImpl, ITypeDeclaration
    {
        readonly Type type;

        readonly ITypeDeclaration baseTypeDecl;
        public SRTypeDeclarationImpl(Type type)
            : base(type)
        {
            this.type = type;
            baseTypeDecl = type == typeof(object) ? null : (SRTypeDeclarationImpl)type.BaseType;
        }

        public static explicit operator SRTypeDeclarationImpl(Type type)
        {
            return new SRTypeDeclarationImpl(type);
        }

        public static explicit operator Type(SRTypeDeclarationImpl typeDecl)
        {
            return typeDecl.type;
        }

        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public string AssemblyQualifiedName { get { throw new NotImplementedException(); } }

        public ITypeDeclaration BaseType
        {
            get { return baseTypeDecl; }
        }

        public IModuleDeclaration Module { get { throw new NotImplementedException(); } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            return (SRConstructorDeclarationImpl)type.GetConstructor(types);
        }
    }

}
