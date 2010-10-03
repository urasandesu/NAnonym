using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRLabelGeneratorImpl : SRLabelDeclarationImpl, ILabelGenerator
    {
        public SRLabelGeneratorImpl(Label label)
            : base(label)
        {
        }
    }
}
