using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit
{
    public static class ConstructorBuilderMixin
    {
        public static void ExpressBody(this ConstructorBuilder constructorBuilder, Action<ExpressiveMethodBodyGenerator> expression)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRConstructorGeneratorImpl(constructorBuilder));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this ConstructorBuilder constructorBuilder, Action<ExpressiveMethodBodyGenerator> expression, FieldBuilder[] fieldBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRConstructorGeneratorImpl(constructorBuilder, fieldBuilders));
            gen.ExpressBodyEnd(expression);
        }

        public static void ExpressBody(this ConstructorBuilder constructorBuilder, Action<ExpressiveMethodBodyGenerator> expression, ParameterBuilder[] parameterBuilders)
        {
            var gen = new ExpressiveMethodBodyGenerator(new SRConstructorGeneratorImpl(constructorBuilder, parameterBuilders));
            gen.ExpressBodyEnd(expression);
        }

    }
}
