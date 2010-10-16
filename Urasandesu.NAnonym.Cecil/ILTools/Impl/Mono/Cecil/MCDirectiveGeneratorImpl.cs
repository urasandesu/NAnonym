using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;
using MCC = Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCDirectiveGeneratorImpl : MCDirectiveDeclarationImpl, IDirectiveGenerator
    {
        public MCDirectiveGeneratorImpl(MCC::Instruction instruction)
            : base(instruction)
        {
        }
    }
}
