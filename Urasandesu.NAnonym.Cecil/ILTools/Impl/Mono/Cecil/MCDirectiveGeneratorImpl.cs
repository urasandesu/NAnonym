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

        public static explicit operator MC::Cil.Instruction(MCDirectiveGeneratorImpl directiveGen)
        {
            return directiveGen.instruction;
        }

        public static explicit operator MCDirectiveGeneratorImpl(MC::Cil.Instruction instruction)
        {
            return new MCDirectiveGeneratorImpl(instruction);
        }

        #region IDirectiveDeclaration メンバ

        OpCode IDirectiveDeclaration.OpCode
        {
            get { return instruction.OpCode.Cast(); }
        }

        object IDirectiveDeclaration.Operand
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
