/* 
 * File: MCPortableScopeItemImpl.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

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
    class MCPortableScopeItemImpl : UN::ManuallyDeserializable, UNI::IPortableScopeItem
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


        public ITypeDeclaration DeclaringType
        {
            get { throw new NotImplementedException(); }
        }


        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
        }


        public bool IsPublic
        {
            get { throw new NotImplementedException(); }
        }
    }
}

