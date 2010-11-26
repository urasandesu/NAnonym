using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMemberDeclarationImpl : UN::ILTools.ManuallyDeserializable, UN::ILTools.IMemberDeclaration
    {
        [NonSerialized]
        MemberReference memberRef;

        public MCMemberDeclarationImpl(MemberReference memberRef)
            : base(true)
        {
            Initialize(memberRef);
        }

        void Initialize(MemberReference memberRef)
        {
            this.memberRef = memberRef;
        }

        #region IMemberDeclaration メンバ

        public string Name
        {
            get { return memberRef.Name; }
        }

        #endregion

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var memberRef = default(MemberReference);
            if ((memberRef = context.Context as MemberReference) != null)
            {
                Initialize(memberRef);
            }
        }

        #region IMemberDeclaration メンバ


        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
