using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCDirectiveGeneratorImpl : MCDirectiveDeclarationImpl, IDirectiveGenerator
    {
        MC::Cil.Instruction instruction;
        public MCDirectiveGeneratorImpl(MC::Cil.Instruction instruction)
        {
            this.instruction = instruction;
        }

        #region IDirectiveDeclaration メンバ

        OpCode IDirectiveDeclaration.OpCode
        {
            get { return instruction.OpCode.ToUni(); }
        }

        object IDirectiveDeclaration.Operand
        {
            get { return instruction.Operand; }
        }

        #endregion
    }
}
