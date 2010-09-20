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
