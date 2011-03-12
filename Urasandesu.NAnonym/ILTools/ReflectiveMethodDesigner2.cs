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
                OnEvalEnded(state.CurrentBlock);
                Formula.Pin(state.CurrentBlock);
            }
        }

        protected virtual void OnEvalEnded(BlockFormula entryPoint)
        {
            // Optimize the intermediate formula.
            // NOTE: The visitor chain is applied order by FILO.
            var visitor = default(IFormulaVisitor);
            visitor = new FormulaNoActionVisitor();
            visitor = new VariableResolver(visitor);
            visitor = new ConvertDecreaser(visitor);
            visitor = new ConvertIncreaser(visitor);
            entryPoint.Accept(visitor);
            

            // Build IL.
            if (ILBuilder != null)
            {
                entryPoint.Accept(ILBuilder);
            }
        }

        public ILBuilder ILBuilder { get; set; } 

        public string Dump()
        {
            return state.CurrentBlock.ToString();
        }
    }

    public class ReturnChecker : FormulaAdapter
    {
        public ReturnChecker(IFormulaVisitor visitor)
            : base(visitor)
        {
        }

        public override void Visit(BlockFormula formula)
        {
            base.Visit(formula);
        }

        public override void Visit(ReturnFormula formula)
        {
            /*
             * VariableResolver と同じパターン。
             * 現在の Block に Return を記録しつつ、Return にも現在の Block を記録する。
             * 現在の Block に Return が記録済みであればエラー（もう現在の場所に来ないのは明白なため）。
             * 現在の Block の ParentBlock に Return が記録済みの場合は、途中で分岐があるかどうかとか、式の結果が定数とか調べる必要がある。→こっちはやれる余地を残しておくということでとりあえず後回し。
             * 
             */
            base.Visit(formula);
        }
    }

    public class ILBuilder : FormulaAdapter
    {
        IMethodBaseGenerator methodGen;
        public ILBuilder(IMethodBaseGenerator methodGen)
            : base(new FormulaNoActionVisitor())
        {
            this.methodGen = methodGen;
        }

        public override void Visit(BaseNewFormula formula)
        {
            methodGen.Body.ILOperator.Emit(OpCodes.Ldarg_0);
            var ci = default(IConstructorDeclaration);
            if (methodGen.DeclaringType.BaseType != null)
            {
                ci = methodGen.DeclaringType.BaseType.GetConstructor(new Type[] { });
            }
            else
            {
                throw new NotImplementedException();
            }
            methodGen.Body.ILOperator.Emit(OpCodes.Call, ci);
            base.Visit(formula);
        }

        public override void Visit(BlockFormula formula)
        {
            foreach (var local in formula.Locals)
            {
                local.Local = methodGen.Body.ILOperator.AddLocal(local.LocalName, local.TypeDeclaration);
            }
            base.Visit(formula);
        }

        public override void Visit(ConstantFormula formula)
        {
            string s = default(string);
            short? ns = default(short?);
            int? ni = default(int?);
            double? nd = default(double?);
            char? nc = default(char?);
            sbyte? nsb = default(sbyte?);
            bool? nb = default(bool?);
            var e = default(Enum);
            var t = default(Type);
            var mi = default(MethodInfo);
            var ci = default(ConstructorInfo);
            var fi = default(FieldInfo);
            if (formula.ConstantValue == null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldnull);
            }
            else if ((s = formula.ConstantValue as string) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldstr, s);
            }
            else if ((ns = formula.ConstantValue as short?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, (int)ns);
                methodGen.Body.ILOperator.Emit(OpCodes.Conv_I2);
            }
            else if ((ni = formula.ConstantValue as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = formula.ConstantValue as double?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = formula.ConstantValue as char?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nsb = formula.ConstantValue as sbyte?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)nsb);
            }
            else if ((nb = formula.ConstantValue as bool?) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = formula.ConstantValue as Enum) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = formula.ConstantValue as Type) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, t);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetTypeFromHandleInfo);
            }
            else if ((mi = formula.ConstantValue as MethodInfo) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, mi);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                methodGen.Body.ILOperator.Emit(OpCodes.Castclass, typeof(MethodInfo));
            }
            else if ((ci = formula.ConstantValue as ConstructorInfo) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, ci);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                methodGen.Body.ILOperator.Emit(OpCodes.Castclass, typeof(ConstructorInfo));
            }
            else if ((fi = formula.ConstantValue as FieldInfo) != null)
            {
                methodGen.Body.ILOperator.Emit(OpCodes.Ldtoken, fi);
                methodGen.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetFieldFromHandleInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
            base.Visit(formula);
        }

        public override void Visit(ReturnFormula formula)
        {
            base.Visit(formula);
            methodGen.Body.ILOperator.Emit(OpCodes.Ret);
        }

        public override void Visit(EndFormula formula)
        {
            base.Visit(formula);
            methodGen.Body.ILOperator.Emit(OpCodes.Ret);
        }
    }

}
