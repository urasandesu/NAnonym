/* 
 * File: MCFieldDeclarationImpl.cs
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
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCFieldDeclarationImpl : MCMemberDeclarationImpl, IFieldDeclaration
    {
        readonly FieldReference fieldRef;
        readonly FieldDefinition fieldDef;

        MCTypeDeclarationImpl fieldTypeDecl;

        public MCFieldDeclarationImpl(FieldReference fieldRef)
            : base(fieldRef)
        {
            this.fieldRef = fieldRef;
            fieldDef = fieldRef.Resolve();
            fieldTypeDecl = new MCTypeDeclarationImpl(fieldRef.FieldType);
        }

        internal FieldReference FieldRef { get { return fieldRef; } }

        public bool IsStatic
        {
            get { return fieldDef.IsStatic; }
        }

        public bool IsPublic
        {
            get { return fieldDef.IsPublic; }
        }

        public object GetValue(object obj)
        {
            throw new NotSupportedException();
        }

        public void SetValue(object obj, object value)
        {
            throw new NotSupportedException();
        }

        ITypeDeclaration IFieldDeclaration.FieldType
        {
            get { return fieldTypeDecl; }
        }
    }
}

