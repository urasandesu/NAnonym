using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRAssemblyDeclarationImpl : IAssemblyDeclaration
    {
        Assembly assembly;
        public SRAssemblyDeclarationImpl(Assembly assembly)
        {
            this.assembly = assembly;
        }

        #region IDeserializableManually メンバ

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
