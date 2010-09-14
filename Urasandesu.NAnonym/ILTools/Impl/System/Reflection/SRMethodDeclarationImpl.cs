using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRMethodDeclarationImpl : SRMethodBaseDeclarationImpl, IMethodDeclaration
    {
        readonly MethodInfo methodInfo;
        public SRMethodDeclarationImpl(MethodInfo methodInfo)
            : base(methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        #region IMethodDeclaration メンバ

        public ITypeDeclaration ReturnType
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
