using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        readonly TypeBuilder typeBuilder;
        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        #region ITypeGenerator メンバ

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
