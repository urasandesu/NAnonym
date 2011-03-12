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
        readonly protected IFormulaVisitor visitor;
        public FormulaAdapter(IFormulaVisitor visitor)
        {
            Required.NotDefault(visitor, () => visitor);
            this.visitor = visitor;
        }
        public virtual void Visit(BinaryFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Left != null) VisitBinaryLeft(formula.Left);
            if (formula.Right != null) VisitBinaryRight(formula.Right);
        }
        public virtual void VisitBinaryLeft(Formula formula)
		{
			visitor.VisitBinaryLeft(formula);
			formula.Accept(this);
		}
        public virtual void VisitBinaryRight(Formula formula)
		{
			visitor.VisitBinaryRight(formula);
			formula.Accept(this);
		}
        public virtual void Visit(ArithmeticBinaryFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(LogicalBinaryFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(AssignFormula formula)
        {
            visitor.Visit(formula);
            Visit((BinaryFormula)formula);
        }
        public virtual void Visit(NotEqualFormula formula)
        {
            visitor.Visit(formula);
            Visit((LogicalBinaryFormula)formula);
        }
        public virtual void Visit(AddFormula formula)
        {
            visitor.Visit(formula);
            Visit((ArithmeticBinaryFormula)formula);
        }
        public virtual void Visit(MultiplyFormula formula)
        {
            visitor.Visit(formula);
            Visit((ArithmeticBinaryFormula)formula);
        }
        public virtual void Visit(AndAlsoFormula formula)
        {
            visitor.Visit(formula);
            Visit((LogicalBinaryFormula)formula);
        }
        public virtual void Visit(EqualFormula formula)
        {
            visitor.Visit(formula);
            Visit((LogicalBinaryFormula)formula);
        }
        public virtual void Visit(ExclusiveOrFormula formula)
        {
            visitor.Visit(formula);
            Visit((ArithmeticBinaryFormula)formula);
        }
        public virtual void Visit(LessThanFormula formula)
        {
            visitor.Visit(formula);
            Visit((LogicalBinaryFormula)formula);
        }
        public virtual void Visit(BlockFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Formulas != null) VisitBlockFormulas(formula.Formulas);
        }
        public virtual void VisitBlockFormulas(FormulaCollection<Formula> formula)
		{
			visitor.VisitBlockFormulas(formula);
			formula.Accept(this);
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
            if (formula.Resolved != null) VisitVariableResolved(formula.Resolved);
        }
        public virtual void VisitVariableResolved(Formula formula)
		{
			visitor.VisitVariableResolved(formula);
			formula.Accept(this);
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
            if (formula.Operand != null) VisitUnaryOperand(formula.Operand);
        }
        public virtual void VisitUnaryOperand(Formula formula)
		{
			visitor.VisitUnaryOperand(formula);
			formula.Accept(this);
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
            if (formula.Test != null) VisitConditionalTest(formula.Test);
            if (formula.IfTrue != null) VisitConditionalIfTrue(formula.IfTrue);
            if (formula.IfFalse != null) VisitConditionalIfFalse(formula.IfFalse);
        }
        public virtual void VisitConditionalTest(Formula formula)
		{
			visitor.VisitConditionalTest(formula);
			formula.Accept(this);
		}
        public virtual void VisitConditionalIfTrue(Formula formula)
		{
			visitor.VisitConditionalIfTrue(formula);
			formula.Accept(this);
		}
        public virtual void VisitConditionalIfFalse(Formula formula)
		{
			visitor.VisitConditionalIfFalse(formula);
			formula.Accept(this);
		}
        public virtual void Visit(ReturnFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Body != null) VisitReturnBody(formula.Body);
        }
        public virtual void VisitReturnBody(Formula formula)
		{
			visitor.VisitReturnBody(formula);
			formula.Accept(this);
		}
        public virtual void Visit(CallFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Instance != null) VisitCallInstance(formula.Instance);
            if (formula.Arguments != null) VisitCallArguments(formula.Arguments);
        }
        public virtual void VisitCallInstance(Formula formula)
		{
			visitor.VisitCallInstance(formula);
			formula.Accept(this);
		}
        public virtual void VisitCallArguments(FormulaCollection<Formula> formula)
		{
			visitor.VisitCallArguments(formula);
			formula.Accept(this);
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
            if (formula.Formulas != null) VisitNewArrayInitFormulas(formula.Formulas);
        }
        public virtual void VisitNewArrayInitFormulas(FormulaCollection<Formula> formula)
		{
			visitor.VisitNewArrayInitFormulas(formula);
			formula.Accept(this);
		}
        public virtual void Visit(NewFormula formula)
        {
            visitor.Visit(formula);
            Visit((Formula)formula);
            if (formula.Arguments != null) VisitNewArguments(formula.Arguments);
        }
        public virtual void VisitNewArguments(FormulaCollection<Formula> formula)
		{
			visitor.VisitNewArguments(formula);
			formula.Accept(this);
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
            if (formula.Instance != null) VisitMemberInstance(formula.Instance);
        }
        public virtual void VisitMemberInstance(Formula formula)
		{
			visitor.VisitMemberInstance(formula);
			formula.Accept(this);
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
            if (formula.Block != null) VisitEndBlock(formula.Block);
        }
        public virtual void VisitEndBlock(BlockFormula formula)
		{
			visitor.VisitEndBlock(formula);
			formula.Accept(this);
		}
    }
}
