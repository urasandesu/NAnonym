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

        #region IMemberDeclaration メンバ

        string IMemberDeclaration.Name
        {
            get { return fieldInfo.Name; }
        }

        #endregion

        #region IDeserializableManually メンバ

        void IDeserializableManually.OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        internal FieldInfo FieldInfo { get { return fieldInfo; } }
    }
}
