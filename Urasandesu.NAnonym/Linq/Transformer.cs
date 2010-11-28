/* 
 * File: Transformer.cs
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

namespace Urasandesu.NAnonym.Linq
{
    public sealed class Transformer<TSource, TDestination> : TransformerEnumerateOnly<TSource, TDestination>
    {
        readonly Func<TDestination, TSource> invertor;
        public Transformer(IList<TSource> source, Func<TSource, TDestination> selector, Func<TDestination, TSource> invertor)
            : base(source, selector)
        {
            Required.NotDefault(invertor, () => invertor);

            this.invertor = invertor;
        }

        public override int IndexOf(TDestination item)
        {
            return source.IndexOf(invertor(item));
        }

        public override void Insert(int index, TDestination item)
        {
            source.Insert(index, invertor(item));
        }

        public override void RemoveAt(int index)
        {
            source.RemoveAt(index);
        }

        public override TDestination this[int index]
        {
            get
            {
                return selector(source[index]);
            }
            set
            {
                source[index] = invertor(value);
            }
        }

        public override void Add(TDestination item)
        {
            source.Add(invertor(item));
        }

        public override void Clear()
        {
            source.Clear();
        }

        public override bool Contains(TDestination item)
        {
            return source.Contains(invertor(item));
        }

        public override void CopyTo(TDestination[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < source.Count; i++)
            {
                array[i] = selector(source[i]);
            }
        }

        public override bool Remove(TDestination item)
        {
            return source.Remove(invertor(item));
        }
    }
}

