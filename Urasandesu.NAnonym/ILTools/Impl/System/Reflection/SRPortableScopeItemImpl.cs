using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR = System.Reflection;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRPortableScopeItemImpl : IPortableScopeItem
    {
        // NOTE: 構築中はこちら。まだ実体化してないので、Assembly.Load できない。
        public SRPortableScopeItemImpl(SR::Emit.FieldBuilder fieldBuilder, SR::Emit.LocalBuilder localBuilder)
        {
            throw new NotImplementedException();
        }

        // NOTE: 構築後はこちら。Deserialize 時などに利用することを想定。RawData に含まれる情報を利用し、Assembly.Load で一気取りする。
        public SRPortableScopeItemImpl(PortableScopeItemRawData rawData)
        {
            throw new NotImplementedException();
        }

        public object Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object Source
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public ITypeDeclaration Type
        {
            get { throw new NotImplementedException(); }
        }

        public int Index
        {
            get { throw new NotImplementedException(); }
        }

        public Type FieldType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
