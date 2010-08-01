using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    class SRMemberDeclarationImpl : IMemberDeclaration
    {
        readonly MemberInfo memberInfo;
        public SRMemberDeclarationImpl(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        #region IMemberDeclaration メンバ

        public string Name
        {
            get { return memberInfo.Name; }
        }

        #endregion
    }
}
