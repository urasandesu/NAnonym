using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Mixins.System.Reflection.Emit
{
    public static class DynamicMethodMixin
    {
        public static void ExpressBody(this DynamicMethod dynamicMethod, Action<ExpressiveMethodBodyGenerator> expression)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRDynamicMethodGeneratorImpl(dynamicMethod));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this DynamicMethod dynamicMethod, Action<ExpressiveMethodBodyGenerator> expression, ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRDynamicMethodGeneratorImpl(dynamicMethod, parameterBuilders));
            gen.ExpressBodyEnd(expression);
        }
    }
}
