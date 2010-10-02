using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools
{
    public static class ExpressiveMethodBodyGeneratorMixin
    {
        internal static void ExpressBodyEnd(this ExpressiveMethodBodyGenerator methodBodyGen, Action<ExpressiveMethodBodyGenerator> expression)
        {
            expression(methodBodyGen);
            if (methodBodyGen.Directives.Last().OpCode != OpCodes.Ret)
            {
                methodBodyGen.Eval(_ => _.End());
            }
        }
    }
}
