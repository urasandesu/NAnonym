using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    public class LocalPropertyGet<TBase, T>
    {
        public LocalClass<TBase> Override(Func<TBase, Func<T>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Func<TBase, Func<T>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalPropertySet<TBase, T>
    {
        public LocalClass<TBase> Override(Func<TBase, Action<T>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Implement(Func<TBase, Action<T>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
