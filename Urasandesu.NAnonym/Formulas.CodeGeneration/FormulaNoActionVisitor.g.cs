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
        public void Visit(BinaryFormula formula)
        {
        }
        public void Visit(AssignFormula formula)
        {
        }
        public void Visit(NotEqualFormula formula)
        {
        }
        public void Visit(AddFormula formula)
        {
        }
        public void Visit(MultiplyFormula formula)
        {
        }
        public void Visit(AndAlsoFormula formula)
        {
        }
        public void Visit(EqualFormula formula)
        {
        }
        public void Visit(ExclusiveOrFormula formula)
        {
        }
        public void Visit(BlockFormula formula)
        {
        }
        public void Visit(ConstantFormula formula)
        {
        }
        public void Visit(Formula formula)
        {
        }
        public void Visit(VariableFormula formula)
        {
        }
        public void Visit(LocalFormula formula)
        {
        }
        public void Visit(UnaryFormula formula)
        {
        }
        public void Visit(ConvertFormula formula)
        {
        }
        public void Visit(TypeAsFormula formula)
        {
        }
        public void Visit(ConditionalFormula formula)
        {
        }
        public void Visit(ReturnFormula formula)
        {
        }
        public void Visit(CallFormula formula)
        {
        }
        public void Visit(ReflectiveCallFormula formula)
        {
        }
        public void Visit(NewArrayInitFormula formula)
        {
        }
        public void Visit(NewFormula formula)
        {
        }
        public void Visit(BaseNewFormula formula)
        {
        }
        public void Visit(ReflectiveNewFormula formula)
        {
        }
        public void Visit(MemberFormula formula)
        {
        }
        public void Visit(PropertyFormula formula)
        {
        }
        public void Visit(ReflectivePropertyFormula formula)
        {
        }
        public void Visit(FieldFormula formula)
        {
        }
        public void Visit(ReflectiveFieldFormula formula)
        {
        }
        public void Visit(EndFormula formula)
        {
        }
    }
}
