using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    // NOTE: Method や Constructor は Declaration と Generator が一段離れるイメージ。
    sealed class SRConstructorGeneratorImpl : SRConstructorDeclarationImpl, IConstructorGenerator, ISRMethodBaseGenerator
    {
        SRMethodBaseGeneratorImpl methodGen;

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
            this.methodGen = new SRMethodBaseGeneratorImpl(this, constructorBuilder, parameterBuilders, fieldBuilders);
        }

        public SRConstructorGeneratorImpl(ITypeGenerator declaringTypeGen, ConstructorBuilder constructorBuilder)
            : base(constructorBuilder)
        {
            this.methodGen = new SRMethodBaseGeneratorImpl(declaringTypeGen, this, constructorBuilder);
        }

        internal new ConstructorBuilder Source { get { return (ConstructorBuilder)base.Source; } }

        public new IMethodBodyGenerator Body
        {
            get { return methodGen.Body; }
        }

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return methodGen.Parameters; }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            return methodGen.AddPortableScopeItem(fieldInfo);
        }

        public new ITypeGenerator DeclaringType
        {
            get { return methodGen.DeclaringType; }
        }

        public IMethodBaseGenerator CreateInstance(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return methodGen.CreateInstance(name, attributes, returnType, parameterTypes);
        }

        public IMethodBaseGenerator ExpressBody(Action<ExpressiveMethodBodyGenerator> bodyExpression)
        {
            methodGen.ExpressBody(bodyExpression);
            return this;
        }

        public IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return methodGen.AddParameter(position, attributes, parameterName);
        }

        public ILGenerator GetILGenerator()
        {
            return Source.GetILGenerator();
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return Source.DefineParameter(position, attributes, parameterName);
        }
    }
}
