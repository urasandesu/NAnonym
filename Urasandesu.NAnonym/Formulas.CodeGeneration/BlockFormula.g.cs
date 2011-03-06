/* 
 * File: BlockFormula.g.cs
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
using System.ComponentModel;

namespace Urasandesu.NAnonym.Formulas
{
    public partial class BlockFormula : Formula
    {

        protected override void InitializeForCodeGeneration()
        {
            base.InitializeForCodeGeneration();
            NodeType = NodeType.Block;
            ParentBlock = default(BlockFormula);
            ChildBlocks = new FormulaCollection<BlockFormula>();
            Variables = new FormulaCollection<Formula>();
            Formulas = new FormulaCollection<Formula>();
            Result = default(Formula);
        }

        public const string NameOfParentBlock = "ParentBlock";
        BlockFormula parentBlock;
        public BlockFormula ParentBlock 
        { 
            get { return parentBlock; } 
            set 
            {
                SetFormulaWithoutNotification(NameOfParentBlock, value, ref parentBlock);
            }
        }
        public const string NameOfChildBlocks = "ChildBlocks";
        FormulaCollection<BlockFormula> childBlocks;
        public FormulaCollection<BlockFormula> ChildBlocks 
        { 
            get { return childBlocks; } 
            set 
            {
                SetFormula(NameOfChildBlocks, value, ref childBlocks);
            }
        }
        public const string NameOfVariables = "Variables";
        FormulaCollection<Formula> variables;
        public FormulaCollection<Formula> Variables 
        { 
            get { return variables; } 
            set 
            {
                SetFormula(NameOfVariables, value, ref variables);
            }
        }
        public const string NameOfFormulas = "Formulas";
        FormulaCollection<Formula> formulas;
        public FormulaCollection<Formula> Formulas 
        { 
            get { return formulas; } 
            set 
            {
                SetFormula(NameOfFormulas, value, ref formulas);
            }
        }
        public const string NameOfResult = "Result";
        Formula result;
        public Formula Result 
        { 
            get { return result; } 
            set 
            {
                SetFormula(NameOfResult, value, ref result);
            }
        }


        public override Formula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }


        protected override void PinCore()
        {
            Formula.Pin(ParentBlock);
            Formula.Pin(ChildBlocks);
            Formula.Pin(Variables);
            Formula.Pin(Formulas);
            Formula.Pin(Result);
            base.PinCore();
        }


        public override void AppendTo(StringBuilder sb)
        {
            base.AppendTo(sb);
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfChildBlocks);
            sb.Append("\": ");
            if (ChildBlocks == null)
            {
                sb.Append("null");
            }
            else
            {
                ChildBlocks.AppendWithBracketTo(sb);
            }
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfVariables);
            sb.Append("\": ");
            if (Variables == null)
            {
                sb.Append("null");
            }
            else
            {
                Variables.AppendWithBracketTo(sb);
            }
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfFormulas);
            sb.Append("\": ");
            if (Formulas == null)
            {
                sb.Append("null");
            }
            else
            {
                Formulas.AppendWithBracketTo(sb);
            }
            sb.Append(", ");
            sb.Append("\"");
            sb.Append(NameOfResult);
            sb.Append("\": ");
            if (Result == null)
            {
                sb.Append("null");
            }
            else
            {
                Result.AppendWithBracketTo(sb);
            }
        }
    }
}

