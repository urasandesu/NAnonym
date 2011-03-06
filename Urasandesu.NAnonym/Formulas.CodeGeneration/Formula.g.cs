/* 
 * File: Formula.g.cs
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
using Urasandesu.NAnonym.ILTools;
using System.ComponentModel;

namespace Urasandesu.NAnonym.Formulas
{
    public abstract partial class Formula : INotifyPropertyChanged
    {
        public Formula()
            : base()
        {
            InitializeForCodeGeneration();
            Initialize();
        }

        protected virtual void InitializeForCodeGeneration()
        {
            Referrer = default(Formula);
            NodeType = NodeType.None;
            TypeDeclaration = default(ITypeDeclaration);
        }

        public const string NameOfReferrer = "Referrer";
        Formula referrer;
        public Formula Referrer 
        { 
            get { return referrer; } 
            set 
            {
                SetFormula(NameOfReferrer, value, ref referrer);
            }
        }
        public const string NameOfNodeType = "NodeType";
        NodeType nodeType;
        public NodeType NodeType 
        { 
            get { return nodeType; } 
            set 
            {
                SetValue(NameOfNodeType, value, ref nodeType);
            }
        }
        public const string NameOfTypeDeclaration = "TypeDeclaration";
        ITypeDeclaration typeDeclaration;
        public ITypeDeclaration TypeDeclaration 
        { 
            get { return typeDeclaration; } 
            set 
            {
                SetValue(NameOfTypeDeclaration, value, ref typeDeclaration);
            }
        }


        protected virtual void PinCore()
        {
            Formula.Pin(Referrer);
        }


        public virtual void AppendTo(StringBuilder sb)
        {
            sb.Append("\"");
            sb.Append(NameOfNodeType);
            sb.Append("\": ");
            AppendValueTo(NodeType, sb);
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfTypeDeclaration);
            sb.Append("\": ");
            AppendValueTo(TypeDeclaration, sb);
        }
    }
}

