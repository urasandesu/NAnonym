/* 
 * File: SRParameterGeneratorImpl.cs
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
using System.Reflection.Emit;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRParameterGeneratorImpl : SRParameterDeclarationImpl, IParameterGenerator
    {
        ParameterBuilder parameterBuilder;
        public SRParameterGeneratorImpl(ParameterBuilder parameterBuilder)
            : base(parameterBuilder.Name, ExportType(parameterBuilder), parameterBuilder.Position)
        {
            this.parameterBuilder = parameterBuilder;
        }

        static Type ExportType(ParameterBuilder parameterBuilder)
        {
            var parameterBuilderType = typeof(ParameterBuilder);
            var m_methodBuilderField = parameterBuilderType.GetField("m_methodBuilder", BindingFlags.NonPublic | BindingFlags.Instance);
            var m_methodBuilder = (MethodBuilder)m_methodBuilderField.GetValue(parameterBuilder);

            var methodBuilderType = typeof(MethodBuilder);
            var m_parameterTypesField = methodBuilderType.GetField("m_parameterTypes", BindingFlags.NonPublic | BindingFlags.Instance);
            var m_parameterTypes = (Type[])m_parameterTypesField.GetValue(m_methodBuilder);

            return m_parameterTypes[parameterBuilder.Position - 1];
        }

        internal ParameterBuilder ParameterBuilder
        {
            get { return parameterBuilder; }
        }

        #region IParameterGenerator メンバ

        public new ITypeGenerator ParameterType
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}

