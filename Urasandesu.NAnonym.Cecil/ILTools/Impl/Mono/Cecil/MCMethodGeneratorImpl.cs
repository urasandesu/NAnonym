/* 
 * File: MCMethodGeneratorImpl.cs
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
using Mono.Cecil.Cil;
using MCO = Mono.Collections;
using MC = Mono.Cecil;
using System.Collections.ObjectModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using SR = System.Reflection;
using System.Runtime.Serialization;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    sealed class MCMethodGeneratorImpl : MCMethodDeclarationImpl, UNI::IMethodGenerator
    {
        MCMethodBaseGeneratorImpl methodBaseGen;
        [NonSerialized]
        UNI::ITypeGenerator returnTypeGen;

        public MCMethodGeneratorImpl(MethodDefinition methodDef)
            : base(methodDef)
        {
            methodBaseGen = new MCMethodBaseGeneratorImpl(methodDef);
            Initialize(methodDef);
        }

        public MCMethodGeneratorImpl(MethodDefinition methodDef, ILEmitMode mode, Instruction target)
            : base(methodDef, mode, target)
        {
            methodBaseGen = new MCMethodBaseGeneratorImpl(methodDef, mode, target);
            Initialize(methodDef);
        }

        void Initialize(MethodDefinition methodDef)
        {
            var returnTypeDef = methodDef.ReturnType.Resolve();
            returnTypeGen = new MCTypeGeneratorImpl(returnTypeDef);
        }

        public new UNI::ITypeGenerator ReturnType
        {
            get { return returnTypeGen; }
        }

        public new UNI::IMethodBodyGenerator Body
        {
            get { return methodBaseGen.Body; }
        }

        public new UNI::ITypeGenerator DeclaringType
        {
            get { return methodBaseGen.DeclaringType; }
        }

        public new ReadOnlyCollection<UNI::IParameterGenerator> Parameters
        {
            get { return methodBaseGen.Parameters; }
        }

        public UNI::IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            return methodBaseGen.AddPortableScopeItem(fieldInfo);
        }

        public UNI::IMethodBaseGenerator CreateInstance(string name, SR::MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            base.OnDeserializedManually(context);
            Initialize(MethodDef);
        }

        public UNI::IMethodBaseGenerator ExpressBody(Action<UNI::ReflectiveMethodDesigner> bodyExpression)
        {
            methodBaseGen.ExpressBody(bodyExpression);
            return this;
        }

        public UNI::IParameterGenerator AddParameter(int position, SR::ParameterAttributes attributes, string parameterName)
        {
            throw new NotImplementedException();
        }

        public UNI::PortableScope CarryPortableScope()
        {
            return methodBaseGen.CarryPortableScope();
        }
    }
}

