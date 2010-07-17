using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    public class LocalPropertyGet<TBase, T> where TBase : class
    {
        public LocalClass<TBase> As(Func<T> propertyGet)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Override(Func<TBase, Func<T>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Instead(Func<TBase, Func<T>> expression)
        {
            throw new NotImplementedException();
        }
    }

    public class LocalPropertySet<TBase, T> where TBase : class
    {
        public LocalClass<TBase> As(Action<T> propertySet)
        {
            throw new NotFiniteNumberException();
        }

        public LocalClass<TBase> Override(Func<TBase, Action<T>> expression)
        {
            throw new NotImplementedException();
        }

        public LocalClass<TBase> Instead(Func<TBase, Action<T>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
