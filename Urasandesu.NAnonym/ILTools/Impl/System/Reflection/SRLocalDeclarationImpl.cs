using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRLocalDeclarationImpl : ILocalDeclaration
    {
        string name;
        Type type;
        int index;

        ITypeDeclaration typeDecl;

        public SRLocalDeclarationImpl(Type type, int index)
            : this(null, type, index)
        {
        }

        public SRLocalDeclarationImpl(string name, Type type, int index)
        {
            this.name = name;
            this.type = type;
            this.index = index;
            typeDecl = new SRTypeDeclarationImpl(type);
        }

        #region ILocalDeclaration メンバ

        public string Name
        {
            get { return name; }
        }

        public ITypeDeclaration Type
        {
            get { return typeDecl; }
        }

        public int Index
        {
            get { return index; }
        }

        #endregion
    }
}
