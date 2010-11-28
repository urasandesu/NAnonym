using System;
using System.Collections.Generic;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalMethodWeaver : MethodWeaver
    {
        public GlobalMethodWeaver(ConstructorWeaver constructorWeaver, HashSet<WeaveMethodInfo> methodSet)
            : base(constructorWeaver, methodSet)
        {
        }

        protected override MethodWeaveDefiner GetMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
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
                return new GlobalReplaceMethodWeaveDefiner(parent, injectionMethod);
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

        protected override MethodWeaveBuilder GetMethodBuilder(MethodWeaveDefiner parentDefiner)
        {
            return new GlobalMethodWeaveBuilder(parentDefiner);
        }
    }
}
