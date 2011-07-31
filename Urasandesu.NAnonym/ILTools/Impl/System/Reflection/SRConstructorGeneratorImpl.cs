/* 
 * File: SRConstructorGeneratorImpl.cs
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
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    // NOTE: Method や Constructor は Declaration と Generator が一段離れるイメージ。
    sealed class SRConstructorGeneratorImpl : SRConstructorDeclarationImpl, IConstructorGenerator, ISRMethodBaseGenerator
    {
        SRMethodBaseGeneratorImpl methodGen;

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder)
            : this(constructorBuilder, new ParameterBuilder[] { }, new FieldBuilder[] { })
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, ParameterBuilder[] parameterBuilders)
            : this(constructorBuilder, parameterBuilders, new FieldBuilder[] { })
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, FieldBuilder[] fieldBuilders)
            : this(constructorBuilder, new ParameterBuilder[] { }, fieldBuilders)
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
            : base(constructorBuilder)
        {
            this.methodGen = new SRMethodBaseGeneratorImpl(this, constructorBuilder, parameterBuilders, fieldBuilders);
        }

        public SRConstructorGeneratorImpl(ITypeGenerator declaringTypeGen, ConstructorBuilder constructorBuilder)
            : base(constructorBuilder)
        {
            this.methodGen = new SRMethodBaseGeneratorImpl(declaringTypeGen, this, constructorBuilder);
        }

        internal new ConstructorBuilder Source { get { return (ConstructorBuilder)base.Source; } }

        public new IMethodBodyGenerator Body
        {
            get { return methodGen.Body; }
        }

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return methodGen.Parameters; }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            return methodGen.AddPortableScopeItem(fieldInfo);
        }

        public new ITypeGenerator DeclaringType
        {
            get { return methodGen.DeclaringType; }
        }

        public IMethodBaseGenerator CreateInstance(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return methodGen.CreateInstance(name, attributes, returnType, parameterTypes);
        }

        public IMethodBaseGenerator ExpressBody(Action<ReflectiveMethodDesigner2> bodyExpression)
        {
            methodGen.ExpressBody(bodyExpression);
            return this;
        }

        public IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return methodGen.AddParameter(position, attributes, parameterName);
        }

        public ILGenerator GetILGenerator()
        {
            return Source.GetILGenerator();
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return Source.DefineParameter(position, attributes, parameterName);
        }



        public PortableScope CarryPortableScope()
        {
            throw new NotImplementedException();
        }


        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression)
        {
            throw new NotImplementedException();
        }


        public IMethodBaseGenerator ExpressBody2(Action<ReflectiveMethodDesigner2> bodyExpression, ITypeDeclaration returnType)
        {
            throw new NotImplementedException();
        }
    }
}

