using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;
using MCC = Mono.Cecil.Cil;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil.Cil;


namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCDirectiveDeclarationImpl : IDirectiveDeclaration
    {
        MCC::Instruction instruction;
        object nanonymOperand;
        object clrOperand;

        public MCDirectiveDeclarationImpl(MCC::Instruction instruction)
        {
            this.instruction = instruction;
            if (instruction.Operand == null)
            {
                nanonymOperand = null;
                clrOperand = null;
            }
            else if (instruction.Operand is byte)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is CallSite) throw new NotImplementedException();
            else if (instruction.Operand is double)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is FieldReference)
            {
                var fieldRef = (FieldReference)instruction.Operand;
                nanonymOperand = new MCFieldDeclarationImpl(fieldRef);
                clrOperand = fieldRef.ToFieldInfo();
            }
            else if (instruction.Operand is float)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is MCC::Instruction)
            {
                nanonymOperand = new MCDirectiveGeneratorImpl((MCC::Instruction)instruction.Operand);
                clrOperand = null;
            }
            else if (instruction.Operand is MCC::Instruction[])
            {
                nanonymOperand = ((MCC::Instruction[])instruction.Operand).Select(_instruction => new MCDirectiveGeneratorImpl(_instruction)).ToArray();
                clrOperand = null;
            }
            else if (instruction.Operand is int)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is long)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is MethodReference)
            {
                var methodRef = (MethodReference)instruction.Operand;
                if (methodRef.Name == ".ctor")
                {
                    nanonymOperand = new MCConstructorDeclarationImpl(methodRef);
                    clrOperand = methodRef.ToConstructorInfo();
                }
                else
                {
                    nanonymOperand = new MCMethodDeclarationImpl(methodRef);
                    clrOperand = methodRef.ToMethodInfo();
                }
            }
            else if (instruction.Operand is ParameterDefinition)
            {
                var parameterDef = (ParameterDefinition)instruction.Operand;
                nanonymOperand = new MCParameterGeneratorImpl(parameterDef);
                clrOperand = parameterDef.ToParameterInfo();
            }
            else if (instruction.Operand is sbyte)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is string)
            {
                nanonymOperand = instruction.Operand;
                clrOperand = instruction.Operand;
            }
            else if (instruction.Operand is TypeReference)
            {
                var typeRef = (TypeReference)instruction.Operand;
                nanonymOperand = new MCTypeDeclarationImpl(typeRef);
                clrOperand = typeRef.ToType();
            }
            else if (instruction.Operand is MCC::VariableDefinition)
            {
                nanonymOperand = new MCLocalGeneratorImpl((MCC::VariableDefinition)instruction.Operand);
                clrOperand = null;
            }
            else throw new NotSupportedException();
        }

        #region IDirectiveDeclaration メンバ

        public OpCode OpCode
        {
            get { return instruction.OpCode.ToNAnonym(); }
        }

        public object RawOperand
        {
            get { return instruction.Operand; }
        }

        #endregion

        #region IDirectiveDeclaration メンバ


        public object NAnonymOperand
        {
            get { return nanonymOperand; }
        }

        public object ClrOperand
        {
            get { return clrOperand; }
        }

        #endregion
    }
}
