using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    class MCMethodBodyDeclarationImpl : IMethodBodyDeclaration
    {
        #region IMethodBodyDeclaration メンバ

        public ReadOnlyCollection<ILocalDeclaration> Locals
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IDirectiveDeclaration> Directives
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
