using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMemberDeclarationImpl : UNI::ManuallyDeserializable, UNI::IMemberDeclaration
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

        public string Name
        {
            get { return memberRef.Name; }
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var memberRef = default(MemberReference);
            if ((memberRef = context.Context as MemberReference) != null)
            {
                Initialize(memberRef);
            }
        }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

    }
}
