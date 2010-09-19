using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MO = Mono.Collections;
using MC = Mono.Cecil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using UN = Urasandesu.NAnonym;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    sealed class MCMethodBodyGeneratorImpl : MCMethodBodyDeclarationImpl, UN::ILTools.IMethodBodyGenerator
    {
        readonly MC::Cil.MethodBody bodyDef;
        readonly ReadOnlyCollection<ILocalGenerator> locals;
        ReadOnlyCollection<IDirectiveGenerator> directives;

        public MCMethodBodyGeneratorImpl(MC::Cil.MethodBody bodyDef)
        {
            this.bodyDef = bodyDef;
            locals = new ReadOnlyCollection<ILocalGenerator>(
                bodyDef.Variables.TransformEnumerateOnly(variableDef => (ILocalGenerator)(MCLocalGeneratorImpl)variableDef));
            directives = new ReadOnlyCollection<IDirectiveGenerator>(
                bodyDef.Instructions.TransformEnumerateOnly(instruction => (IDirectiveGenerator)(MCDirectiveGeneratorImpl)instruction));
        }

        public static explicit operator MCMethodBodyGeneratorImpl(MC::Cil.MethodBody bodyDef)
        {
            return new MCMethodBodyGeneratorImpl(bodyDef);
        }

        public static explicit operator MC::Cil.MethodBody(MCMethodBodyGeneratorImpl bodyGen)
        {
            return bodyGen.bodyDef;
        }

        public IILOperator GetILOperator()
        {
            return (MCILOperatorImpl)bodyDef.GetILProcessor();
        }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return locals; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return directives; }
        }
    }
}
