using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRMethodBaseDeclarationImpl : SRMemberDeclarationImpl, IMethodBaseDeclaration
    {
        readonly MethodBase methodBase;
        public SRMethodBaseDeclarationImpl(MethodBase methodBase)
            : base(methodBase)
        {
            this.methodBase = methodBase;
        }

        public MethodBase MethodBase { get { return (MethodBase)base.Source; } }

        #region IMethodBaseDeclaration メンバ

        public IMethodBodyDeclaration Body
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDeclaration DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<IParameterDeclaration> Parameters
        {
            get { return new ReadOnlyCollection<IParameterDeclaration>(new IParameterDeclaration[] { }); }
        }

        public IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
