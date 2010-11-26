using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeGeneratorImpl : SRTypeDeclarationImpl, ITypeGenerator
    {
        List<FieldBuilder> fieldBuilders;
        List<IFieldGenerator> listFields;
        ReadOnlyCollection<IFieldGenerator> fields;

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder)
            : this(typeBuilder, new FieldBuilder[] { })
        {
        }

        public SRTypeGeneratorImpl(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
            : base(typeBuilder)
        {
            this.fieldBuilders = new List<FieldBuilder>(fieldBuilders);
            listFields = new List<IFieldGenerator>();
            listFields.AddRange(fieldBuilders.Select(fieldBuilder => (IFieldGenerator)new SRFieldGeneratorImpl(fieldBuilder)));
            fields = new ReadOnlyCollection<IFieldGenerator>(listFields.TransformEnumerateOnly(fieldGen => (IFieldGenerator)fieldGen));
        }

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldBuilder = Source.DefineField(fieldName, type, attributes);
            var fieldGen = new SRFieldGeneratorImpl(fieldBuilder);
            listFields.Add(fieldGen);
            return fieldGen;
        }

        public IMethodBaseGenerator AddMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodBuilder = Source.DefineMethod(name, attributes, returnType, parameterTypes);
            return new SRMethodGeneratorImpl(methodBuilder);
        }

        public override IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldBuilder = fieldBuilders.FirstOrDefault(_fieldBuilder => _fieldBuilder.Name == name && (_fieldBuilder.ExportBinding() & bindingAttr) != 0);
            return fieldBuilder == null ? null : new SRFieldGeneratorImpl(fieldBuilder);
        }

        internal FieldBuilder[] FieldBuilders { get { return fieldBuilders.ToArray(); } }
        internal new TypeBuilder Source { get { return (TypeBuilder)base.Source; } }

        public new ReadOnlyCollection<IFieldGenerator> Fields
        {
            get { return fields; }
        }

        public new IModuleGenerator Module
        {
            get { return base.Module as IModuleGenerator; }
        }

        public ITypeGenerator AddInterfaceImplementation(Type interfaceType)
        {
            Source.AddInterfaceImplementation(interfaceType);
            return this;
        }

        public IConstructorGenerator AddConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
        {
            var constructorBuilder = Source.DefineConstructor(attributes, callingConvention, parameterTypes);
            var constructorGen = new SRConstructorGeneratorImpl(this, constructorBuilder);
            return constructorGen;
        }

        public Type CreateType()
        {
            return Source.CreateType();
        }

        public IMethodBaseGenerator AddMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            var methodBuilder = Source.DefineMethod(name, attributes, callingConvention, returnType, parameterTypes);
            var methodGen = new SRMethodGeneratorImpl(this, methodBuilder);
            return methodGen;
        }
    }
}
