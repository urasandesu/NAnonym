using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMethodDeclarationImpl : MCMethodBaseDeclarationImpl, IMethodDeclaration
    {
        public MCMethodDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
        }

        #region IMethodDeclaration メンバ

        public ITypeDeclaration ReturnType
        {
            get { return (MCTypeDeclarationImpl)MethodDef.ReturnType; }
        }

        #endregion
    }
}
