using System;
using System.Collections.Generic;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodInjection : MethodInjection
    {
        public GlobalMethodInjection(ConstructorInjection constructorInjection, HashSet<InjectionMethodInfo> methodSet)
            : base(constructorInjection, methodSet)
        {
        }

        protected override MethodInjectionDefiner GetMethodDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.Implement)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.Replace)
            {
                return new GlobalReplaceMethodInjectionDefiner(parent, injectionMethod);
            }
            else if (injectionMethod.Mode == SetupModes.Before)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.After)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected override MethodInjectionBuilder GetMethodBuilder(MethodInjectionDefiner parentDefiner)
        {
            return new GlobalMethodInjectionBuilder(parentDefiner);
        }
    }
}
