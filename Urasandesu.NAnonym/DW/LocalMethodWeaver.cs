using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    class LocalMethodWeaver : MethodWeaver
    {
        public LocalMethodWeaver(ConstructorWeaver constructorWeaver, HashSet<WeaveMethodInfo> methodSet)
            : base(constructorWeaver, methodSet)
        {
        }

        protected override MethodWeaveDefiner GetMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                return new LocalOverrideMethodWeaveDefiner(parent, injectionMethod);
            }
            else if (injectionMethod.Mode == SetupModes.Implement)
            {
                return new LocalImplementMethodWeaveDefiner(parent, injectionMethod);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected override MethodWeaveBuilder GetMethodBuilder(MethodWeaveDefiner parentDefiner)
        {
            return new LocalMethodWeaveBuilder(parentDefiner);
        }
    }
}
