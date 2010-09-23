using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    // 安全のために関連付いているかどうかのフラグが必要かも？
    sealed class MCLocalGeneratorImpl : UN::ILTools.ILocalGenerator
    {
        readonly VariableDefinition variableDef;
        public MCLocalGeneratorImpl(VariableDefinition variableDef)
        {
            this.variableDef = variableDef;
        }

        internal VariableDefinition VariableDef { get { return variableDef; } }

        public string Name
        {
            get { return variableDef.Name; }
        }

        #region ILocalDeclaration メンバ


        public UN::ILTools.ITypeDeclaration Type
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
