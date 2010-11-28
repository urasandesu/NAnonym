using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRDynamicMethodGeneratorImpl : SRMethodDeclarationImpl, IMethodGenerator, ISRMethodBaseGenerator
    {
        SRMethodBaseGeneratorImpl methodBaseGen;

        public SRDynamicMethodGeneratorImpl(DynamicMethod dynamicMethod)
            : this(dynamicMethod, new ParameterBuilder[] { }, new FieldBuilder[] { })
        {
        }

        public SRDynamicMethodGeneratorImpl(DynamicMethod dynamicMethod, ParameterBuilder[] parameterBuilders)
            : this(dynamicMethod, parameterBuilders, new FieldBuilder[] { })
        {
        }

        public SRDynamicMethodGeneratorImpl(DynamicMethod dynamicMethod, FieldBuilder[] fieldBuilders)
            : this(dynamicMethod, new ParameterBuilder[] { }, fieldBuilders)
        {
        }

        public SRDynamicMethodGeneratorImpl(DynamicMethod dynamicMethod, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
            : base(dynamicMethod)
        {
            this.methodBaseGen = new SRMethodBaseGeneratorImpl(this, dynamicMethod, parameterBuilders, fieldBuilders);
        }

        internal new DynamicMethod Source { get { return (DynamicMethod)base.Source; } }

        public new ITypeGenerator ReturnType
        {
            get { throw new NotImplementedException(); }
        }

        public new IMethodBodyGenerator Body
        {
            get { return methodBaseGen.Body; }
        }

        public new ITypeGenerator DeclaringType
        {
            get { return methodBaseGen.DeclaringType; }
        }

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return methodBaseGen.Parameters; }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator CreateInstance(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            throw new NotImplementedException();
        }

        public IMethodBaseGenerator ExpressBody(Action<ExpressiveMethodBodyGenerator> bodyExpression)
        {
            methodBaseGen.ExpressBody(bodyExpression);
            return this;
        }

        public IParameterGenerator AddParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return methodBaseGen.AddParameter(position, attributes, parameterName);
        }

        public ILGenerator GetILGenerator()
        {
            return Source.GetILGenerator();
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            return Source.DefineParameter(position, attributes, parameterName);
        }

        public PortableScope CarryPortableScope()
        {
            throw new NotImplementedException();
        }
    }
}
