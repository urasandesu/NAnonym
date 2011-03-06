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
    public class TransformerEnumerateOnly<TResult> : IList<TResult>
    {
        protected readonly IList source;
        protected readonly Func<object, TResult> selector;
        public TransformerEnumerateOnly(IList source, Func<object, TResult> selector)
        {
            Required.NotDefault(source, () => source);
            Required.NotDefault(selector, () => selector);

            this.source = source;
            this.selector = selector;
        }

        public virtual int IndexOf(TResult item)
        {
            throw new NotSupportedException();
        }

        public virtual void Insert(int index, TResult item)
        {
            throw new NotSupportedException();
        }

        public virtual void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public virtual TResult this[int index]
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

        public virtual void Add(TResult item)
        {
            throw new NotSupportedException();
        }

        public virtual void Clear()
        {
            throw new NotSupportedException();
        }

        public virtual bool Contains(TResult item)
        {
            throw new NotSupportedException();
        }

        public virtual void CopyTo(TResult[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < Count; i++)
            {
                array[i] = this[i];
            }
        }

        public int Count
        {
            get { return source.Count; }
        }

        public bool IsReadOnly
        {
            get { return source.IsReadOnly; }
        }

        public virtual bool Remove(TResult item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return source.Cast<object>().Select(selector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

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
            for (int i = arrayIndex; i < Count; i++)
            {
                array[i] = this[i];
            }
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

        public IEnumerator<TDestination> GetEnumerator()
        {
            return source.Select(selector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

