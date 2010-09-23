using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MCO = Mono.Collections;
using MC = Mono.Cecil;
using System.Collections.ObjectModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using SR = System.Reflection;
using System.Runtime.Serialization;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCMethodGeneratorImpl : MCMethodDeclarationImpl, IMethodGenerator
    {
        [NonSerialized]
        ITypeGenerator returnTypeGen;
        [NonSerialized]
        ReadOnlyCollection<IParameterGenerator> parameters;

        public MCMethodGeneratorImpl(MethodDefinition methodDef)
            : base(methodDef)
        {
            Initialize(methodDef);
        }

        void Initialize(MethodDefinition methodDef)
        {
            var returnTypeDef = methodDef.ReturnType.Resolve();
            returnTypeGen = (MCTypeGeneratorImpl)returnTypeDef;
            // TODO: 反変がサポートされるようになったら修正する。
            parameters = new ReadOnlyCollection<IParameterGenerator>(
                base.Parameters.TransformEnumerateOnly(paramter => (IParameterGenerator)paramter));
        }

        #region IMethodGenerator メンバ

        public new ITypeGenerator ReturnType
        {
            get { return returnTypeGen; }
        }

        #endregion

        #region IMethodBaseGenerator メンバ

        public new IMethodBodyGenerator Body
        {
            get { return (IMethodBodyGenerator)BodyDecl; }
        }

        public new ITypeGenerator DeclaringType
        {
            get { return (ITypeGenerator)DeclaringTypeDecl; }
        }

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return parameters; }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            var variableDef = new VariableDefinition(fieldInfo.Name, MethodDef.Module.Import(fieldInfo.FieldType));
            MethodDef.Body.Variables.Add(variableDef);
            var itemRawData = new PortableScopeItemRawData(this, variableDef.Name, variableDef.Index);
            var fieldDef = new FieldDefinition(itemRawData.FieldName, MC::FieldAttributes.Private | MC::FieldAttributes.SpecialName, MethodDef.Module.Import(fieldInfo.FieldType));
            MethodDef.DeclaringType.Fields.Add(fieldDef);
            return new MCPortableScopeItemImpl(itemRawData, fieldDef, variableDef);
        }

        #endregion

        //[OnDeserialized]
        //internal new void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        base.OnDeserialized(context);
        //        Initialize(MethodDef);
        //    }
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            base.OnDeserializedManually(context);
            Initialize(MethodDef);
        }
    }
}
