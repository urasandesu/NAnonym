using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MO = Mono.Collections;
using MC = Mono.Cecil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    sealed class MCMethodBodyGeneratorImpl : MCMethodBodyDeclarationImpl, IMethodBodyGenerator
    {
        readonly MC::Cil.MethodBody bodyDef;
        readonly ReadOnlyCollection<ILocalGenerator> locals;
        public MCMethodBodyGeneratorImpl(MC::Cil.MethodBody bodyDef)
        {
            this.bodyDef = bodyDef;
            locals = new ReadOnlyCollection<ILocalGenerator>(
                bodyDef.Variables.TransformEnumerateOnly(variableDef => (ILocalGenerator)(MCLocalGeneratorImpl)variableDef));
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
            get { throw new NotImplementedException(); }
        }
    }
}
