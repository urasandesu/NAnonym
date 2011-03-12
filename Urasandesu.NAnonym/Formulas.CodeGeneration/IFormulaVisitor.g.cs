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
        void Visit(BinaryFormula formula);
        void VisitBinaryLeft(Formula formula);
        void VisitBinaryRight(Formula formula);
        void Visit(ArithmeticBinaryFormula formula);
        void Visit(LogicalBinaryFormula formula);
        void Visit(AssignFormula formula);
        void Visit(NotEqualFormula formula);
        void Visit(AddFormula formula);
        void Visit(MultiplyFormula formula);
        void Visit(AndAlsoFormula formula);
        void Visit(EqualFormula formula);
        void Visit(ExclusiveOrFormula formula);
        void Visit(LessThanFormula formula);
        void Visit(BlockFormula formula);
        void VisitBlockFormulas(FormulaCollection<Formula> formula);
        void Visit(ConstantFormula formula);
        void Visit(Formula formula);
        void Visit(VariableFormula formula);
        void VisitVariableResolved(Formula formula);
        void Visit(LocalFormula formula);
        void Visit(UnaryFormula formula);
        void VisitUnaryOperand(Formula formula);
        void Visit(ConvertFormula formula);
        void Visit(TypeAsFormula formula);
        void Visit(ConditionalFormula formula);
        void VisitConditionalTest(Formula formula);
        void VisitConditionalIfTrue(Formula formula);
        void VisitConditionalIfFalse(Formula formula);
        void Visit(ReturnFormula formula);
        void VisitReturnBody(Formula formula);
        void Visit(CallFormula formula);
        void VisitCallInstance(Formula formula);
        void VisitCallArguments(FormulaCollection<Formula> formula);
        void Visit(ReflectiveCallFormula formula);
        void Visit(NewArrayInitFormula formula);
        void VisitNewArrayInitFormulas(FormulaCollection<Formula> formula);
        void Visit(NewFormula formula);
        void VisitNewArguments(FormulaCollection<Formula> formula);
        void Visit(BaseNewFormula formula);
        void Visit(ReflectiveNewFormula formula);
        void Visit(MemberFormula formula);
        void VisitMemberInstance(Formula formula);
        void Visit(PropertyFormula formula);
        void Visit(ReflectivePropertyFormula formula);
        void Visit(FieldFormula formula);
        void Visit(ReflectiveFieldFormula formula);
        void Visit(EndFormula formula);
        void VisitEndBlock(BlockFormula formula);
    }
}
