using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    // NOTE: Method や Constructor は Declaration と Generator が一段離れるイメージ。
    sealed class SRConstructorGeneratorImpl : SRConstructorDeclarationImpl, IConstructorGenerator
    {
        readonly ConstructorBuilder constructorBuilder;

        readonly IMethodBodyGenerator bodyGen;
        readonly ITypeGenerator declaringTypeGen;

        List<ParameterBuilder> parameterBuilders;
        ReadOnlyCollection<IParameterGenerator> parameters;

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder)
            : this(constructorBuilder, new ParameterBuilder[] { }, new FieldBuilder[] { })
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, ParameterBuilder[] parameterBuilders)
            : this(constructorBuilder, parameterBuilders, new FieldBuilder[] { })
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, FieldBuilder[] fieldBuilders)
            : this(constructorBuilder, new ParameterBuilder[] { }, fieldBuilders)
        {
        }

        public SRConstructorGeneratorImpl(ConstructorBuilder constructorBuilder, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
            : base(constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            bodyGen = new SRMethodBodyGeneratorImpl(constructorBuilder);
            this.parameterBuilders = new List<ParameterBuilder>(parameterBuilders);
            parameters = new ReadOnlyCollection<IParameterGenerator>(
                this.parameterBuilders.TransformEnumerateOnly(parameterBuilder => (IParameterGenerator)new SRParameterGeneratorImpl(parameterBuilder)));
            var declaringTypeBuilder = constructorBuilder.DeclaringType as TypeBuilder;
            declaringTypeGen = declaringTypeBuilder == null ? null : new SRTypeGeneratorImpl(declaringTypeBuilder, fieldBuilders);
        }

        public new IMethodBodyGenerator Body
        {
            get { return bodyGen; }
        }

        #region IMethodBaseGenerator メンバ

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return parameters; }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        public new ITypeGenerator DeclaringType
        {
            get { return declaringTypeGen; }
        }

        public IMethodBaseGenerator CreateInstance(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}
