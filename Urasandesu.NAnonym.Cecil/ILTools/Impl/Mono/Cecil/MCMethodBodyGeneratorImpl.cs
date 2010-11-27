using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MO = Mono.Collections;
using MC = Mono.Cecil;
using MCC = Mono.Cecil.Cil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCMethodBodyGeneratorImpl : MCMethodBodyDeclarationImpl, UNI::IMethodBodyGenerator
    {
        readonly MCC::MethodBody bodyDef;
        readonly ReadOnlyCollection<ILocalGenerator> locals;
        ReadOnlyCollection<IDirectiveGenerator> directives;
        readonly MCILOperatorImpl ilOperator;

        public MCMethodBodyGeneratorImpl(MCC::MethodBody bodyDef)
            : this(bodyDef, ILEmitMode.Normal, null)
        {
        }

        public MCMethodBodyGeneratorImpl(MCC::MethodBody bodyDef, ILEmitMode mode, Instruction target)
        {
            this.bodyDef = bodyDef;
            ilOperator = new MCILOperatorImpl(bodyDef.GetILProcessor(), mode, target);
            locals = new ReadOnlyCollection<ILocalGenerator>(
                bodyDef.Variables.TransformEnumerateOnly(variableDef => (ILocalGenerator)new MCLocalGeneratorImpl(variableDef)));
            directives = new ReadOnlyCollection<IDirectiveGenerator>(
                bodyDef.Instructions.TransformEnumerateOnly(instruction => (IDirectiveGenerator)new MCDirectiveGeneratorImpl(instruction)));
        }

        public IILOperator ILOperator { get { return ilOperator; } }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return locals; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return directives; }
        }

        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }
    }
}
