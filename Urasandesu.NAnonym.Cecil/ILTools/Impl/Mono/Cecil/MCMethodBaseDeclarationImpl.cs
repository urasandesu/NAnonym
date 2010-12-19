/* 
 * File: MCMethodBaseDeclarationImpl.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Urasandesu.NAnonym.Linq;
using MC = Mono.Cecil;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCMethodBaseDeclarationImpl : MCMemberDeclarationImpl, UNI::IMethodBaseDeclaration
    {
        [NonSerialized]
        MethodReference methodRef;

        [NonSerialized]
        MethodDefinition methodDef;
        string methodName;
        MC::MethodAttributes methodAttr;
        string[] parameterTypeFullNames;

        UNI::ITypeDeclaration declaringTypeDecl;

        [NonSerialized]
        UNI::IMethodBodyDeclaration bodyDecl;
        [NonSerialized]
        ReadOnlyCollection<UNI::IParameterDeclaration> parameters;

        public MCMethodBaseDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
            Initialize(methodRef, ILEmitMode.Normal, null);
        }

        public MCMethodBaseDeclarationImpl(MethodReference methodRef, ILEmitMode mode, Instruction target)
            : base(methodRef)
        {
            Initialize(methodRef, mode, target);
        }

        void Initialize(MethodReference methodRef, ILEmitMode mode, Instruction target)
        {
            this.methodRef = methodRef;
            this.methodDef = methodRef.Resolve();
            methodName = methodDef.Name;
            methodAttr = methodDef.Attributes;
            parameterTypeFullNames = methodDef.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray();
            if (methodDef.Body != null)
            {
                bodyDecl = new MCMethodBodyGeneratorImpl(methodDef.Body, mode, target);
            }
            declaringTypeDecl = new MCTypeGeneratorImpl(methodRef.DeclaringType.Resolve());
            parameters = new ReadOnlyCollection<UNI::IParameterDeclaration>(
                methodRef.Parameters.TransformEnumerateOnly(parameter => (UNI::IParameterDeclaration)new MCParameterGeneratorImpl(parameter)));
        }


        public UNI::IMethodBodyDeclaration Body
        {
            get { return bodyDecl; }
        }

        public UNI::ITypeDeclaration DeclaringType
        {
            get { return declaringTypeDecl; }
        }

        public ReadOnlyCollection<UNI::IParameterDeclaration> Parameters
        {
            get { return parameters; }
        }

        public UNI::IPortableScopeItem NewPortableScopeItem(UNI::PortableScopeItemRawData itemRawData, object value)
        {
            var fieldDef = MethodDef.DeclaringType.Fields.First(field => field.Name == itemRawData.FieldName);
            var variableDef = MethodDef.Body.Variables.First(variable => variable.Index == itemRawData.LocalIndex);
            return new MCPortableScopeItemImpl(itemRawData, value, fieldDef, variableDef);
        }


        internal MethodDefinition MethodDef { get { return methodDef; } }
        protected UNI::IMethodBodyDeclaration BodyDecl { get { return bodyDecl; } }
        protected UNI::ITypeDeclaration DeclaringTypeDecl { get { return declaringTypeDecl; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var declaringTypeGen = (MCTypeGeneratorImpl)this.declaringTypeDecl;
            declaringTypeGen.OnDeserialized(context);
            var typeDef = declaringTypeGen.TypeDef;
            var methodDef = typeDef.Methods.First(
                method =>
                    method.Name == methodName &&
                    method.Attributes == methodAttr &&
                    method.Parameters.Select(parameter => parameter.ParameterType.FullName).Equivalent(parameterTypeFullNames));
            // TODO: PortableScope 系のテストを通るようにする。
            Initialize(methodDef, ILEmitMode.Normal, null);
            base.OnDeserializedManually(new StreamingContext(context.State, methodDef));
        }
    }
}

