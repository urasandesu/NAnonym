/* 
 * File: ReflectiveMethodDesigner.cs
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
    public class ReflectiveMethodDesigner2
    {
        ExpressionToFormulaState state;
        public ReflectiveMethodDesigner2()
        {
            state = new ExpressionToFormulaState();
        }

        public void Eval(Expression<Action> exp)
        {
            if (state.IsEnded)
            {
                throw new NotSupportedException("The internal DSL has already ended.");
            }
            exp.Body.EvalTo(state);
            if (state.IsEnded)
            {
                OnEvalEnded(state.EntryPoint);
                Formula.Pin(state.EntryPoint);
            }
        }

        protected virtual void OnEvalEnded(EndFormula entryPoint)
        {
            // Resolve the reference of types, variables and members.
            {
                var visitor = default(IFormulaVisitor);
                visitor = new FormulaNoActionVisitor();
                visitor = new VariableResolver(visitor);
                visitor = new TypeResolver(visitor, GetReturnType());
                entryPoint.Accept(visitor);
            }

            // Optimize the intermediate formula.
            {
                var visitor = default(IFormulaVisitor);
                visitor = new FormulaNoActionVisitor();
                visitor = new ConvertDecreaser(visitor);
                visitor = new ConvertIncreaser(visitor);
                entryPoint.Accept(visitor);
            }

            // Build IL.
            if (ILBuilder != null)
            {
                entryPoint.Accept(ILBuilder);
            }
        }

        public ILBuilder ILBuilder { get; set; }
        public ITypeDeclaration GetReturnType()
        {
            var methodGen = default(IMethodGenerator);
            if (ILBuilder == null || (methodGen = ILBuilder.MethodGenerator as IMethodGenerator) == null)
            {
                return typeof(void).ToTypeDecl();
            }
            else
            {
                return methodGen.ReturnType;
            }
        }

        public string Dump()
        {
            return state.EntryPoint.ToString();
        }
    }
}
