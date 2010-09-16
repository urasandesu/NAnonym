using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCConstructorDeclarationImpl : MCMethodBaseDeclarationImpl, UN::ILTools.IConstructorDeclaration
    {
        readonly MethodReference constructorRef;
        public MCConstructorDeclarationImpl(MethodReference constructorRef)
            : base(constructorRef)
        {
            this.constructorRef = constructorRef;
        }

        public static explicit operator MCConstructorDeclarationImpl(MethodReference constructorRef)
        {
            return new MCConstructorDeclarationImpl(constructorRef);
        }

        public static explicit operator MethodReference(MCConstructorDeclarationImpl constructorDecl)
        {
            return constructorDecl.constructorRef;
        }
    }
}
