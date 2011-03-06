/* 
 * File: FormulaNoActionVisitor.g.cs
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
    public class FormulaNoActionVisitor : IFormulaVisitor
    {
        public Formula Visit(BinaryFormula formula)
        {
            return formula;
        }
        public Formula Visit(AssignFormula formula)
        {
            return formula;
        }
        public Formula Visit(NotEqualFormula formula)
        {
            return formula;
        }
        public Formula Visit(AddFormula formula)
        {
            return formula;
        }
        public Formula Visit(MultiplyFormula formula)
        {
            return formula;
        }
        public Formula Visit(AndAlsoFormula formula)
        {
            return formula;
        }
        public Formula Visit(EqualFormula formula)
        {
            return formula;
        }
        public Formula Visit(ExclusiveOrFormula formula)
        {
            return formula;
        }
        public Formula Visit(BlockFormula formula)
        {
            return formula;
        }
        public Formula Visit(ConstantFormula formula)
        {
            return formula;
        }
        public Formula Visit(Formula formula)
        {
            return formula;
        }
        public Formula Visit(VariableFormula formula)
        {
            return formula;
        }
        public Formula Visit(UnaryFormula formula)
        {
            return formula;
        }
        public Formula Visit(ConvertFormula formula)
        {
            return formula;
        }
        public Formula Visit(TypeAsFormula formula)
        {
            return formula;
        }
        public Formula Visit(ConditionalFormula formula)
        {
            return formula;
        }
        public Formula Visit(ReturnFormula formula)
        {
            return formula;
        }
        public Formula Visit(CallFormula formula)
        {
            return formula;
        }
        public Formula Visit(ReflectiveCallFormula formula)
        {
            return formula;
        }
        public Formula Visit(NewArrayInitFormula formula)
        {
            return formula;
        }
        public Formula Visit(NewFormula formula)
        {
            return formula;
        }
        public Formula Visit(ReflectiveNewFormula formula)
        {
            return formula;
        }
        public Formula Visit(MemberFormula formula)
        {
            return formula;
        }
        public Formula Visit(PropertyFormula formula)
        {
            return formula;
        }
        public Formula Visit(ReflectivePropertyFormula formula)
        {
            return formula;
        }
        public Formula Visit(FieldFormula formula)
        {
            return formula;
        }
        public Formula Visit(ReflectiveFieldFormula formula)
        {
            return formula;
        }
        public Formula Visit(EndFormula formula)
        {
            return formula;
        }
    }
}
