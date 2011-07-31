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
        public virtual void Visit(RightJoinBinaryFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(AssignFormula formula)
        {
            if (formula.Right != null) VisitAssignRight(formula, formula.Right);
            if (formula.Left != null) VisitAssignLeft(formula, formula.Left);
            Visit((RightJoinBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitAssignRight(AssignFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(AssignFormula)) return;
			VisitAssignRightCore(formula, right);
			visitor.VisitAssignRight(formula, right);
		}
        protected virtual void VisitAssignRightCore(AssignFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void VisitAssignLeft(AssignFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(AssignFormula)) return;
			VisitAssignLeftCore(formula, left);
			visitor.VisitAssignLeft(formula, left);
		}
        protected virtual void VisitAssignLeftCore(AssignFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void Visit(LeftJoinBinaryFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(ArithmeticBinaryFormula formula)
        {
            Visit((LeftJoinBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(LogicalBinaryFormula formula)
        {
            Visit((LeftJoinBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(NotEqualFormula formula)
        {
            if (formula.Left != null) VisitNotEqualLeft(formula, formula.Left);
            if (formula.Right != null) VisitNotEqualRight(formula, formula.Right);
            Visit((LogicalBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitNotEqualLeft(NotEqualFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(NotEqualFormula)) return;
			VisitNotEqualLeftCore(formula, left);
			visitor.VisitNotEqualLeft(formula, left);
		}
        protected virtual void VisitNotEqualLeftCore(NotEqualFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitNotEqualRight(NotEqualFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(NotEqualFormula)) return;
			VisitNotEqualRightCore(formula, right);
			visitor.VisitNotEqualRight(formula, right);
		}
        protected virtual void VisitNotEqualRightCore(NotEqualFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(AddFormula formula)
        {
            if (formula.Left != null) VisitAddLeft(formula, formula.Left);
            if (formula.Right != null) VisitAddRight(formula, formula.Right);
            Visit((ArithmeticBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitAddLeft(AddFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(AddFormula)) return;
			VisitAddLeftCore(formula, left);
			visitor.VisitAddLeft(formula, left);
		}
        protected virtual void VisitAddLeftCore(AddFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitAddRight(AddFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(AddFormula)) return;
			VisitAddRightCore(formula, right);
			visitor.VisitAddRight(formula, right);
		}
        protected virtual void VisitAddRightCore(AddFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(MultiplyFormula formula)
        {
            if (formula.Left != null) VisitMultiplyLeft(formula, formula.Left);
            if (formula.Right != null) VisitMultiplyRight(formula, formula.Right);
            Visit((ArithmeticBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitMultiplyLeft(MultiplyFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(MultiplyFormula)) return;
			VisitMultiplyLeftCore(formula, left);
			visitor.VisitMultiplyLeft(formula, left);
		}
        protected virtual void VisitMultiplyLeftCore(MultiplyFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitMultiplyRight(MultiplyFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(MultiplyFormula)) return;
			VisitMultiplyRightCore(formula, right);
			visitor.VisitMultiplyRight(formula, right);
		}
        protected virtual void VisitMultiplyRightCore(MultiplyFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(AndAlsoFormula formula)
        {
            if (formula.Left != null) VisitAndAlsoLeft(formula, formula.Left);
            if (formula.Right != null) VisitAndAlsoRight(formula, formula.Right);
            Visit((LogicalBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitAndAlsoLeft(AndAlsoFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(AndAlsoFormula)) return;
			VisitAndAlsoLeftCore(formula, left);
			visitor.VisitAndAlsoLeft(formula, left);
		}
        protected virtual void VisitAndAlsoLeftCore(AndAlsoFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitAndAlsoRight(AndAlsoFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(AndAlsoFormula)) return;
			VisitAndAlsoRightCore(formula, right);
			visitor.VisitAndAlsoRight(formula, right);
		}
        protected virtual void VisitAndAlsoRightCore(AndAlsoFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(EqualFormula formula)
        {
            if (formula.Left != null) VisitEqualLeft(formula, formula.Left);
            if (formula.Right != null) VisitEqualRight(formula, formula.Right);
            Visit((LogicalBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitEqualLeft(EqualFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(EqualFormula)) return;
			VisitEqualLeftCore(formula, left);
			visitor.VisitEqualLeft(formula, left);
		}
        protected virtual void VisitEqualLeftCore(EqualFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitEqualRight(EqualFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(EqualFormula)) return;
			VisitEqualRightCore(formula, right);
			visitor.VisitEqualRight(formula, right);
		}
        protected virtual void VisitEqualRightCore(EqualFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(ExclusiveOrFormula formula)
        {
            if (formula.Left != null) VisitExclusiveOrLeft(formula, formula.Left);
            if (formula.Right != null) VisitExclusiveOrRight(formula, formula.Right);
            Visit((ArithmeticBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitExclusiveOrLeft(ExclusiveOrFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(ExclusiveOrFormula)) return;
			VisitExclusiveOrLeftCore(formula, left);
			visitor.VisitExclusiveOrLeft(formula, left);
		}
        protected virtual void VisitExclusiveOrLeftCore(ExclusiveOrFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitExclusiveOrRight(ExclusiveOrFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(ExclusiveOrFormula)) return;
			VisitExclusiveOrRightCore(formula, right);
			visitor.VisitExclusiveOrRight(formula, right);
		}
        protected virtual void VisitExclusiveOrRightCore(ExclusiveOrFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(LessThanFormula formula)
        {
            if (formula.Left != null) VisitLessThanLeft(formula, formula.Left);
            if (formula.Right != null) VisitLessThanRight(formula, formula.Right);
            Visit((LogicalBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitLessThanLeft(LessThanFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(LessThanFormula)) return;
			VisitLessThanLeftCore(formula, left);
			visitor.VisitLessThanLeft(formula, left);
		}
        protected virtual void VisitLessThanLeftCore(LessThanFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitLessThanRight(LessThanFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(LessThanFormula)) return;
			VisitLessThanRightCore(formula, right);
			visitor.VisitLessThanRight(formula, right);
		}
        protected virtual void VisitLessThanRightCore(LessThanFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(LessThanOrEqualFormula formula)
        {
            if (formula.Left != null) VisitLessThanOrEqualLeft(formula, formula.Left);
            if (formula.Right != null) VisitLessThanOrEqualRight(formula, formula.Right);
            Visit((LogicalBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitLessThanOrEqualLeft(LessThanOrEqualFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(LessThanOrEqualFormula)) return;
			VisitLessThanOrEqualLeftCore(formula, left);
			visitor.VisitLessThanOrEqualLeft(formula, left);
		}
        protected virtual void VisitLessThanOrEqualLeftCore(LessThanOrEqualFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitLessThanOrEqualRight(LessThanOrEqualFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(LessThanOrEqualFormula)) return;
			VisitLessThanOrEqualRightCore(formula, right);
			visitor.VisitLessThanOrEqualRight(formula, right);
		}
        protected virtual void VisitLessThanOrEqualRightCore(LessThanOrEqualFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(SubtractFormula formula)
        {
            if (formula.Left != null) VisitSubtractLeft(formula, formula.Left);
            if (formula.Right != null) VisitSubtractRight(formula, formula.Right);
            Visit((ArithmeticBinaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitSubtractLeft(SubtractFormula formula, Formula left)
		{
		    if (formula.GetType() != typeof(SubtractFormula)) return;
			VisitSubtractLeftCore(formula, left);
			visitor.VisitSubtractLeft(formula, left);
		}
        protected virtual void VisitSubtractLeftCore(SubtractFormula formula, Formula left)
		{
			left.Accept(this);
		}
        public virtual void VisitSubtractRight(SubtractFormula formula, Formula right)
		{
		    if (formula.GetType() != typeof(SubtractFormula)) return;
			VisitSubtractRightCore(formula, right);
			visitor.VisitSubtractRight(formula, right);
		}
        protected virtual void VisitSubtractRightCore(SubtractFormula formula, Formula right)
		{
			right.Accept(this);
		}
        public virtual void Visit(BlockFormula formula)
        {
            if (formula.Formulas != null) VisitBlockFormulas(formula, formula.Formulas);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitBlockFormulas(BlockFormula formula, FormulaCollection<Formula> formulas)
		{
		    if (formula.GetType() != typeof(BlockFormula)) return;
			VisitBlockFormulasCore(formula, formulas);
			visitor.VisitBlockFormulas(formula, formulas);
		}
        protected virtual void VisitBlockFormulasCore(BlockFormula formula, FormulaCollection<Formula> formulas)
		{
			formulas.Accept(this);
		}
        public virtual void Visit(ConstantFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(Node formula)
        {
            visitor.Visit(formula);
        }
        public virtual void Visit(Formula formula)
        {
            Visit((Node)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(VariableFormula formula)
        {
            if (formula.Resolved != null) VisitVariableResolved(formula, formula.Resolved);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitVariableResolved(VariableFormula formula, Node resolved)
		{
		    if (formula.GetType() != typeof(VariableFormula)) return;
			VisitVariableResolvedCore(formula, resolved);
			visitor.VisitVariableResolved(formula, resolved);
		}
        protected virtual void VisitVariableResolvedCore(VariableFormula formula, Node resolved)
		{
			resolved.Accept(this);
		}
        public virtual void Visit(LocalNode formula)
        {
            Visit((Node)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(ArgumentFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(UnaryFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(ConvertFormula formula)
        {
            if (formula.Operand != null) VisitConvertOperand(formula, formula.Operand);
            Visit((UnaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitConvertOperand(ConvertFormula formula, Formula operand)
		{
		    if (formula.GetType() != typeof(ConvertFormula)) return;
			VisitConvertOperandCore(formula, operand);
			visitor.VisitConvertOperand(formula, operand);
		}
        protected virtual void VisitConvertOperandCore(ConvertFormula formula, Formula operand)
		{
			operand.Accept(this);
		}
        public virtual void Visit(TypeAsFormula formula)
        {
            if (formula.Operand != null) VisitTypeAsOperand(formula, formula.Operand);
            Visit((UnaryFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitTypeAsOperand(TypeAsFormula formula, Formula operand)
		{
		    if (formula.GetType() != typeof(TypeAsFormula)) return;
			VisitTypeAsOperandCore(formula, operand);
			visitor.VisitTypeAsOperand(formula, operand);
		}
        protected virtual void VisitTypeAsOperandCore(TypeAsFormula formula, Formula operand)
		{
			operand.Accept(this);
		}
        public virtual void Visit(ConditionalFormula formula)
        {
            if (formula.Test != null) VisitConditionalTest(formula, formula.Test);
            if (formula.IfTrue != null) VisitConditionalIfTrue(formula, formula.IfTrue);
            if (formula.IfFalse != null) VisitConditionalIfFalse(formula, formula.IfFalse);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitConditionalTest(ConditionalFormula formula, Formula test)
		{
		    if (formula.GetType() != typeof(ConditionalFormula)) return;
			VisitConditionalTestCore(formula, test);
			visitor.VisitConditionalTest(formula, test);
		}
        protected virtual void VisitConditionalTestCore(ConditionalFormula formula, Formula test)
		{
			test.Accept(this);
		}
        public virtual void VisitConditionalIfTrue(ConditionalFormula formula, Formula ifTrue)
		{
		    if (formula.GetType() != typeof(ConditionalFormula)) return;
			VisitConditionalIfTrueCore(formula, ifTrue);
			visitor.VisitConditionalIfTrue(formula, ifTrue);
		}
        protected virtual void VisitConditionalIfTrueCore(ConditionalFormula formula, Formula ifTrue)
		{
			ifTrue.Accept(this);
		}
        public virtual void VisitConditionalIfFalse(ConditionalFormula formula, Formula ifFalse)
		{
		    if (formula.GetType() != typeof(ConditionalFormula)) return;
			VisitConditionalIfFalseCore(formula, ifFalse);
			visitor.VisitConditionalIfFalse(formula, ifFalse);
		}
        protected virtual void VisitConditionalIfFalseCore(ConditionalFormula formula, Formula ifFalse)
		{
			ifFalse.Accept(this);
		}
        public virtual void Visit(ReturnFormula formula)
        {
            if (formula.Body != null) VisitReturnBody(formula, formula.Body);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitReturnBody(ReturnFormula formula, Formula body)
		{
		    if (formula.GetType() != typeof(ReturnFormula)) return;
			VisitReturnBodyCore(formula, body);
			visitor.VisitReturnBody(formula, body);
		}
        protected virtual void VisitReturnBodyCore(ReturnFormula formula, Formula body)
		{
			body.Accept(this);
		}
        public virtual void Visit(CallFormula formula)
        {
            if (formula.Instance != null) VisitCallInstance(formula, formula.Instance);
            if (formula.Arguments != null) VisitCallArguments(formula, formula.Arguments);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitCallInstance(CallFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(CallFormula)) return;
			VisitCallInstanceCore(formula, instance);
			visitor.VisitCallInstance(formula, instance);
		}
        protected virtual void VisitCallInstanceCore(CallFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void VisitCallArguments(CallFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(CallFormula)) return;
			VisitCallArgumentsCore(formula, arguments);
			visitor.VisitCallArguments(formula, arguments);
		}
        protected virtual void VisitCallArgumentsCore(CallFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(ReflectiveCallFormula formula)
        {
            if (formula.Instance != null) VisitReflectiveCallInstance(formula, formula.Instance);
            if (formula.Arguments != null) VisitReflectiveCallArguments(formula, formula.Arguments);
            Visit((CallFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitReflectiveCallInstance(ReflectiveCallFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(ReflectiveCallFormula)) return;
			VisitReflectiveCallInstanceCore(formula, instance);
			visitor.VisitReflectiveCallInstance(formula, instance);
		}
        protected virtual void VisitReflectiveCallInstanceCore(ReflectiveCallFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void VisitReflectiveCallArguments(ReflectiveCallFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(ReflectiveCallFormula)) return;
			VisitReflectiveCallArgumentsCore(formula, arguments);
			visitor.VisitReflectiveCallArguments(formula, arguments);
		}
        protected virtual void VisitReflectiveCallArgumentsCore(ReflectiveCallFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(NewArrayInitFormula formula)
        {
            if (formula.Formulas != null) VisitNewArrayInitFormulas(formula, formula.Formulas);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitNewArrayInitFormulas(NewArrayInitFormula formula, FormulaCollection<Formula> formulas)
		{
		    if (formula.GetType() != typeof(NewArrayInitFormula)) return;
			VisitNewArrayInitFormulasCore(formula, formulas);
			visitor.VisitNewArrayInitFormulas(formula, formulas);
		}
        protected virtual void VisitNewArrayInitFormulasCore(NewArrayInitFormula formula, FormulaCollection<Formula> formulas)
		{
			formulas.Accept(this);
		}
        public virtual void Visit(NewFormula formula)
        {
            if (formula.Arguments != null) VisitNewArguments(formula, formula.Arguments);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitNewArguments(NewFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(NewFormula)) return;
			VisitNewArgumentsCore(formula, arguments);
			visitor.VisitNewArguments(formula, arguments);
		}
        protected virtual void VisitNewArgumentsCore(NewFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(BaseNewFormula formula)
        {
            if (formula.Arguments != null) VisitBaseNewArguments(formula, formula.Arguments);
            Visit((NewFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitBaseNewArguments(BaseNewFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(BaseNewFormula)) return;
			VisitBaseNewArgumentsCore(formula, arguments);
			visitor.VisitBaseNewArguments(formula, arguments);
		}
        protected virtual void VisitBaseNewArgumentsCore(BaseNewFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(ReflectiveNewFormula formula)
        {
            if (formula.Arguments != null) VisitReflectiveNewArguments(formula, formula.Arguments);
            Visit((NewFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitReflectiveNewArguments(ReflectiveNewFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(ReflectiveNewFormula)) return;
			VisitReflectiveNewArgumentsCore(formula, arguments);
			visitor.VisitReflectiveNewArguments(formula, arguments);
		}
        protected virtual void VisitReflectiveNewArgumentsCore(ReflectiveNewFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(MemberFormula formula)
        {
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void Visit(PropertyFormula formula)
        {
            if (formula.Instance != null) VisitPropertyInstance(formula, formula.Instance);
            Visit((MemberFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitPropertyInstance(PropertyFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(PropertyFormula)) return;
			VisitPropertyInstanceCore(formula, instance);
			visitor.VisitPropertyInstance(formula, instance);
		}
        protected virtual void VisitPropertyInstanceCore(PropertyFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void Visit(ReflectivePropertyFormula formula)
        {
            if (formula.Instance != null) VisitReflectivePropertyInstance(formula, formula.Instance);
            Visit((PropertyFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitReflectivePropertyInstance(ReflectivePropertyFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(ReflectivePropertyFormula)) return;
			VisitReflectivePropertyInstanceCore(formula, instance);
			visitor.VisitReflectivePropertyInstance(formula, instance);
		}
        protected virtual void VisitReflectivePropertyInstanceCore(ReflectivePropertyFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void Visit(FieldFormula formula)
        {
            if (formula.Instance != null) VisitFieldInstance(formula, formula.Instance);
            Visit((MemberFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitFieldInstance(FieldFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(FieldFormula)) return;
			VisitFieldInstanceCore(formula, instance);
			visitor.VisitFieldInstance(formula, instance);
		}
        protected virtual void VisitFieldInstanceCore(FieldFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void Visit(ReflectiveFieldFormula formula)
        {
            if (formula.Instance != null) VisitReflectiveFieldInstance(formula, formula.Instance);
            Visit((FieldFormula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitReflectiveFieldInstance(ReflectiveFieldFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(ReflectiveFieldFormula)) return;
			VisitReflectiveFieldInstanceCore(formula, instance);
			visitor.VisitReflectiveFieldInstance(formula, instance);
		}
        protected virtual void VisitReflectiveFieldInstanceCore(ReflectiveFieldFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
        public virtual void Visit(EndFormula formula)
        {
            if (formula.EntryBlock != null) VisitEndEntryBlock(formula, formula.EntryBlock);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitEndEntryBlock(EndFormula formula, BlockFormula entryBlock)
		{
		    if (formula.GetType() != typeof(EndFormula)) return;
			VisitEndEntryBlockCore(formula, entryBlock);
			visitor.VisitEndEntryBlock(formula, entryBlock);
		}
        protected virtual void VisitEndEntryBlockCore(EndFormula formula, BlockFormula entryBlock)
		{
			entryBlock.Accept(this);
		}
        public virtual void Visit(InvokeFormula formula)
        {
            if (formula.DelegateOrLambda != null) VisitInvokeDelegateOrLambda(formula, formula.DelegateOrLambda);
            if (formula.Arguments != null) VisitInvokeArguments(formula, formula.Arguments);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitInvokeDelegateOrLambda(InvokeFormula formula, Formula delegateOrLambda)
		{
		    if (formula.GetType() != typeof(InvokeFormula)) return;
			VisitInvokeDelegateOrLambdaCore(formula, delegateOrLambda);
			visitor.VisitInvokeDelegateOrLambda(formula, delegateOrLambda);
		}
        protected virtual void VisitInvokeDelegateOrLambdaCore(InvokeFormula formula, Formula delegateOrLambda)
		{
			delegateOrLambda.Accept(this);
		}
        public virtual void VisitInvokeArguments(InvokeFormula formula, FormulaCollection<Formula> arguments)
		{
		    if (formula.GetType() != typeof(InvokeFormula)) return;
			VisitInvokeArgumentsCore(formula, arguments);
			visitor.VisitInvokeArguments(formula, arguments);
		}
        protected virtual void VisitInvokeArgumentsCore(InvokeFormula formula, FormulaCollection<Formula> arguments)
		{
			arguments.Accept(this);
		}
        public virtual void Visit(MethodPtrFormula formula)
        {
            if (formula.Instance != null) VisitMethodPtrInstance(formula, formula.Instance);
            Visit((Formula)formula);
            visitor.Visit(formula);
        }
        public virtual void VisitMethodPtrInstance(MethodPtrFormula formula, Formula instance)
		{
		    if (formula.GetType() != typeof(MethodPtrFormula)) return;
			VisitMethodPtrInstanceCore(formula, instance);
			visitor.VisitMethodPtrInstance(formula, instance);
		}
        protected virtual void VisitMethodPtrInstanceCore(MethodPtrFormula formula, Formula instance)
		{
			instance.Accept(this);
		}
    }
}
