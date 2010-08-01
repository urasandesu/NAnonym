using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCMethodDeclarationImpl : MCMethodBaseDeclarationImpl, IMethodDeclaration
    {
        readonly MethodReference methodRef;
        public MCMethodDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
            this.methodRef = methodRef;
        }

        #region IMethodDeclaration メンバ

        public ITypeDeclaration ReturnType
        {
            get { return (MCTypeDeclarationImpl)methodRef.ReturnType; }
        }

        #endregion
    }
}
