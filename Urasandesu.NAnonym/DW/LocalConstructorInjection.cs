using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorInjection : ConstructorInjection
    {
        public LocalConstructorInjection(ITypeGenerator declaringTypeGenerator, HashSet<InjectionFieldInfo> fieldSet)
            : base(declaringTypeGenerator, fieldSet)
        {
        }

        protected override ConstructorInjectionDefiner GetConstructorDefiner(ConstructorInjection parent)
        {
            return new LocalConstructorInjectionDefiner(parent);
        }

        protected override ConstructorInjectionBuilder GetConstructorBuilder(ConstructorInjectionDefiner parentDefiner)
        {
            return new LocalConstructorInjectionBuilder(parentDefiner);
        }
    }
}
