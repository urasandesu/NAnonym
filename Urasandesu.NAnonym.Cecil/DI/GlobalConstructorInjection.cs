using System;
using System.Collections.Generic;
using Urasandesu.NAnonym.DI;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjection : ConstructorInjection
    {
        public GlobalConstructorInjection(
            UNI::ITypeGenerator declaringTypeGenerator,
            HashSet<InjectionFieldInfo> fieldSet)
            : base(declaringTypeGenerator, fieldSet)
        {
        }

        protected override ConstructorInjectionDefiner GetConstructorDefiner(ConstructorInjection parent)
        {
            return new GlobalConstructorInjectionDefiner(parent);
        }

        protected override ConstructorInjectionBuilder GetConstructorBuilder(ConstructorInjectionDefiner parentDefiner)
        {
            return new GlobalConstructorInjectionBuilder(parentDefiner);
        }
    }
}
