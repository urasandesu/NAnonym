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
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfTest);
            sb.Append("\": ");
            if (Test == null)
            {
                sb.Append("null");
            }
            else
            {
                Test.AppendWithBracketTo(sb);
            }
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfIfTrue);
            sb.Append("\": ");
            if (IfTrue == null)
            {
                sb.Append("null");
            }
            else
            {
                IfTrue.AppendWithBracketTo(sb);
            }
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfIfFalse);
            sb.Append("\": ");
            if (IfFalse == null)
            {
                sb.Append("null");
            }
            else
            {
                IfFalse.AppendWithBracketTo(sb);
            }
        }
    }
}

