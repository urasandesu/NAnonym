/* 
 * File: VariableFormula.g.cs
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
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.NAnonym.Formulas
{
    public partial class VariableFormula : Formula
    {

        protected override void InitializeForCodeGeneration()
        {
            base.InitializeForCodeGeneration();
			NodeType = NodeType.Variable;
            VariableName = default(string);
            VariableIndex = -1;
            Resolved = default(Node);
            Block = default(BlockFormula);
        }

        public const string NameOfVariableName = "VariableName";
        string variableName;
        public string VariableName 
        { 
            get { return variableName; } 
            set 
            {
                SetValue(NameOfVariableName, value, ref variableName);
            }
        }
        public const string NameOfVariableIndex = "VariableIndex";
        int variableIndex;
        public int VariableIndex 
        { 
            get { return variableIndex; } 
            set 
            {
                SetValue(NameOfVariableIndex, value, ref variableIndex);
            }
        }
        public const string NameOfResolved = "Resolved";
        Node resolved;
        public Node Resolved 
        { 
            get { return resolved; } 
            set 
            {
                SetNode(NameOfResolved, value, ref resolved);
            }
        }
        public const string NameOfBlock = "Block";
        BlockFormula block;
        public BlockFormula Block 
        { 
            get { return block; } 
            set 
            {
                SetNode(NameOfBlock, value, ref block);
            }
        }


        public override void Accept(IFormulaVisitor visitor)
        {
            visitor.Visit(this);
        }


        protected override void PinCore()
        {
            Formula.Pin(Resolved);
            Formula.Pin(Block);
            base.PinCore();
        }


        public override void AppendTo(StringBuilder sb)
        {
            base.AppendTo(sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfVariableName);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(VariableName, sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfVariableIndex);
            sb.Append(NodeToString.EndOfName);
            NodeToString.AppendValueTo(VariableIndex, sb);
            sb.Append(NodeToString.Delimiter);
            sb.Append(NodeToString.StartOfName);
            sb.Append(NameOfResolved);
            sb.Append(NodeToString.EndOfName);
            if (Resolved == null)
            {
                sb.Append(NodeToString.NullString);
            }
            else
            {
                Resolved.AppendWithStartEndTo(sb);
            }
        }
    }
}

