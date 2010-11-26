using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRMemberDeclarationImpl : ManuallyDeserializable, IMemberDeclaration
    {
        readonly object source;

        public SRMemberDeclarationImpl(MemberInfo memberInfo)
            : base(true)
        {
            this.source = memberInfo;
        }

        internal MemberInfo Source { get { return (MemberInfo)((IMemberDeclaration)this).Source; } }

        object IMemberDeclaration.Source { get { return source; } }

        public string Name
        {
            get { return Source.Name; }
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
