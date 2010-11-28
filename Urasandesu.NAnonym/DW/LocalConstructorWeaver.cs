using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorWeaver : ConstructorWeaver
    {
        public LocalConstructorWeaver(ITypeGenerator declaringTypeGenerator, HashSet<WeaveFieldInfo> fieldSet)
            : base(declaringTypeGenerator, fieldSet)
        {
        }

        protected override ConstructorWeaveDefiner GetConstructorDefiner(ConstructorWeaver parent)
        {
            return new LocalConstructorWeaveDefiner(parent);
        }

        protected override ConstructorWeaveBuilder GetConstructorBuilder(ConstructorWeaveDefiner parentDefiner)
        {
            return new LocalConstructorWeaveBuilder(parentDefiner);
        }
    }
}
