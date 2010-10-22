using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        readonly TypeBuilder typeBuilder;
        readonly FieldBuilder[] fieldBuilders;

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
            : this(typeBuilder)
        {
            this.fieldBuilders = fieldBuilders;
        }

        #region ITypeGenerator メンバ

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator AddMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldBuilder = fieldBuilders.FirstOrDefault(_fieldBuilder => _fieldBuilder.Name == name && (_fieldBuilder.ExportBinding() & bindingAttr) != 0);
            return fieldBuilder == null ? null : new SRFieldGeneratorImpl(fieldBuilder);
        }

        internal FieldBuilder[] FieldBuilders { get { return fieldBuilders; } }
    }
}
