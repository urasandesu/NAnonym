/* 
 * File: ConditionalFormula.g.cs
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
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.Formulas
{
    public partial class ConditionalFormula : Formula
    {

        protected override void InitializeForCodeGeneration()
        {
            base.InitializeForCodeGeneration();
			NodeType = NodeType.Conditional;
            Test = default(Formula);
            IfTrue = default(Formula);
            IfFalse = default(Formula);
            Label1 = default(ILabelDeclaration);
            Label2 = default(ILabelDeclaration);
        }

        public const string NameOfTest = "Test";
        Formula test;
        public Formula Test 
        { 
            get { return test; } 
            set 
            {
                SetFormula(NameOfTest, value, ref test);
            }
        }
        public const string NameOfIfTrue = "IfTrue";
        Formula ifTrue;
        public Formula IfTrue 
        { 
            get { return ifTrue; } 
            set 
            {
                SetFormula(NameOfIfTrue, value, ref ifTrue);
            }
        }
        public const string NameOfIfFalse = "IfFalse";
        Formula ifFalse;
        public Formula IfFalse 
        { 
            get { return ifFalse; } 
            set 
            {
                SetFormula(NameOfIfFalse, value, ref ifFalse);
            }
        }
        public const string NameOfLabel1 = "Label1";
        ILabelDeclaration label1;
        public ILabelDeclaration Label1 
        { 
            get { return label1; } 
            set 
            {
                SetValue(NameOfLabel1, value, ref label1);
            }
        }
        public const string NameOfLabel2 = "Label2";
        ILabelDeclaration label2;
        public ILabelDeclaration Label2 
        { 
            get { return label2; } 
            set 
            {
                SetValue(NameOfLabel2, value, ref label2);
            }
        }


        public override void Accept(IFormulaVisitor visitor)
        {
            visitor.Visit(this);
        }


        protected override void PinCore()
        {
            Formula.Pin(Test);
            Formula.Pin(IfTrue);
            Formula.Pin(IfFalse);
            base.PinCore();
        }


        public override void AppendTo(StringBuilder sb)
        {
            base.AppendTo(sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfTest);
            sb.Append(NodeToString.EndOfName);
            if (Test == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                Test.AppendWithStartEndTo(sb);
            }
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfIfTrue);
            sb.Append(NodeToString.EndOfName);
            if (IfTrue == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                IfTrue.AppendWithStartEndTo(sb);
            }
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfIfFalse);
            sb.Append(NodeToString.EndOfName);
            if (IfFalse == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                IfFalse.AppendWithStartEndTo(sb);
            }
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfLabel1);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(Label1, sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfLabel2);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(Label2, sb);
        }
    }
}

