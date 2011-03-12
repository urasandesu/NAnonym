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
        public virtual void Visit(BinaryFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Left != null) formula.Left.Accept(this);
            if (formula.Right != null) formula.Right.Accept(this);
        }
        public virtual void Visit(AssignFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(NotEqualFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(AddFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(MultiplyFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(AndAlsoFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(EqualFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(ExclusiveOrFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(BlockFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Formulas != null) formula.Formulas.Accept(this);
        }
        public virtual void Visit(ConstantFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
        }
        public virtual void Visit(Formula formula)
        {
            visitor.Visit(formula);
        }
        public virtual void Visit(VariableFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Resolved != null) formula.Resolved.Accept(this);
        }
        public virtual void Visit(LocalFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
        }
        public virtual void Visit(UnaryFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Operand != null) formula.Operand.Accept(this);
        }
        public virtual void Visit(ConvertFormula formula)
        {
            visitor.Visit(formula);
            Visit((UnaryFormula)formula);
        }
        public virtual void Visit(TypeAsFormula formula)
        {
            visitor.Visit(formula);
            Visit((UnaryFormula)formula);
        }
        public virtual void Visit(ConditionalFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Test != null) formula.Test.Accept(this);
            if (formula.IfTrue != null) formula.IfTrue.Accept(this);
            if (formula.IfFalse != null) formula.IfFalse.Accept(this);
        }
        public virtual void Visit(ReturnFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Body != null) formula.Body.Accept(this);
        }
        public virtual void Visit(CallFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Instance != null) formula.Instance.Accept(this);
            if (formula.Arguments != null) formula.Arguments.Accept(this);
        }
        public virtual void Visit(ReflectiveCallFormula formula)
        {
            visitor.Visit(formula);
            Visit((CallFormula)formula);
        }
        public virtual void Visit(NewArrayInitFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Formulas != null) formula.Formulas.Accept(this);
        }
        public virtual void Visit(NewFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Arguments != null) formula.Arguments.Accept(this);
        }
        public virtual void Visit(BaseNewFormula formula)
        {
            visitor.Visit(formula);
            Visit((NewFormula)formula);
        }
        public virtual void Visit(ReflectiveNewFormula formula)
        {
            visitor.Visit(formula);
            Visit((NewFormula)formula);
        }
        public virtual void Visit(MemberFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Instance != null) formula.Instance.Accept(this);
        }
        public virtual void Visit(PropertyFormula formula)
        {
            visitor.Visit(formula);
            Visit((MemberFormula)formula);
        }
        public virtual void Visit(ReflectivePropertyFormula formula)
        {
            visitor.Visit(formula);
            Visit((PropertyFormula)formula);
        }
        public virtual void Visit(FieldFormula formula)
        {
            visitor.Visit(formula);
            Visit((MemberFormula)formula);
        }
        public virtual void Visit(ReflectiveFieldFormula formula)
        {
            visitor.Visit(formula);
            Visit((FieldFormula)formula);
        }
        public virtual void Visit(EndFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
        }
    }
}
