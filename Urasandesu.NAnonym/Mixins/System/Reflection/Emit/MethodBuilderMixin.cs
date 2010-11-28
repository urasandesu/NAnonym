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
    public static class MethodBuilderMixin
    {
        public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression, ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder, parameterBuilders));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression, FieldBuilder[] fieldBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder, fieldBuilders));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this MethodBuilder methodBuilder, Action<ExpressiveMethodBodyGenerator> expression, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRMethodGeneratorImpl(methodBuilder, parameterBuilders, fieldBuilders));
            gen.ExpressBodyEnd(expression);
        }
    }
}
