using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    // NOTE: Method や Constructor は Declaration と Generator が一段離れるイメージ。
    sealed class SRConstructorGeneratorImpl : SRConstructorDeclarationImpl, IConstructorGenerator
    {
        readonly ConstructorBuilder constructorBuilder;

        readonly IMethodBodyGenerator bodyGen;
        readonly ITypeGenerator declaringTypeGen;
        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder)
            : base(constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            bodyGen = (SRMethodBodyGeneratorImpl)constructorBuilder;
            var declaringTypeBuilder = constructorBuilder.DeclaringType as TypeBuilder;
            declaringTypeGen = declaringTypeBuilder == null ? null : (SRTypeGeneratorImpl)declaringTypeBuilder;
        }

        public static explicit operator SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            return new SRConstructorGeneratorImpl(constructorBuilder);
        }

        public static explicit operator ConstructorBuilder(SRConstructorGeneratorImpl methodDecl)
        {
            return methodDecl.constructorBuilder;
        }

        public new IMethodBodyGenerator Body
        {
            get { return bodyGen; }
        }

        #region IMethodBaseGenerator メンバ

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public PortableScope2Item AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMethodBaseGenerator メンバ


        public new ITypeGenerator DeclaringType
        {
            get { return declaringTypeGen; }
        }

        #endregion
    }


}
