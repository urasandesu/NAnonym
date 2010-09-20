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
