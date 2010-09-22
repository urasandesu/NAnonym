using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    // NOTE: Method や Constructor は Declaration と Generator が一段離れるイメージ。
    sealed class SRConstructorGeneratorImpl : SRConstructorDeclarationImpl, IConstructorGenerator
    {
        readonly ConstructorBuilder constructorBuilder;

        readonly IMethodBodyGenerator bodyGen;
        readonly ITypeGenerator declaringTypeGen;
        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, params ParameterBuilder[] parameterBuilders)
            : base(constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            bodyGen = new SRMethodBodyGeneratorImpl(constructorBuilder);
            var declaringTypeBuilder = constructorBuilder.DeclaringType as TypeBuilder;
            declaringTypeGen = declaringTypeBuilder == null ? null : new SRTypeGeneratorImpl(declaringTypeBuilder);
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

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
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
