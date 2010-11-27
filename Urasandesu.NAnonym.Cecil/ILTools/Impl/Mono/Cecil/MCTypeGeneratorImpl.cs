using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using SR = System.Reflection;
using MC = Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.System.Reflection;


namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCTypeGeneratorImpl : MCTypeDeclarationImpl, UNI::ITypeGenerator
    {
        [NonSerialized]
        ReadOnlyCollection<UNI::IFieldGenerator> fields;

        public MCTypeGeneratorImpl(TypeDefinition typeDef)
            : base(typeDef)
        {
            fields = new ReadOnlyCollection<UNI::IFieldGenerator>(base.Fields.TransformEnumerateOnly(fieldDecl => (UNI::IFieldGenerator)fieldDecl));
        }

        internal new TypeDefinition TypeDef
        {
            get { return base.TypeDef; }
        }

        public UNI::IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldDef = new FieldDefinition(fieldName, (MC::FieldAttributes)attributes, TypeDef.Module.Import(type));
            TypeDef.Fields.Add(fieldDef);
            return (MCFieldGeneratorImpl)fieldDef;
        }

        public UNI::IMethodBaseGenerator AddMethod(string name, SR::MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodDef = new MethodDefinition(name, (MethodAttributes)attributes, TypeDef.Module.Import(returnType));
            TypeDef.Methods.Add(methodDef);
            parameterTypes.ForEach(parameterType => methodDef.Parameters.Add(new ParameterDefinition(TypeDef.Module.Import(parameterType))));
            return new MCMethodGeneratorImpl(methodDef);
        }

        public new ReadOnlyCollection<UNI::IFieldGenerator> Fields
        {
            get { return fields; }
        }

        public new UNI::IModuleGenerator Module
        {
            get { return base.Module as UNI::IModuleGenerator; }
        }

        public UNI::IMethodBaseGenerator AddMethod(string name, SR::MethodAttributes attributes, SR::CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public UNI::ITypeGenerator AddInterfaceImplementation(Type interfaceType)
        {
            throw new NotImplementedException();
        }

        public UNI::IConstructorGenerator AddConstructor(SR::MethodAttributes attributes, SR::CallingConventions callingConvention, Type[] parameterTypes)
        {
            var constructorDef = new MethodDefinition(".ctor", (MethodAttributes)attributes, TypeDef.Module.Import(typeof(void)));
            //constructorDef.CallingConvention = callingConvention.ToCecil();   // TODO: うまく動いてなさそう？
            parameterTypes.Select(parameterType => new ParameterDefinition(TypeDef.Module.Import(parameterType))).AddRangeTo(constructorDef.Parameters);
            TypeDef.Methods.Add(constructorDef);
            return new MCConstructorGeneratorImpl(constructorDef);
        }

        public Type CreateType()
        {
            throw new NotImplementedException();
        }
    }
}
