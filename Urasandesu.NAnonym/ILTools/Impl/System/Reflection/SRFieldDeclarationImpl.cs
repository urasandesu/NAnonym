using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRFieldDeclarationImpl : SRMemberDeclarationImpl, IFieldDeclaration
    {
        FieldInfo fieldInfo;
        public SRFieldDeclarationImpl(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        string IMemberDeclaration.Name
        {
            get { return fieldInfo.Name; }
        }

        void IManuallyDeserializable.OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        internal FieldInfo FieldInfo { get { return fieldInfo; } }

        public Type FieldType
        {
            get { throw new NotImplementedException(); }
        }

    }
}
