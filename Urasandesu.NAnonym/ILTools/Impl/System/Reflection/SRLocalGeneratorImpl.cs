using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRLocalGeneratorImpl : SRLocalDeclarationImpl, ILocalGenerator
    {
        LocalBuilder localBuilder;
        public SRLocalGeneratorImpl(string name, LocalBuilder localBuilder)
            : base(name, localBuilder.LocalType, localBuilder.LocalIndex)
        {
            this.localBuilder = localBuilder;
        }

        internal LocalBuilder LocalBuilder { get { return localBuilder; } }
    }
}
