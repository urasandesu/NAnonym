/* 
 * File: BinaryFormula.g.cs
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
    public abstract partial class BinaryFormula : Formula
    {

        protected override void InitializeForCodeGeneration()
        {
            base.InitializeForCodeGeneration();
            Method = default(IMethodDeclaration);
            Right = default(Formula);
            Left = default(Formula);
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
        public abstract string MethodToStringValueIfDefault { get; }
        public const string NameOfRight = "Right";
        Formula right;
        public Formula Right 
        { 
            get { return right; } 
            set 
            {
                SetFormula(NameOfRight, value, ref right);
            }
        }
        public const string NameOfLeft = "Left";
        Formula left;
        public Formula Left 
        { 
            get { return left; } 
            set 
            {
                SetFormula(NameOfLeft, value, ref left);
            }
        }


        protected override void PinCore()
        {
            Formula.Pin(Right);
            Formula.Pin(Left);
            base.PinCore();
        }


        public override void AppendTo(StringBuilder sb)
        {
            base.AppendTo(sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfMethod);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(Method, sb, MethodToStringValueIfDefault);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfRight);
            sb.Append(NodeToString.EndOfName);
            if (Right == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                Right.AppendWithStartEndTo(sb);
            }
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfLeft);
            sb.Append(NodeToString.EndOfName);
            if (Left == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                Left.AppendWithStartEndTo(sb);
            }
        }
    }
}

