/* 
 * File: ReflectiveMethodDesigner2.cs
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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Urasandesu.NAnonym.Formulas;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using SRE = System.Reflection.Emit;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools
{
    public class ReflectiveMethodDesigner2
    {
        ExpressionToFormulaState state;
        public ReflectiveMethodDesigner2()
            : this(new ExpressionToFormulaState())
        {
        }

        public ReflectiveMethodDesigner2(ExpressionToFormulaState state)
        {
            this.state = state;
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
                PostEval(state.EntryPoint);
                Formula.Pin(state.EntryPoint);
            }
        }

        public void ExpressInternally(
            Expression<Func<ILGenerator>> ilId, 
            ITypeDeclaration returnType, 
            ITypeDeclaration[] parameterTypes, 
            Action<ReflectiveMethodDesigner2> statementsBlock)
        {
            var ilName = TypeSavable.GetName(ilId);
            var methodGen = new SRMethodBaseEmitHook(ILBuilder.MethodGenerator, ilName, this);
            var ilBuilder = new ILBuilder(methodGen, returnType);
            ExpressInternally(ilBuilder, statementsBlock);
        }

        public void ExpressInternally(ILBuilder ilBuilder, Action<ReflectiveMethodDesigner2> statementsBlock)
        {
            var gen = new ReflectiveMethodDesigner2();
            gen.ILBuilder = ilBuilder;
            statementsBlock(gen);
            gen.Eval(() => Dsl.End());
        }

        public ReadOnlyCollection<IDirectiveGenerator> ToDirectives(LambdaExpression exp)
        {
            throw new NotImplementedException();
        }

        protected virtual void PostEval(EndFormula entryPoint)
        {
            ResolveReference(entryPoint);
            OptimizeIntermediateFormula(entryPoint);
            BuildIL(entryPoint);
        }

        protected virtual void ResolveReference(EndFormula entryPoint)
        {
            var visitor = default(IFormulaVisitor);
            visitor = new NoActionVisitor();
            visitor = new VariableResolver(visitor);
            visitor = new TypeResolver(visitor, GetReturnType());
            entryPoint.Accept(visitor);
        }

        protected virtual void OptimizeIntermediateFormula(EndFormula entryPoint)
        {
            var visitor = default(IFormulaVisitor);
            visitor = new NoActionVisitor();
            visitor = new ConvertDecreaser(visitor);
            visitor = new ConvertIncreaser(visitor);
            entryPoint.Accept(visitor);
        }

        protected virtual void BuildIL(EndFormula entryPoint)
        {
            if (ILBuilder != null)
            {
                entryPoint.Accept(ILBuilder);
            }
        }

        public ILBuilder ILBuilder { get; set; }
        
        public ITypeDeclaration GetReturnType()
        {
            if (ILBuilder == null)
            {
                return typeof(void).ToTypeDecl();
            }
            else
            {
                return ILBuilder.ReturnType;
            }
        }

        public string DumpEntryPoint()
        {
            return state.EntryPoint.ToString();
        }

        public string DumpCurrent()
        {
            return state.CurrentBlock.ToString();
        }
    }
}
