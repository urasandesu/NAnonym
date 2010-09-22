using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCParameterDeclarationImpl : UN::ILTools.IParameterDeclaration
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

        public UN::ILTools.ITypeDeclaration ParameterType
        {
            get { return (MCTypeDeclarationImpl)parameterRef.ParameterType; }
        }

        public int Position
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
