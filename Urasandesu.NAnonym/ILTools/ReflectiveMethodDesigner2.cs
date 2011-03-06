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
                // ・state をチェックし、終了していたら以下の処理を開始する（ふつコンの流れを参考にすると良さげ）。
                //   ・参照の解決。
                //   ・型チェック、戻り値の確定。
                //   ・無駄な Convert の排除（最適化）。
                //   ・IL の生成。
                // NOTE: The visitor chain is applied order by FILO.
                var visitor = default(IFormulaVisitor);
                visitor = new FormulaNoActionVisitor();
                visitor = new ConvertDecreaser(visitor);
                visitor = new ConvertIncreaser(visitor);
                state.CurrentBlock.Accept(visitor);
                Formula.Pin(state.CurrentBlock);
            }
        }

        public string Dump()
        {
            return state.CurrentBlock.ToString();
        }
    }
}
