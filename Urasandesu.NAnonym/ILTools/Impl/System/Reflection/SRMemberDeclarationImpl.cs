using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRMemberDeclarationImpl : ManuallyDeserializable, IMemberDeclaration
    {
        readonly MemberInfo memberInfo;
        public SRMemberDeclarationImpl(MemberInfo memberInfo)
            : base(true)
        {
            this.memberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get { return memberInfo; } }

        #region IMemberDeclaration メンバ

        public object Source { get { throw new NotImplementedException(); } }

        public string Name
        {
            get { return memberInfo.Name; }
        }

        #endregion

        protected override void OnDeserializedManually(global::System.Runtime.Serialization.StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
