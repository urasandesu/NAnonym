/* 
 * File: ExpressionToFormulaState.cs
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
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using System.Reflection;
using Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Formulas
{
    public class ExpressionToFormulaState
    {
        public ExpressionToFormulaState()
        {
            CurrentBlock = null;
            PushBlock();
            Arguments = new Collection<Formula>();
            Conditions = new Collection<ConditionalFormula>();
            InlineValueState = new ExpressionToInlineValueState();
            ConstMembersCache = new Dictionary<Type, Dictionary<object, FieldInfo>>();
            Returns = new Collection<ReturnFormula>();
        }

        public void PushBlock()
        {
            CurrentBlock = new BlockFormula() { ParentBlock = CurrentBlock };
        }

        public void PopBlock()
        {
            if (CurrentBlock == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                CurrentBlock = CurrentBlock.ParentBlock;
            }
        }

        public BlockFormula CurrentBlock { get; private set; }
        public Collection<Formula> Arguments { get; private set; }
        public Collection<ConditionalFormula> Conditions { get; private set; }
        public ExpressionToInlineValueState InlineValueState { get; private set; }
        public Dictionary<Type, Dictionary<object, FieldInfo>> ConstMembersCache { get; private set; }
        public Collection<ReturnFormula> Returns { get; private set; }
        public EndFormula EntryPoint { get; set; }
        public bool IsEnded { get; set; }
    }
}

