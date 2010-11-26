using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    sealed class SRMethodBodyGeneratorImpl : SRMethodBodyDeclarationImpl, IMethodBodyGenerator
    {
        SRILOperatorImpl il;

        ReadOnlyCollection<ILocalGenerator> readonlyLocalGens;

        public SRMethodBodyGeneratorImpl(ISRMethodBaseGenerator methodBodyGen)
        {
            Initialize(new SRILOperatorImpl(methodBodyGen.GetILGenerator(), this));
        }

        void Initialize(SRILOperatorImpl il)
        {
            this.il = il;
            LocalGens = new List<SRLocalGeneratorImpl>();
            readonlyLocalGens = new ReadOnlyCollection<ILocalGenerator>(
                LocalGens.TransformEnumerateOnly(localGen => (ILocalGenerator)localGen));
        }

        public IILOperator ILOperator { get { return il; } }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return readonlyLocalGens; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return il.Directives; }
        }

        internal List<SRLocalGeneratorImpl> LocalGens { get; private set; }

        public ILocalGenerator AddLocal(ILocalGenerator localGen)
        {
            throw new NotImplementedException();
        }

    }
}
