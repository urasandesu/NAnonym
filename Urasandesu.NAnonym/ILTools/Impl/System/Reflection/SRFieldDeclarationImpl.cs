/* 
 * File: SRFieldDeclarationImpl.cs
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
    class SRFieldDeclarationImpl : SRMemberDeclarationImpl, IFieldDeclaration
    {
        FieldInfo fieldInfo;
        ITypeDeclaration fieldType;
        public SRFieldDeclarationImpl(FieldInfo fieldInfo)
            : this(fieldInfo, null)
        {
        }

        public SRFieldDeclarationImpl(FieldInfo fieldInfo, ITypeDeclaration declaringType)
            : base(fieldInfo, declaringType)
        {
            this.fieldInfo = fieldInfo;
        }

        internal FieldInfo FieldInfo { get { return fieldInfo; } }

        public ITypeDeclaration FieldType
        {
            get 
            {
                if (fieldType == null)
                {
                    fieldType = new SRTypeDeclarationImpl(fieldInfo.FieldType);
                }

                return fieldType; 
            }
        }

        public bool IsPublic
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { return fieldInfo.IsStatic; }
        }

        public object GetValue(object obj)
        {
            throw new NotImplementedException();
        }

        public void SetValue(object obj, object value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return fieldInfo.ToString();
        }
    }
}

