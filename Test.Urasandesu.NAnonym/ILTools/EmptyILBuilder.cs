/* 
 * File: EmptyILBuilder.cs
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

using Urasandesu.NAnonym.Formulas;
using Urasandesu.NAnonym.ILTools;

namespace Test.Urasandesu.NAnonym.ILTools
{
    class EmptyILBuilder : ILBuilder
    {
        public EmptyILBuilder(IMethodBaseGenerator methodGen)
            : base(methodGen)
        {
        }

        public override void Visit(BinaryFormula formula)
        {
        }
        public override void Visit(AssignFormula formula)
        {
        }
        public override void Visit(NotEqualFormula formula)
        {
        }
        public override void Visit(AddFormula formula)
        {
        }
        public override void Visit(MultiplyFormula formula)
        {
        }
        public override void Visit(AndAlsoFormula formula)
        {
        }
        public override void Visit(EqualFormula formula)
        {
        }
        public override void Visit(ExclusiveOrFormula formula)
        {
        }
        public override void Visit(BlockFormula formula)
        {
        }
        public override void Visit(ConstantFormula formula)
        {
        }
        public override void Visit(Formula formula)
        {
        }
        public override void Visit(VariableFormula formula)
        {
        }
        public override void Visit(LocalFormula formula)
        {
        }
        public override void Visit(UnaryFormula formula)
        {
        }
        public override void Visit(ConvertFormula formula)
        {
        }
        public override void Visit(TypeAsFormula formula)
        {
        }
        public override void Visit(ConditionalFormula formula)
        {
        }
        public override void Visit(ReturnFormula formula)
        {
        }
        public override void Visit(CallFormula formula)
        {
        }
        public override void Visit(ReflectiveCallFormula formula)
        {
        }
        public override void Visit(NewArrayInitFormula formula)
        {
        }
        public override void Visit(NewFormula formula)
        {
        }
        public override void Visit(BaseNewFormula formula)
        {
        }
        public override void Visit(ReflectiveNewFormula formula)
        {
        }
        public override void Visit(MemberFormula formula)
        {
        }
        public override void Visit(PropertyFormula formula)
        {
        }
        public override void Visit(ReflectivePropertyFormula formula)
        {
        }
        public override void Visit(FieldFormula formula)
        {
        }
        public override void Visit(ReflectiveFieldFormula formula)
        {
        }
        public override void Visit(EndFormula formula)
        {
        }
    }
}
