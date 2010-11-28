using System;
using System.Collections.Generic;
using Urasandesu.NAnonym.DW;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorWeaver : ConstructorWeaver
    {
        public GlobalConstructorWeaver(
            UNI::ITypeGenerator declaringTypeGenerator,
            HashSet<WeaveFieldInfo> fieldSet)
            : base(declaringTypeGenerator, fieldSet)
        {
        }

        protected override ConstructorWeaveDefiner GetConstructorDefiner(ConstructorWeaver parent)
        {
            return new GlobalConstructorWeaveDefiner(parent);
        }

        protected override ConstructorWeaveBuilder GetConstructorBuilder(ConstructorWeaveDefiner parentDefiner)
        {
            return new GlobalConstructorWeaveBuilder(parentDefiner);
        }
    }
}
