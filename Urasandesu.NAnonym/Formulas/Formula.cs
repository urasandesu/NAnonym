/* 
 * File: Formula.cs
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
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Urasandesu.NAnonym.Formulas
{
    public abstract partial class Formula
    {
        protected virtual void Initialize()
        {
        }

        public bool IsPinned { get; private set; }

        protected void CheckCanModify(Formula formula)
        {
            if (formula != null && formula.IsPinned)
            {
                throw new NotSupportedException("This object has already pinned, so it can not modify.");
            }
        }

        protected void SetValue<T>(string propertyName, T value, ref T result)
        {
            CheckCanModify(this);
            result = value;
        }

        protected void SetFormula<TFormula>(string propertyName, TFormula formula, ref TFormula result) where TFormula : Formula
        {
            SetReferrer(result, null);
            SetValue(propertyName, formula, ref result);
            SetReferrer(result, this);
        }
        protected void SetFormulaAsValue<TFormula>(string propertyName, TFormula formula, ref TFormula result) where TFormula : Formula
        {
            SetValue(propertyName, formula, ref result);
        }

        protected void SetReferrer(Formula target, Formula referrer)
        {
            if (target != null)
            {
                CheckCanModify(target.Referrer);
                target.referrer = referrer;
            }
        }

        public static void Pin(Formula item)
        {
            if (item != null && !item.IsPinned)
            {
                item.IsPinned = true;
                item.PinCore();
            }
        }

        public static void AppendValueTo<TValue>(TValue value, StringBuilder sb)
        {
            AppendValueTo(value, sb, null);
        }

        public static void AppendValueTo<TValue>(TValue value, StringBuilder sb, string ifDefault)
        {
            if (!(value is ValueType) && value.IsDefault())
            {
                sb.Append(ifDefault == null ? "null" : ifDefault);
            }
            else
            {
                var s = value.ToString();
                var result = default(double);
                if (double.TryParse(s, out result))
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append("\"");
                    sb.Append(s);
                    sb.Append("\"");
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            AppendWithBracketTo(sb);
            return sb.ToString();
        }

        public virtual void AppendWithBracketTo(StringBuilder sb)
        {
            sb.Append("{");
            AppendTo(sb);
            sb.Append("}");
        }

        public abstract void Accept(IFormulaVisitor visitor);
    }
}

