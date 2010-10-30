using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools
{
    public class StoreExpressible
    {
        readonly StoreExpressible<object> storeExpressible = new StoreExpressible<object>();
        readonly MethodInfo AsInfo1;
        readonly MethodInfo AsInfo2;

        public StoreExpressible()
        {
            AsInfo1 = TypeSavable.GetMethodInfo<object, object>(() => As);
            AsInfo2 = TypeSavable.GetMethodInfo<object, object>(() => storeExpressible.As);
        }

        public bool IsAs(MethodInfo target)
        {
            return AsInfo1.Equivalent(AsInfo1) || AsInfo2.EquivalentWithoutGenericArguments(target);
        }

        public object As(object value)
        {
            return null;
        }
    }

    public class StoreExpressible<T>
    {
        public T As(T value)
        {
            return default(T);
        }
    }
}
