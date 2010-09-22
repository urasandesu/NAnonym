using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRParameterDeclarationImpl : IParameterDeclaration
    {
        ParameterInfo parameterInfo;
        public SRParameterDeclarationImpl(ParameterInfo parameterInfo)
            : this(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.Position)
        {
            this.parameterInfo = parameterInfo;
        }

        ITypeDeclaration typeDecl;
        string name;
        Type type;
        int position;
        public SRParameterDeclarationImpl(string name, Type type, int position)
        {
            this.name = name;
            this.type = type;
            this.position = position;
            typeDecl = new SRTypeDeclarationImpl(type);
        }

        #region IParameterDeclaration メンバ

        public string Name
        {
            get { return name; }
        }

        public ITypeDeclaration ParameterType
        {
            get { return typeDecl; }
        }

        public int Position
        {
            get { return position; }
        }

        #endregion
    }
}
