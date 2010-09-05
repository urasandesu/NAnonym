using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    // 安全のために関連付いているかどうかのフラグが必要かも？
    sealed class MCLocalGeneratorImpl : ILocalGenerator
    {
        readonly VariableDefinition variableDef;
        public MCLocalGeneratorImpl(VariableDefinition variableDef)
        {
            this.variableDef = variableDef;
        }

        public static explicit operator MCLocalGeneratorImpl(VariableDefinition variableDef)
        {
            return new MCLocalGeneratorImpl(variableDef);
        }

        public static explicit operator VariableDefinition(MCLocalGeneratorImpl localGen)
        {
            return localGen.variableDef;
        }

        public string Name
        {
            get { return variableDef.Name; }
        }

        #region ILocalDeclaration メンバ


        public ITypeDeclaration Type
        {
            get { throw new NotImplementedException(); }
        }

        public int Index
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
