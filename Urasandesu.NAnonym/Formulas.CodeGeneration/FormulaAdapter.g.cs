/* 
 * File: FormulaAdapter.g.cs
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

namespace Urasandesu.NAnonym.Formulas
{
    public abstract class FormulaAdapter : IFormulaVisitor
    {
        IFormulaVisitor visitor;
        public FormulaAdapter(IFormulaVisitor visitor)
        {
            Required.NotDefault(visitor, () => visitor);
            this.visitor = visitor;
        }
        public virtual Formula Visit(BinaryFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Left != null) formula.Left.Accept(this);
            if (formula.Right != null) formula.Right.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(AssignFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(NotEqualFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(AddFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(MultiplyFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(AndAlsoFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(EqualFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ExclusiveOrFormula formula)
        {
            Visit((BinaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(BlockFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Formulas != null) formula.Formulas.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ConstantFormula formula)
        {
            Visit((Formula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(Formula formula)
        {
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(VariableFormula formula)
        {
            Visit((Formula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(UnaryFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Operand != null) formula.Operand.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ConvertFormula formula)
        {
            Visit((UnaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(TypeAsFormula formula)
        {
            Visit((UnaryFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ConditionalFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Test != null) formula.Test.Accept(this);
            if (formula.IfTrue != null) formula.IfTrue.Accept(this);
            if (formula.IfFalse != null) formula.IfFalse.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ReturnFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Body != null) formula.Body.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(CallFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Instance != null) formula.Instance.Accept(this);
            if (formula.Arguments != null) formula.Arguments.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ReflectiveCallFormula formula)
        {
            Visit((CallFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(NewArrayInitFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Formulas != null) formula.Formulas.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(NewFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Arguments != null) formula.Arguments.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ReflectiveNewFormula formula)
        {
            Visit((NewFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(MemberFormula formula)
        {
            Visit((Formula)formula);
            if (formula.Instance != null) formula.Instance.Accept(this);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(PropertyFormula formula)
        {
            Visit((MemberFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ReflectivePropertyFormula formula)
        {
            Visit((PropertyFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(FieldFormula formula)
        {
            Visit((MemberFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(ReflectiveFieldFormula formula)
        {
            Visit((FieldFormula)formula);
            return visitor.Visit(formula);
        }
        public virtual Formula Visit(EndFormula formula)
        {
            Visit((Formula)formula);
            return visitor.Visit(formula);
        }
    }
}
