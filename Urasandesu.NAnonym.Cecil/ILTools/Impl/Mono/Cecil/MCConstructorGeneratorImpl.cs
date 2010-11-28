using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UNI = Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCConstructorGeneratorImpl : MCConstructorDeclarationImpl, UNI::IConstructorGenerator
    {
        MCMethodBaseGeneratorImpl methodBaseGen;

        public MCConstructorGeneratorImpl(MethodDefinition constructorDef)
            : base(constructorDef)
        {
            methodBaseGen = new MCMethodBaseGeneratorImpl(constructorDef);
        }


        public new UNI::IMethodBodyGenerator Body
        {
            get { return methodBaseGen.Body; }
        }

        public new UNI::ITypeGenerator DeclaringType
        {
            get { return methodBaseGen.DeclaringType; }
        }

        public new ReadOnlyCollection<UNI::IParameterGenerator> Parameters
        {
            get { return methodBaseGen.Parameters; }
        }

        public UNI::IPortableScopeItem AddPortableScopeItem(SR::FieldInfo fieldInfo)
        {
            return methodBaseGen.AddPortableScopeItem(fieldInfo);
        }

        public UNI::IMethodBaseGenerator ExpressBody(Action<UNI::ExpressiveMethodBodyGenerator> bodyExpression)
        {
            methodBaseGen.ExpressBody(bodyExpression);
            return this;
        }

        public UNI::IParameterGenerator AddParameter(int position, SR::ParameterAttributes attributes, string parameterName)
        {
            return methodBaseGen.AddParameter(position, attributes, parameterName);
        }

        public UNI::PortableScope CarryPortableScope()
        {
            return methodBaseGen.CarryPortableScope();
        }
    }
}
