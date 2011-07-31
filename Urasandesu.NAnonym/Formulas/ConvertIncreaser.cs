/* 
 * File: ConvertIncreaser.cs
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
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Formulas
{
    public class ConvertIncreaser : FormulaAdapter
    {
        public ConvertIncreaser(IFormulaVisitor visitor)
            : base(visitor)
        {
        }

        public override void Visit(LeftJoinBinaryFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Right, _ => formula.Left.TypeDeclaration, increased => formula.Right = increased);
        }

        public override void Visit(RightJoinBinaryFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Right, _ => formula.Left.TypeDeclaration, increased => formula.Right = increased);
        }

        public override void Visit(UnaryFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Operand, _ => _.TypeDeclaration, increased => formula.Operand = increased);
        }

        public override void Visit(ConditionalFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Test, _ => _.TypeDeclaration, increased => formula.Test = increased);
        }

        public override void Visit(ReturnFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Body, _ => _.TypeDeclaration, increased => formula.Body = increased);
        }

        public override void Visit(NewFormula formula)
        {
            base.Visit(formula);
            for (int i = 0; i < formula.Constructor.Parameters.Count; i++)
            {
                var expectedType = formula.Constructor.Parameters[i].ParameterType;
                IncreaseIfNecessary(formula.Arguments[i], _ => expectedType, increased => formula.Arguments[i] = increased);
            }
        }

        public override void Visit(CallFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Instance, _ => _.TypeDeclaration, increased => formula.Instance = increased);
            for (int i = 0; i < formula.Method.Parameters.Count; i++)
            {
                var expectedType = formula.Method.Parameters[i].ParameterType;
                IncreaseIfNecessary(formula.Arguments[i], _ => expectedType, increased => formula.Arguments[i] = increased);
            }
        }

        public override void Visit(InvokeFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.DelegateOrLambda, _ => _.TypeDeclaration, increased => formula.DelegateOrLambda = increased);
            for (int i = 0; i < formula.Method.Parameters.Count; i++)
            {
                var expectedType = formula.Method.Parameters[i].ParameterType;
                IncreaseIfNecessary(formula.Arguments[i], _ => expectedType, increased => formula.Arguments[i] = increased);
            }
        }

        public override void Visit(MethodPtrFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Instance, _ => _.TypeDeclaration, increased => formula.Instance = increased);
        }

        public override void Visit(NewArrayInitFormula formula)
        {
            base.Visit(formula);
            var expectedType = formula.TypeDeclaration.GetElementType();
            for (int i = 0; i < formula.Formulas.Count; i++)
            {
                IncreaseIfNecessary(formula.Formulas[i], _ => expectedType, increased => formula.Formulas[i] = increased);
            }
        }

        public override void Visit(MemberFormula formula)
        {
            base.Visit(formula);
            IncreaseIfNecessary(formula.Instance, _ => _.TypeDeclaration, increased => formula.Instance = increased);
        }

        void IncreaseIfNecessary(Formula formula, Func<Formula, ITypeDeclaration> getExpectedType, Action<Formula> increase)
        {
            if (formula != null)
            {
                var expectedType = getExpectedType(formula);
                if (!expectedType.IsAssignableExplicitlyFrom(formula.TypeDeclaration))
                {
                    var convert = new ConvertFormula(formula, expectedType);
                    increase(convert);
                }
            }
        }
    }
}

