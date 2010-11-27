using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UNI = Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCConstructorDeclarationImpl : MCMethodBaseDeclarationImpl, UNI::IConstructorDeclaration
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
