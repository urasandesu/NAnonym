using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UNI = Urasandesu.NAnonym.ILTools;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCLabelGeneratorImpl : MCLabelDeclarationImpl, UNI::ILabelGenerator
    {
        public MCLabelGeneratorImpl(Instruction target)
            : base(target)
        {
        }
    }
}
