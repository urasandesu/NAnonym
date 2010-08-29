using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCPortableScope2ItemImpl : PortableScope2Item
    {
        // NOTE: 構築中はこちら。まだ実体化してないので、GlobalAssemblyResolver.Resolve できない。
        public MCPortableScope2ItemImpl(FieldDefinition fieldDef, MC::Cil.VariableDefinition variableDef)
        {
            throw new NotImplementedException();
        }

        // NOTE: 構築後はこちら。Deserialize 時などに利用することを想定。RawData に含まれる情報を利用し、GlobalAssemblyResolver.Resolve で一気取りする。
        public MCPortableScope2ItemImpl(PortableScope2ItemRawData rawData)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        protected override string IMemberDeclaration_Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}
