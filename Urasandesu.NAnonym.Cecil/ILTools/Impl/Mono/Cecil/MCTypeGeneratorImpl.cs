﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using SR = System.Reflection;
using MC = Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCTypeGeneratorImpl : MCTypeDeclarationImpl, ITypeGenerator
    {
        TypeDefinition typeDef;
        ReadOnlyCollection<IFieldGenerator> fields;
        public MCTypeGeneratorImpl(TypeDefinition typeDef)
            : base(typeDef)
        {
            this.typeDef = typeDef;
            fields = new ReadOnlyCollection<IFieldGenerator>(base.Fields.TransformEnumerateOnly(fieldDecl => (IFieldGenerator)fieldDecl));
        }

        internal TypeDefinition TypeDef
        {
            get { return typeDef; }
        }

        public IFieldGenerator AddField(string fieldName, Type type, SR::FieldAttributes attributes)
        {
            var fieldDef = new FieldDefinition(fieldName, (MC::FieldAttributes)attributes, TypeDef.Module.Import(type));
            TypeDef.Fields.Add(fieldDef);
            return (MCFieldGeneratorImpl)fieldDef;
        }

        #region ITypeGenerator メンバ

        public IMethodBaseGenerator AddMethod(string name, SR::MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            var methodDef = new MethodDefinition(name, (MethodAttributes)attributes, typeDef.Module.Import(returnType));
            typeDef.Methods.Add(methodDef);
            parameterTypes.ForEach(parameterType => methodDef.Parameters.Add(new ParameterDefinition(typeDef.Module.Import(parameterType))));
            return new MCMethodGeneratorImpl(methodDef);
        }

        #endregion

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
