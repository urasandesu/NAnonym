/* 
 * File: SRParameterDeclarationImpl.cs
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

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRParameterDeclarationImpl : IParameterDeclaration
    {
        ParameterInfo parameterInfo;
        public SRParameterDeclarationImpl(ParameterInfo parameterInfo)
            : this(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.Position)
        {
            this.parameterInfo = parameterInfo;
        }

        ITypeDeclaration typeDecl;
        string name;
        Type type;
        int position;
        public SRParameterDeclarationImpl(string name, Type type, int position)
        {
            this.name = name;
            this.type = type;
            this.position = position;
            typeDecl = new SRTypeDeclarationImpl(type);
        }

        #region IParameterDeclaration メンバ

        public string Name
        {
            get { return name; }
        }

        public ITypeDeclaration ParameterType
        {
            get { return typeDecl; }
        }

        public int Position
        {
            get { return position; }
        }

        #endregion
    }
}

