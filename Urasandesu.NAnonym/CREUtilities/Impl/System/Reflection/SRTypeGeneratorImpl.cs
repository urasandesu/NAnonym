using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        readonly TypeBuilder typeBuilder;
        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        public static explicit operator SRTypeGeneratorImpl(TypeBuilder typeBuilder)
        {
            return new SRTypeGeneratorImpl(typeBuilder);
        }

        public static explicit operator TypeBuilder(SRTypeGeneratorImpl typeGen)
        {
            return typeGen.typeBuilder;
        }

        #region ITypeGenerator メンバ

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
