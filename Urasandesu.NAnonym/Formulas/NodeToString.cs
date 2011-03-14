/* 
 * File: NodeToString.cs
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
using System.Text;

namespace Urasandesu.NAnonym.Formulas
{
    public abstract class NodeToString : INodeToString
    {
        public void AppendValueTo<TValue>(TValue value, StringBuilder sb)
        {
            AppendValueTo(value, sb, null);
        }

        public void AppendValueTo<TValue>(TValue value, StringBuilder sb, string ifDefault)
        {
            if (!(value is ValueType) && value.IsDefault())
            {
                sb.Append(ifDefault == null ? NullString : ifDefault);
            }
            else
            {
                var s = value.ToString();
                var result = default(double);
                if (double.TryParse(s, out result))
                {
                    sb.Append(StartOfNumber);
                    sb.Append(s);
                    sb.Append(EndOfNumber);
                }
                else
                {
                    sb.Append(StartOfString);
                    sb.Append(s);
                    sb.Append(EndOfString);
                }
            }
        }

        public void AppendListWithStartEndTo<TNode>(IList<TNode> list, StringBuilder sb) where TNode : Node
        {
            sb.Append(StartOfCollection);
            var oneOrMore = false;
            foreach (var node in list)
            {
                if (!oneOrMore)
                {
                    oneOrMore = true;
                    node.AppendWithStartEndTo(sb);
                }
                else
                {
                    sb.Append(Delimiter);
                    node.AppendWithStartEndTo(sb);
                }
            }
            sb.Append(EndOfCollection);
        }

        public void AppendWithStartEndTo(Node node, StringBuilder sb)
        {
            sb.Append(StartOfObject);
            node.AppendTo(sb);
            sb.Append(EndOfObject);
        }

        public abstract string NullString { get; }
        public abstract string StartOfName { get; }
        public abstract string EndOfName { get; }
        public abstract string StartOfNumber { get; }
        public abstract string EndOfNumber { get; }
        public abstract string StartOfString { get; }
        public abstract string EndOfString { get; }
        public abstract string StartOfObject { get; }
        public abstract string EndOfObject { get; }
        public abstract string StartOfCollection { get; }
        public abstract string EndOfCollection { get; }
        public abstract string Delimiter { get; }
    }
}
