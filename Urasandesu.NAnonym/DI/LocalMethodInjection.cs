using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjection : MethodInjection
    {
        public LocalMethodInjection(ConstructorInjection constructorInjection, HashSet<InjectionMethodInfo> methodSet)
            : base(constructorInjection, methodSet)
        {
        }

        protected override MethodInjectionDefiner GetMethodDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                return new LocalOverrideMethodInjectionDefiner(parent, injectionMethod);
            }
            else if (injectionMethod.Mode == SetupModes.Implement)
            {
                return new LocalImplementMethodInjectionDefiner(parent, injectionMethod);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected override MethodInjectionBuilder GetMethodBuilder(MethodInjectionDefiner parentDefiner)
        {
            return new LocalMethodInjectionBuilder(parentDefiner);
        }
    }
}
