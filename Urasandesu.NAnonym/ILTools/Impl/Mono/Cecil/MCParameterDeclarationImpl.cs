using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    class MCParameterDeclarationImpl : IParameterDeclaration
    {
        readonly ParameterReference parameterRef;
        public MCParameterDeclarationImpl(ParameterReference parameterRef)
        {
            this.parameterRef = parameterRef;
        }

        #region IParameterDeclaration メンバ

        public string Name
        {
            get { return parameterRef.Name; }
        }

        public ITypeDeclaration ParameterType
        {
            get { return (MCTypeDeclarationImpl)parameterRef.ParameterType; }
        }

        #endregion
    }
}
