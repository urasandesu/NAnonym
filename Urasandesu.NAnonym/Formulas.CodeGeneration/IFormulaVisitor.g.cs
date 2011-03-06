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
        Formula Visit(BinaryFormula formula);
        Formula Visit(AssignFormula formula);
        Formula Visit(NotEqualFormula formula);
        Formula Visit(AddFormula formula);
        Formula Visit(MultiplyFormula formula);
        Formula Visit(AndAlsoFormula formula);
        Formula Visit(EqualFormula formula);
        Formula Visit(ExclusiveOrFormula formula);
        Formula Visit(BlockFormula formula);
        Formula Visit(ConstantFormula formula);
        Formula Visit(Formula formula);
        Formula Visit(VariableFormula formula);
        Formula Visit(UnaryFormula formula);
        Formula Visit(ConvertFormula formula);
        Formula Visit(TypeAsFormula formula);
        Formula Visit(ConditionalFormula formula);
        Formula Visit(ReturnFormula formula);
        Formula Visit(CallFormula formula);
        Formula Visit(ReflectiveCallFormula formula);
        Formula Visit(NewArrayInitFormula formula);
        Formula Visit(NewFormula formula);
        Formula Visit(ReflectiveNewFormula formula);
        Formula Visit(MemberFormula formula);
        Formula Visit(PropertyFormula formula);
        Formula Visit(ReflectivePropertyFormula formula);
        Formula Visit(FieldFormula formula);
        Formula Visit(ReflectiveFieldFormula formula);
        Formula Visit(EndFormula formula);
    }
}
