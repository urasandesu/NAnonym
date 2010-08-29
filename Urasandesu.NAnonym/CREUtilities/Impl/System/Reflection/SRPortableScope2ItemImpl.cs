using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    class SRPortableScope2ItemImpl : PortableScope2Item
    {
        // NOTE: 構築中はこちら。まだ実体化してないので、Assembly.Load できない。
        public SRPortableScope2ItemImpl(SR::Emit.FieldBuilder fieldBuilder, SR::Emit.LocalBuilder localBuilder)
        {
            throw new NotImplementedException();
        }

        // NOTE: 構築後はこちら。Deserialize 時などに利用することを想定。RawData に含まれる情報を利用し、Assembly.Load で一気取りする。
        public SRPortableScope2ItemImpl(PortableScope2ItemRawData rawData)
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
