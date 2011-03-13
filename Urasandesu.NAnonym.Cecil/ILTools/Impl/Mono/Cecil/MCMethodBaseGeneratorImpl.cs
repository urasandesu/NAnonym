/* 
 * File: MCMethodBaseGeneratorImpl.cs
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
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using System.Runtime.Serialization;
using MC = Mono.Cecil;
using System.Reflection;
using Mono.Cecil.Cil;
using UNI = Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCMethodBaseGeneratorImpl : MCMethodBaseDeclarationImpl, UNI::IMethodBaseGenerator
    {
        [NonSerialized]
        ReadOnlyCollection<UNI::IParameterGenerator> parameters;

        public MCMethodBaseGeneratorImpl(MethodDefinition methodDef)
            : base(methodDef)
        {
            parameters = new ReadOnlyCollection<UNI::IParameterGenerator>(
                base.Parameters.TransformEnumerateOnly(paramter => (UNI::IParameterGenerator)paramter));
        }

        public MCMethodBaseGeneratorImpl(MethodDefinition methodDef, ILEmitMode mode, Instruction target)
            : base(methodDef, mode, target)
        {
            parameters = new ReadOnlyCollection<UNI::IParameterGenerator>(
                base.Parameters.TransformEnumerateOnly(paramter => (UNI::IParameterGenerator)paramter));
        }

        public new UNI::IMethodBodyGenerator Body
        {
            get { return (UNI::IMethodBodyGenerator)BodyDecl; }
        }

        public new UNI::ITypeGenerator DeclaringType
        {
            get { return (UNI::ITypeGenerator)DeclaringTypeDecl; }
        }

        public new ReadOnlyCollection<UNI::IParameterGenerator> Parameters
        {
            get { return parameters; }
        }

        public UNI::IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            var variableDef = new VariableDefinition(fieldInfo.Name, MethodDef.Module.Import(fieldInfo.FieldType));
            MethodDef.Body.Variables.Add(variableDef);
            var itemRawData = new UNI::PortableScopeItemRawData(this, variableDef.Name, variableDef.Index);
            var fieldDef = new FieldDefinition(itemRawData.FieldName, MC::FieldAttributes.Private | MC::FieldAttributes.SpecialName, MethodDef.Module.Import(fieldInfo.FieldType));
            MethodDef.DeclaringType.Fields.Add(fieldDef);
            return new MCPortableScopeItemImpl(itemRawData, fieldDef, variableDef);
        }

        public UNI::IMethodBaseGenerator ExpressBody(Action<UNI::ReflectiveMethodDesigner> bodyExpression)
        {
            MethodDef.Body.InitLocals = true;
            var gen = new UNI::ReflectiveMethodDesigner(this);
            bodyExpression(gen);
            if (gen.Directives.Last().OpCode != UNI::OpCodes.Ret)
            {
                gen.Eval(() => Dsl.End());
            }
            return this;
        }

        public UNI::IParameterGenerator AddParameter(int position, SR::ParameterAttributes attributes, string parameterName)
        {
            throw new NotImplementedException();
        }

        public UNI::PortableScope CarryPortableScope()
        {
            var scope = new UNI::PortableScope(this);
            return scope;
        }


        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression)
        {
            return ExpressBody2(bodyExpression, typeof(void).ToTypeDecl());
        }


        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression, ITypeDeclaration returnType)
        {
            var gen = new ReflectiveMethodDesigner2();
            gen.ILBuilder = new ILBuilder(this, returnType);
            bodyExpression(gen);
            gen.Eval(() => Dsl.End());
            return this;
        }
    }
}

