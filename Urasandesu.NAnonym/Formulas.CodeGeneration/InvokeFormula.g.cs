/* 
 * File: InvokeFormula.g.cs
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
    public partial class InvokeFormula : Formula
    {

        protected override void InitializeForCodeGeneration()
        {
            base.InitializeForCodeGeneration();
			NodeType = NodeType.Invoke;
            DelegateOrLambda = default(Formula);
            Method = default(IMethodDeclaration);
            Arguments = new FormulaCollection<Formula>();
        }

        public const string NameOfDelegateOrLambda = "DelegateOrLambda";
        Formula delegateOrLambda;
        public Formula DelegateOrLambda 
        { 
            get { return delegateOrLambda; } 
            set 
            {
                SetFormula(NameOfDelegateOrLambda, value, ref delegateOrLambda);
            }
        }
        public const string NameOfMethod = "Method";
        IMethodDeclaration method;
        public IMethodDeclaration Method 
        { 
            get { return method; } 
            set 
            {
                SetValue(NameOfMethod, value, ref method);
            }
        }
        public const string NameOfArguments = "Arguments";
        FormulaCollection<Formula> arguments;
        public FormulaCollection<Formula> Arguments 
        { 
            get { return arguments; } 
            set 
            {
                SetFormula(NameOfArguments, value, ref arguments);
            }
        }


        public override void Accept(IFormulaVisitor visitor)
        {
            visitor.Visit(this);
        }


        protected override void PinCore()
        {
            Formula.Pin(DelegateOrLambda);
            Formula.Pin(Arguments);
            base.PinCore();
        }


        public override void AppendTo(StringBuilder sb)
        {
            base.AppendTo(sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfDelegateOrLambda);
            sb.Append(NodeToString.EndOfName);
            if (DelegateOrLambda == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                DelegateOrLambda.AppendWithStartEndTo(sb);
            }
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfMethod);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(Method, sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfArguments);
            sb.Append(NodeToString.EndOfName);
            if (Arguments == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                Arguments.AppendWithStartEndTo(sb);
            }
        }
    }
}

