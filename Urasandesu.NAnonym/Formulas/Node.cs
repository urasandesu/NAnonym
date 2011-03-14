/* 
 * File: Node.cs
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
using System.Text;

namespace Urasandesu.NAnonym.Formulas
{
    public abstract partial class Node
    {
        INodeToString nodeToString = new NodeToJson();
        protected INodeToString NodeToString { get { return nodeToString; } set { nodeToString = value; } }

        protected virtual void Initialize()
        {
        }

        public bool IsPinned { get; private set; }

        protected void CheckCanModify(Node node)
        {
            if (node != null && node.IsPinned)
            {
                throw new NotSupportedException("This object has already pinned, so it can not modify.");
            }
        }

        protected void SetValue<T>(string propertyName, T value, ref T result)
        {
            CheckCanModify(this);
            result = value;
        }

        protected void SetNode<TNode>(string propertyName, TNode node, ref TNode result) where TNode : Node
        {
            SetValue(propertyName, node, ref result);
        }

        public static void Pin(Node item)
        {
            if (item != null && !item.IsPinned)
            {
                item.IsPinned = true;
                item.PinCore();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            AppendWithStartEndTo(sb);
            return sb.ToString();
        }

        public virtual void AppendWithStartEndTo(StringBuilder sb)
        {
            NodeToString.AppendWithStartEndTo(this, sb);
        }

        public abstract void Accept(IFormulaVisitor visitor);
    }
}
