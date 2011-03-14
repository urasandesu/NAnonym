/* 
 * File: NodeCollection.cs
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
using System.Collections;
using Urasandesu.NAnonym;
using System.Collections.Specialized;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Formulas
{
    public class NodeCollection<TNode> :
        Node, IList<TNode>, ICollection<TNode>, IEnumerable<TNode> where TNode : Node
    {
        protected IList<TNode> list;

        public NodeCollection()
        {
            Initialize(new Collection<TNode>());
        }

        public NodeCollection(params TNode[] formulas)
        {
            Initialize(new Collection<TNode>(formulas));
        }

        void Initialize(IList<TNode> list)
        {
            this.list = list;
        }

        public int IndexOf(TNode item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, TNode item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var removingItem = list[index];
            list.RemoveAt(index);
        }

        public TNode this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                var replacingItem = list[index];
                list[index] = value;
            }
        }

        public void Add(TNode item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(TNode item)
        {
            return list.Contains(item);
        }

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public bool Remove(TNode item)
        {
            return list.Remove(item);
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void PinCore()
        {
            Initialize(new ReadOnlyCollection<TNode>(list));
            base.PinCore();
        }

        public override void AppendWithBracketTo(StringBuilder sb)
        {
            sb.Append("[");
            var oneOrMore = false;
            foreach (var formula in list)
            {
                if (!oneOrMore)
                {
                    oneOrMore = true;
                    formula.AppendWithBracketTo(sb);
                }
                else
                {
                    sb.Append(", ");
                    formula.AppendWithBracketTo(sb);
                }
            }
            sb.Append("]");
        }

        public override void Accept(IFormulaVisitor visitor)
        {
            list.ForEach(_ => _.Accept(visitor));
        }
    }
}
