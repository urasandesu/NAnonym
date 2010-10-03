using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UNI = Urasandesu.NAnonym.ILTools;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCLabelDeclarationImpl : UNI::ILabelDeclaration
    {
        Instruction target;
        public MCLabelDeclarationImpl(Instruction target)
        {
            this.target = target;
        }

        internal Instruction Target { get { return target; } }
    }
}
