using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRLabelDeclarationImpl : ILabelDeclaration
    {
        Label label;
        public SRLabelDeclarationImpl(Label label)
        {
            this.label = label;
        }

        internal Label Label { get { return label; } }
    }
}
