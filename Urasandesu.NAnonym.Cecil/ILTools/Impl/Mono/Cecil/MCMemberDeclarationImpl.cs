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

        //[OnDeserialized]
        //internal void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized && context.Context != null)
        //    {
        //        deserialized = true;
        //        Initialize((MemberReference)context.Context);
        //    }
        //}

        //#region IDeserializationCallback メンバ

        //public virtual void OnDeserialization(object sender)
        //{
        //    var memberRef = default(MemberReference);
        //    if ((memberRef = sender as MemberReference) != null)
        //    {
        //        Initialize(memberRef);
        //    }
        //}

        //#endregion

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var memberRef = default(MemberReference);
            if ((memberRef = context.Context as MemberReference) != null)
            {
                Initialize(memberRef);
            }
        }
    }
}
