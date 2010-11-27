using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMethodDeclarationImpl : MCMethodBaseDeclarationImpl, UNI::IMethodDeclaration
    {
        public MCMethodDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
        }

        public MCMethodDeclarationImpl(MethodReference methodRef, ILEmitMode mode, Instruction target)
            : base(methodRef, mode, target)
        {
        }

        public UNI::ITypeDeclaration ReturnType
        {
            get { return new MCTypeDeclarationImpl(MethodDef.ReturnType); }
        }
    }
}
