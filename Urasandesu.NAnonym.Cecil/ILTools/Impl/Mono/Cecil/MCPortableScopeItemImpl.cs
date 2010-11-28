using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using MC = Mono.Cecil;
using Mono.Cecil.Cil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCPortableScopeItemImpl : UNI::ManuallyDeserializable, UNI::IPortableScopeItem
    {
        UNI::PortableScopeItemRawData itemRawData;

        [NonSerialized]
        internal FieldDefinition fieldDef;

        [NonSerialized]
        internal VariableDefinition variableDef;
        string variableTypeFullName;
        int variableIndex;

        public MCPortableScopeItemImpl(UNI::PortableScopeItemRawData itemRawData, FieldDefinition fieldDef, VariableDefinition variableDef)
            : this(itemRawData, default(object), fieldDef, variableDef)
        {
        }

        // NOTE: 構築中はこちら。まだ実体化してないので、GlobalAssemblyResolver.Resolve できない。
        public MCPortableScopeItemImpl(UNI::PortableScopeItemRawData itemRawData, object value, FieldDefinition fieldDef, VariableDefinition variableDef)
            : base(true)
        {
            this.itemRawData = itemRawData;
            variableTypeFullName = variableDef.VariableType.FullName;
            variableIndex = variableDef.Index;
            Value = value;
            Initialize(this, fieldDef, variableDef);
        }

        static void Initialize(MCPortableScopeItemImpl that, FieldDefinition fieldDef, VariableDefinition variableDef)
        {
            that.fieldDef = fieldDef;
            that.variableDef = variableDef; // MEMO: Index で取得できる範囲の変数は、構築時の名前が残っていないのかも。
        }

        internal FieldDefinition FieldDef { get { return fieldDef; } }

        public object Value { get; set; }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return itemRawData.LocalName; }
        }

        string ILocalDeclaration.Name
        {
            get { throw new NotImplementedException(); }
        }

        string IMemberDeclaration.Name
        {
            get { throw new NotImplementedException(); }
        }

        public UNI::ITypeDeclaration Type
        {
            get { throw new NotImplementedException(); }
        }

        public int Index
        {
            get { throw new NotImplementedException(); }
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            //itemRawData.OnDeserialized(context);

            var methodDecl = default(IMethodBaseDeclaration);
            if ((methodDecl = context.Context as IMethodBaseDeclaration) != null)
            {
                var methodDef = ((MCMethodGeneratorImpl)methodDecl).MethodDef;
                var fieldDef = methodDef.DeclaringType.Fields.First(_fieldDef => _fieldDef.Name == itemRawData.FieldName);
                var variableDef = methodDef.Body.Variables.First(
                    _variableDef => _variableDef.VariableType.FullName == variableTypeFullName && _variableDef.Index == variableIndex);
                Initialize(this, fieldDef, variableDef);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Type FieldType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
