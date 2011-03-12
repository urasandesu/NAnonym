/* 
 * File: MCConstructorGeneratorImpl.cs
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
using UNI = Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCConstructorGeneratorImpl : MCConstructorDeclarationImpl, UNI::IConstructorGenerator
    {
        MCMethodBaseGeneratorImpl methodBaseGen;

        public MCConstructorGeneratorImpl(MethodDefinition constructorDef)
            : base(constructorDef)
        {
            methodBaseGen = new MCMethodBaseGeneratorImpl(constructorDef);
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

        public UNI::IPortableScopeItem AddPortableScopeItem(SR::FieldInfo fieldInfo)
        {
            return methodBaseGen.AddPortableScopeItem(fieldInfo);
        }

        public UNI::IMethodBaseGenerator ExpressBody(Action<UNI::ReflectiveMethodDesigner> bodyExpression)
        {
            methodBaseGen.ExpressBody(bodyExpression);
            return this;
        }

        public UNI::IParameterGenerator AddParameter(int position, SR::ParameterAttributes attributes, string parameterName)
        {
            return methodBaseGen.AddParameter(position, attributes, parameterName);
        }

        public UNI::PortableScope CarryPortableScope()
        {
            return methodBaseGen.CarryPortableScope();
        }


        public UNI::IMethodBaseGenerator ExpressBody2(Action<UNI::ReflectiveMethodDesigner2> bodyExpression)
        {
            methodBaseGen.ExpressBody2(bodyExpression);
            return this;
        }
    }
}

