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

        internal MethodReference ConstructorRef { get { return constructorRef; } }
    }
}
