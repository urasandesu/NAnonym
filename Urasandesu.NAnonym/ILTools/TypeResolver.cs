/* 
 * File: TypeResolver.cs
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
using Urasandesu.NAnonym.Formulas;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class TypeResolver : FormulaAdapter
    {
        static readonly ITypeDeclaration VoidType = typeof(void).ToTypeDecl();
        static readonly ITypeDeclaration BoolType = typeof(bool).ToTypeDecl();
        readonly bool isReturnTypeVoid;
        BlockFormula entryPoint;
        ITypeDeclaration returnType;
        
        public TypeResolver(IFormulaVisitor visitor, ITypeDeclaration returnType)
            : base(visitor)
        {
            this.returnType = returnType;
            isReturnTypeVoid = returnType.Equals(VoidType);
        }

        public override void Visit(ArithmeticBinaryFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Left.TypeDeclaration;
        }

        public override void Visit(LogicalBinaryFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = BoolType;
        }

        public override void Visit(AssignFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Left.TypeDeclaration;
        }

        public override void Visit(BlockFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Result.TypeDeclaration;
        }

        public override void Visit(ConditionalFormula formula)
        {
            base.Visit(formula);
            if (formula.IfTrue != null && formula.IfFalse != null)
            {
                if (!formula.IfTrue.TypeDeclaration.Equals(formula.IfFalse.TypeDeclaration))
                {
                    throw new ReturnCheckException(string.Format(
                        "The IfTrue block type \"{0}\" is incompatible to IfFalse block type \"{1}\"", 
                        formula.IfTrue.TypeDeclaration, formula.IfFalse.TypeDeclaration));
                }
                formula.TypeDeclaration = formula.IfTrue.TypeDeclaration;
            }
            else if (formula.IfTrue != null)
            {
                formula.TypeDeclaration = formula.IfTrue.TypeDeclaration;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void Visit(CallFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Method == null ? null : formula.Method.ReturnType;
        }

        public override void Visit(FieldFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Member == null ? null : formula.Member.FieldType;
        }

        public override void Visit(NewFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Constructor == null ? null : formula.Constructor.DeclaringType;
        }

        public override void Visit(PropertyFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Member == null ? null : formula.Member.PropertyType;
        }

        public override void Visit(ReturnFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Body == null ? null : formula.Body.TypeDeclaration;
        }

        public override void Visit(EndFormula formula)
        {
            base.Visit(formula);
            formula.TypeDeclaration = formula.Block.TypeDeclaration;
        }

        //public override void Visit(BlockFormula formula)
        //{
        //    if (entryPoint == null)
        //    {
        //        entryPoint = formula;
        //        if (!isReturnTypeVoid && !returnType.IsAssignableFrom(entryPoint.TypeDeclaration))
        //        {
        //            throw new ReturnCheckException(string.Format("The return type \"{0}\" can't convert to \"{1}\"", entryPoint.TypeDeclaration, returnType));
        //        }
        //    }

        //    base.Visit(formula);
        //}

        //public override void Visit(ReturnFormula formula)
        //{
        //    if (isReturnTypeVoid)
        //    {
        //        if (!formula.TypeDeclaration.Equals(VoidType))
        //        {
        //            throw new ReturnCheckException(string.Format("The return type \"{0}\" can't convert to \"{1}\"", formula.TypeDeclaration, VoidType));
        //        }
        //    }

        //    base.Visit(formula);
        //}
    }
}
