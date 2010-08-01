using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MC = Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using SR = System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.CREUtilities
{
    //public sealed partial class Directive
    //{
    //    public static implicit operator MC.Cil.Instruction(Directive directive)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static implicit operator Directive(MC.Cil.Instruction instruction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public OpCode OpCode { get; private set; }
    //    public object Operand { get; private set; }

    //    public Directive(OpCode opcode, object operand)
    //    {
    //        this.OpCode = opcode;
    //        this.Operand = operand;
    //    }
    //}

}
