using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCMethodBodyDeclarationImpl : UN::ILTools.IMethodBodyDeclaration
    {
        #region IMethodBodyDeclaration メンバ

        public ReadOnlyCollection<UN::ILTools.ILocalDeclaration> Locals
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<UN::ILTools.IDirectiveDeclaration> Directives
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
