/* 
 * File: TransformerEnumerateOnly.cs
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
using System.Collections;

namespace Urasandesu.NAnonym.Linq
{
    public class TransformerEnumerateOnly<TSource, TDestination> : IList<TDestination>
    {
        protected readonly IList<TSource> source;
        protected readonly Func<TSource, TDestination> selector;
        public TransformerEnumerateOnly(IList<TSource> source, Func<TSource, TDestination> selector)
        {
            Required.NotDefault(source, () => source);
            Required.NotDefault(selector, () => selector);

            this.source = source;
            this.selector = selector;
        }

        #region IList<TDestination> メンバ

        public virtual int IndexOf(TDestination item)
        {
            throw new NotSupportedException();
        }

        public virtual void Insert(int index, TDestination item)
        {
            throw new NotSupportedException();
        }

        public virtual void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public virtual TDestination this[int index]
        {
            get
            {
                return selector(source[index]);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection<TDestination> メンバ

        public virtual void Add(TDestination item)
        {
            throw new NotSupportedException();
        }

        public virtual void Clear()
        {
            throw new NotSupportedException();
        }

        public virtual bool Contains(TDestination item)
        {
            throw new NotSupportedException();
        }

        public virtual void CopyTo(TDestination[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return source.Count; }
        }

        public bool IsReadOnly
        {
            get { return source.IsReadOnly; }
        }

        public virtual bool Remove(TDestination item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<TDestination> メンバ

        public IEnumerator<TDestination> GetEnumerator()
        {
            return source.Select(selector).GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

