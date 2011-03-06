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
        IMethodBaseGenerator methodGen;
        ExpressionToFormulaState state;
        public ReflectiveMethodDesigner2(IMethodBaseGenerator methodGen)
        {
            //this.methodGen = methodGen;
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
                // NOTE: The visitor chain is applied order by FILO.
                var visitor = default(IFormulaVisitor);
                visitor = new FormulaNoActionVisitor();
                //visitor = new VariableResolver(visitor, methodGen);
                visitor = new ConvertDecreaser(visitor);
                visitor = new ConvertIncreaser(visitor);
                state.CurrentBlock.Accept(visitor);
                Formula.Pin(state.CurrentBlock);
                // これ以降、Formula の書き換えは行わない。
                // 変数の作成などもここまでに行う。
            }
        }

        public BlockFormula Current { get { return state.CurrentBlock; } } 

        public string Dump()
        {
            return state.CurrentBlock.ToString();
        }
    }

    public class VariableResolver : FormulaAdapter
    {
        IMethodBaseGenerator methodGen;
        public VariableResolver(IFormulaVisitor visitor, IMethodBaseGenerator methodGen)
            : base(visitor)
        {
            this.methodGen = methodGen;
        }

        public override Formula Visit(AssignFormula formula)
        {
            if (formula.Left != null && formula.Left.NodeType == NodeType.Variable)
            {
                var variable = (VariableFormula)formula.Left;
                if (variable.Local == null)
                {
                    var local = default(ILocalDeclaration);
                    var definedVariable = GetDefinedVariables(variable).FirstOrDefault();
                    if (definedVariable == null)
                    {
                        local = methodGen.Body.ILOperator.AddLocal(variable.VariableName, variable.TypeDeclaration);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                    variable.Local = local;
                }
            }
            return base.Visit(formula);
        }

        IEnumerable<VariableFormula> GetDefinedVariables(VariableFormula formula)
        {
            var referrer = default(Formula);
            while ((referrer = formula.Referrer) != null)
            {
                var block = default(BlockFormula);
                if ((block = referrer as BlockFormula) != null)
                {
                    foreach (var variable in block.Variables.Where(_ => _.VariableName == formula.VariableName))
                    {
                        yield return variable;
                    }
                }
            }
            yield break;
        }
    }


    public class ILBuilder : FormulaAdapter
    {
        IMethodBaseGenerator method;
        public ILBuilder(IMethodBaseGenerator method)
            : base(new FormulaNoActionVisitor())
        {
            this.method = method;
        }

        public override Formula Visit(ConstantFormula formula)
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
                method.Body.ILOperator.Emit(OpCodes.Ldnull);
            }
            else if ((s = formula.ConstantValue as string) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldstr, s);
            }
            else if ((ns = formula.ConstantValue as short?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, (int)ns);
                method.Body.ILOperator.Emit(OpCodes.Conv_I2);
            }
            else if ((ni = formula.ConstantValue as int?) != null)
            {
                // HACK: _S が付く命令は短い形式。-127 ～ 128 以外は最適化が必要。
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(int)ni);
            }
            else if ((nd = formula.ConstantValue as double?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_R8, (double)nd);
            }
            else if ((nc = formula.ConstantValue as char?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)(char)nc);
            }
            else if ((nsb = formula.ConstantValue as sbyte?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4_S, (sbyte)nsb);
            }
            else if ((nb = formula.ConstantValue as bool?) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, (bool)nb ? 1 : 0);
            }
            else if ((e = formula.ConstantValue as Enum) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldc_I4, e.GetHashCode());
            }
            else if ((t = formula.ConstantValue as Type) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, t);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetTypeFromHandleInfo);
            }
            else if ((mi = formula.ConstantValue as MethodInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, mi);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                method.Body.ILOperator.Emit(OpCodes.Castclass, typeof(MethodInfo));
            }
            else if ((ci = formula.ConstantValue as ConstructorInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, ci);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetMethodFromHandleInfo);
                method.Body.ILOperator.Emit(OpCodes.Castclass, typeof(ConstructorInfo));
            }
            else if ((fi = formula.ConstantValue as FieldInfo) != null)
            {
                method.Body.ILOperator.Emit(OpCodes.Ldtoken, fi);
                method.Body.ILOperator.Emit(OpCodes.Call, MethodInfoMixin.GetFieldFromHandleInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
            return base.Visit(formula);
        }

        public override Formula Visit(ReturnFormula formula)
        {
            var result = base.Visit(formula);
            method.Body.ILOperator.Emit(OpCodes.Ret);
            return result;
        }
    }

}
