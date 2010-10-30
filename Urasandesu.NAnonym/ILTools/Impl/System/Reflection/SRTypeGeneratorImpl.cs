using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        readonly TypeBuilder typeBuilder;
        readonly FieldBuilder[] fieldBuilders;

        List<IFieldGenerator> listFields;
        ReadOnlyCollection<IFieldGenerator> fields;

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : base(typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
            : this(typeBuilder)
        {
            this.fieldBuilders = fieldBuilders;
            listFields = new List<IFieldGenerator>();
            listFields.AddRange(fieldBuilders.Select(fieldBuilder => (IFieldGenerator)new SRFieldGeneratorImpl(fieldBuilder)));
            fields = new ReadOnlyCollection<IFieldGenerator>(listFields);
        }

        #region ITypeGenerator メンバ

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator AddMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodBuilder = typeBuilder.DefineMethod(name, attributes, returnType, parameterTypes);
            return new SRMethodGeneratorImpl(methodBuilder);
        }

        #endregion

        public override IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldBuilder = fieldBuilders.FirstOrDefault(_fieldBuilder => _fieldBuilder.Name == name && (_fieldBuilder.ExportBinding() & bindingAttr) != 0);
            return fieldBuilder == null ? null : new SRFieldGeneratorImpl(fieldBuilder);
        }

        internal FieldBuilder[] FieldBuilders { get { return fieldBuilders; } }

        #region ITypeGenerator メンバ


        public new ReadOnlyCollection<IFieldGenerator> Fields
        {
            get { return fields; }
        }

        #endregion

        #region ITypeGenerator メンバ


        public new IModuleGenerator Module
        {
            get { return base.Module as IModuleGenerator; }
        }

        #endregion
    }
}
