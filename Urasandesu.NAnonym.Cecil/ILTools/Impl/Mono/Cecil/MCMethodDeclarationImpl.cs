using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMethodDeclarationImpl : MCMethodBaseDeclarationImpl, UN::ILTools.IMethodDeclaration
    {
        public MCMethodDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
        }

        #region IMethodDeclaration メンバ

        public UN::ILTools.ITypeDeclaration ReturnType
        {
            get { return new MCTypeDeclarationImpl(MethodDef.ReturnType); }
        }

        #endregion
    }
}
