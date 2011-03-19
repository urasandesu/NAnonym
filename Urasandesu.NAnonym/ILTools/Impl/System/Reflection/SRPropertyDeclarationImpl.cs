/* 
 * File: SRPropertyDeclarationImpl.cs
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
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRPropertyDeclarationImpl : SRMemberDeclarationImpl, IPropertyDeclaration
    {
        PropertyInfo propertyInfo;
        ITypeDeclaration propertyType;
        IMethodDeclaration getMethod;
        IMethodDeclaration setMethod;
        public SRPropertyDeclarationImpl(PropertyInfo propertyInfo)
            : this(propertyInfo, null)
        {
        }

        public SRPropertyDeclarationImpl(PropertyInfo propertyInfo, ITypeDeclaration declaringType)
            : base(propertyInfo, declaringType)
        {
            this.propertyInfo = propertyInfo;
        }

        public ITypeDeclaration PropertyType
        {
            get
            {
                if (propertyType == null)
                {
                    propertyType = new SRTypeDeclarationImpl(propertyInfo.PropertyType);
                }
                return propertyType;
            }
        }

        public override string ToString()
        {
            return propertyInfo.ToString();
        }


        public IMethodDeclaration GetMethod
        {
            get 
            {
                if (getMethod == null)
                {
                    getMethod = new SRMethodDeclarationImpl(propertyInfo.GetGetMethod());
                }
                return getMethod;
            }
        }


        public IMethodDeclaration SetMethod
        {
            get 
            {
                if (setMethod == null)
                {
                    setMethod = new SRMethodDeclarationImpl(propertyInfo.GetSetMethod());
                }
                return setMethod;
            }
        }
    }
}
