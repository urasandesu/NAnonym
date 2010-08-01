using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCMemberDeclarationImpl : IMemberDeclaration
    {
        readonly MemberReference memberRef;
        public MCMemberDeclarationImpl(MemberReference memberRef)
        {
            this.memberRef = memberRef;
        }

        #region IMemberDeclaration メンバ

        public string Name
        {
            get { return memberRef.Name; }
        }

        #endregion
    }
}
