/* 
 * File: NoActionVisitor.g.cs
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

namespace Urasandesu.NAnonym.Formulas
{
    public class NoActionVisitor : IFormulaVisitor
    {
        public void Visit(BinaryFormula formula)
        {
        }
        public void Visit(ArithmeticBinaryFormula formula)
        {
        }
        public void Visit(LogicalBinaryFormula formula)
        {
        }
        public void Visit(AssignFormula formula)
        {
        }
        public void VisitAssignRight(AssignFormula formula, Formula right)
		{
		}
        public void VisitAssignLeft(AssignFormula formula, Formula left)
		{
		}
        public void Visit(NotEqualFormula formula)
        {
        }
        public void VisitNotEqualRight(NotEqualFormula formula, Formula right)
		{
		}
        public void VisitNotEqualLeft(NotEqualFormula formula, Formula left)
		{
		}
        public void Visit(AddFormula formula)
        {
        }
        public void VisitAddRight(AddFormula formula, Formula right)
		{
		}
        public void VisitAddLeft(AddFormula formula, Formula left)
		{
		}
        public void Visit(MultiplyFormula formula)
        {
        }
        public void VisitMultiplyRight(MultiplyFormula formula, Formula right)
		{
		}
        public void VisitMultiplyLeft(MultiplyFormula formula, Formula left)
		{
		}
        public void Visit(AndAlsoFormula formula)
        {
        }
        public void VisitAndAlsoRight(AndAlsoFormula formula, Formula right)
		{
		}
        public void VisitAndAlsoLeft(AndAlsoFormula formula, Formula left)
		{
		}
        public void Visit(EqualFormula formula)
        {
        }
        public void VisitEqualRight(EqualFormula formula, Formula right)
		{
		}
        public void VisitEqualLeft(EqualFormula formula, Formula left)
		{
		}
        public void Visit(ExclusiveOrFormula formula)
        {
        }
        public void VisitExclusiveOrRight(ExclusiveOrFormula formula, Formula right)
		{
		}
        public void VisitExclusiveOrLeft(ExclusiveOrFormula formula, Formula left)
		{
		}
        public void Visit(LessThanFormula formula)
        {
        }
        public void VisitLessThanRight(LessThanFormula formula, Formula right)
		{
		}
        public void VisitLessThanLeft(LessThanFormula formula, Formula left)
		{
		}
        public void Visit(BlockFormula formula)
        {
        }
        public void VisitBlockFormulas(BlockFormula formula, FormulaCollection<Formula> formulas)
		{
		}
        public void Visit(ConstantFormula formula)
        {
        }
        public void Visit(Node formula)
        {
        }
        public void Visit(Formula formula)
        {
        }
        public void Visit(VariableFormula formula)
        {
        }
        public void VisitVariableResolved(VariableFormula formula, Node resolved)
		{
		}
        public void Visit(LocalNode formula)
        {
        }
        public void Visit(ArgumentFormula formula)
        {
        }
        public void Visit(UnaryFormula formula)
        {
        }
        public void Visit(ConvertFormula formula)
        {
        }
        public void VisitConvertOperand(ConvertFormula formula, Formula operand)
		{
		}
        public void Visit(TypeAsFormula formula)
        {
        }
        public void VisitTypeAsOperand(TypeAsFormula formula, Formula operand)
		{
		}
        public void Visit(ConditionalFormula formula)
        {
        }
        public void VisitConditionalTest(ConditionalFormula formula, Formula test)
		{
		}
        public void VisitConditionalIfTrue(ConditionalFormula formula, Formula ifTrue)
		{
		}
        public void VisitConditionalIfFalse(ConditionalFormula formula, Formula ifFalse)
		{
		}
        public void Visit(ReturnFormula formula)
        {
        }
        public void VisitReturnBody(ReturnFormula formula, Formula body)
		{
		}
        public void Visit(CallFormula formula)
        {
        }
        public void VisitCallInstance(CallFormula formula, Formula instance)
		{
		}
        public void VisitCallArguments(CallFormula formula, FormulaCollection<Formula> arguments)
		{
		}
        public void Visit(ReflectiveCallFormula formula)
        {
        }
        public void VisitReflectiveCallInstance(ReflectiveCallFormula formula, Formula instance)
		{
		}
        public void VisitReflectiveCallArguments(ReflectiveCallFormula formula, FormulaCollection<Formula> arguments)
		{
		}
        public void Visit(NewArrayInitFormula formula)
        {
        }
        public void VisitNewArrayInitFormulas(NewArrayInitFormula formula, FormulaCollection<Formula> formulas)
		{
		}
        public void Visit(NewFormula formula)
        {
        }
        public void VisitNewArguments(NewFormula formula, FormulaCollection<Formula> arguments)
		{
		}
        public void Visit(BaseNewFormula formula)
        {
        }
        public void VisitBaseNewArguments(BaseNewFormula formula, FormulaCollection<Formula> arguments)
		{
		}
        public void Visit(ReflectiveNewFormula formula)
        {
        }
        public void VisitReflectiveNewArguments(ReflectiveNewFormula formula, FormulaCollection<Formula> arguments)
		{
		}
        public void Visit(MemberFormula formula)
        {
        }
        public void Visit(PropertyFormula formula)
        {
        }
        public void VisitPropertyInstance(PropertyFormula formula, Formula instance)
		{
		}
        public void Visit(ReflectivePropertyFormula formula)
        {
        }
        public void VisitReflectivePropertyInstance(ReflectivePropertyFormula formula, Formula instance)
		{
		}
        public void Visit(FieldFormula formula)
        {
        }
        public void VisitFieldInstance(FieldFormula formula, Formula instance)
		{
		}
        public void Visit(ReflectiveFieldFormula formula)
        {
        }
        public void VisitReflectiveFieldInstance(ReflectiveFieldFormula formula, Formula instance)
		{
		}
        public void Visit(EndFormula formula)
        {
        }
        public void VisitEndEntryBlock(EndFormula formula, BlockFormula entryBlock)
		{
		}
    }
}
