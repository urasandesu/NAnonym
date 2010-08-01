using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    class SRConstructorDeclarationImpl : IConstructorDeclaration
    {
        readonly ConstructorInfo constructorInfo;

        readonly ITypeDeclaration declaringTypeDecl;
        readonly IMethodBodyDeclaration bodyDecl;
        readonly IMethodBodyDeclaration methodBodyMaker;
        public SRConstructorDeclarationImpl(ConstructorInfo constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            declaringTypeDecl = (SRTypeDeclarationImpl)constructorInfo.DeclaringType;
            bodyDecl = null;
            methodBodyMaker = null;
        }

        public static explicit operator SRConstructorDeclarationImpl(ConstructorInfo constructorInfo)
        {
            return new SRConstructorDeclarationImpl(constructorInfo);
        }

        public static explicit operator ConstructorInfo(SRConstructorDeclarationImpl methodDecl)
        {
            return methodDecl.constructorInfo;
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDeclaration DeclaringType
        {
            get { return declaringTypeDecl; }
        }

        public ITypeGenerator DeclaringTypeGen
        {
            get { throw new NotImplementedException(); }
        }

        public IMethodBodyDeclaration Body
        {
            get { return methodBodyMaker; }
        }

        public ReadOnlyCollection<IParameterDeclaration> Parameters
        {
            get { throw new NotImplementedException(); }
        }
    }
}
