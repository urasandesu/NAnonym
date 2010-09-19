using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCDirectiveDeclarationImpl : IDirectiveDeclaration
    {
        #region IDirectiveDeclaration メンバ

        public OpCode OpCode
        {
            get { throw new NotImplementedException(); }
        }

        public object Operand
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
