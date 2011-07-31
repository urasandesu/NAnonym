/* 
 * File: IFormulaVisitor.g.cs
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
    public interface IFormulaVisitor
    {
        void Visit(RightJoinBinaryFormula formula);
        void Visit(AssignFormula formula);
        void VisitAssignRight(AssignFormula formula, Formula right);
        void VisitAssignLeft(AssignFormula formula, Formula left);
        void Visit(LeftJoinBinaryFormula formula);
        void Visit(ArithmeticBinaryFormula formula);
        void Visit(LogicalBinaryFormula formula);
        void Visit(NotEqualFormula formula);
        void VisitNotEqualLeft(NotEqualFormula formula, Formula left);
        void VisitNotEqualRight(NotEqualFormula formula, Formula right);
        void Visit(AddFormula formula);
        void VisitAddLeft(AddFormula formula, Formula left);
        void VisitAddRight(AddFormula formula, Formula right);
        void Visit(MultiplyFormula formula);
        void VisitMultiplyLeft(MultiplyFormula formula, Formula left);
        void VisitMultiplyRight(MultiplyFormula formula, Formula right);
        void Visit(AndAlsoFormula formula);
        void VisitAndAlsoLeft(AndAlsoFormula formula, Formula left);
        void VisitAndAlsoRight(AndAlsoFormula formula, Formula right);
        void Visit(EqualFormula formula);
        void VisitEqualLeft(EqualFormula formula, Formula left);
        void VisitEqualRight(EqualFormula formula, Formula right);
        void Visit(ExclusiveOrFormula formula);
        void VisitExclusiveOrLeft(ExclusiveOrFormula formula, Formula left);
        void VisitExclusiveOrRight(ExclusiveOrFormula formula, Formula right);
        void Visit(LessThanFormula formula);
        void VisitLessThanLeft(LessThanFormula formula, Formula left);
        void VisitLessThanRight(LessThanFormula formula, Formula right);
        void Visit(LessThanOrEqualFormula formula);
        void VisitLessThanOrEqualLeft(LessThanOrEqualFormula formula, Formula left);
        void VisitLessThanOrEqualRight(LessThanOrEqualFormula formula, Formula right);
        void Visit(SubtractFormula formula);
        void VisitSubtractLeft(SubtractFormula formula, Formula left);
        void VisitSubtractRight(SubtractFormula formula, Formula right);
        void Visit(BlockFormula formula);
        void VisitBlockFormulas(BlockFormula formula, FormulaCollection<Formula> formulas);
        void Visit(ConstantFormula formula);
        void Visit(Node formula);
        void Visit(Formula formula);
        void Visit(VariableFormula formula);
        void VisitVariableResolved(VariableFormula formula, Node resolved);
        void Visit(LocalNode formula);
        void Visit(ArgumentFormula formula);
        void Visit(UnaryFormula formula);
        void Visit(ConvertFormula formula);
        void VisitConvertOperand(ConvertFormula formula, Formula operand);
        void Visit(TypeAsFormula formula);
        void VisitTypeAsOperand(TypeAsFormula formula, Formula operand);
        void Visit(ConditionalFormula formula);
        void VisitConditionalTest(ConditionalFormula formula, Formula test);
        void VisitConditionalIfTrue(ConditionalFormula formula, Formula ifTrue);
        void VisitConditionalIfFalse(ConditionalFormula formula, Formula ifFalse);
        void Visit(ReturnFormula formula);
        void VisitReturnBody(ReturnFormula formula, Formula body);
        void Visit(CallFormula formula);
        void VisitCallInstance(CallFormula formula, Formula instance);
        void VisitCallArguments(CallFormula formula, FormulaCollection<Formula> arguments);
        void Visit(ReflectiveCallFormula formula);
        void VisitReflectiveCallInstance(ReflectiveCallFormula formula, Formula instance);
        void VisitReflectiveCallArguments(ReflectiveCallFormula formula, FormulaCollection<Formula> arguments);
        void Visit(NewArrayInitFormula formula);
        void VisitNewArrayInitFormulas(NewArrayInitFormula formula, FormulaCollection<Formula> formulas);
        void Visit(NewFormula formula);
        void VisitNewArguments(NewFormula formula, FormulaCollection<Formula> arguments);
        void Visit(BaseNewFormula formula);
        void VisitBaseNewArguments(BaseNewFormula formula, FormulaCollection<Formula> arguments);
        void Visit(ReflectiveNewFormula formula);
        void VisitReflectiveNewArguments(ReflectiveNewFormula formula, FormulaCollection<Formula> arguments);
        void Visit(MemberFormula formula);
        void Visit(PropertyFormula formula);
        void VisitPropertyInstance(PropertyFormula formula, Formula instance);
        void Visit(ReflectivePropertyFormula formula);
        void VisitReflectivePropertyInstance(ReflectivePropertyFormula formula, Formula instance);
        void Visit(FieldFormula formula);
        void VisitFieldInstance(FieldFormula formula, Formula instance);
        void Visit(ReflectiveFieldFormula formula);
        void VisitReflectiveFieldInstance(ReflectiveFieldFormula formula, Formula instance);
        void Visit(EndFormula formula);
        void VisitEndEntryBlock(EndFormula formula, BlockFormula entryBlock);
        void Visit(InvokeFormula formula);
        void VisitInvokeDelegateOrLambda(InvokeFormula formula, Formula delegateOrLambda);
        void VisitInvokeArguments(InvokeFormula formula, FormulaCollection<Formula> arguments);
        void Visit(MethodPtrFormula formula);
        void VisitMethodPtrInstance(MethodPtrFormula formula, Formula instance);
    }
}
