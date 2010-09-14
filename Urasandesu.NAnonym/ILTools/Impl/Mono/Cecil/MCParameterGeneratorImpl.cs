using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    class MCParameterGeneratorImpl : MCParameterDeclarationImpl, IParameterGenerator
    {
        readonly ParameterDefinition parameterDef;

        readonly ITypeGenerator parameterTypeGen;
        public MCParameterGeneratorImpl(ParameterDefinition parameterDef)
            : base(parameterDef)
        {
            this.parameterDef = parameterDef;
            var typeDef = parameterDef.ParameterType as TypeDefinition;
            parameterTypeGen = typeDef == null ? null : (MCTypeGeneratorImpl)typeDef;
        }

        public static explicit operator MCParameterGeneratorImpl(ParameterDefinition parameterDef)
        {
            return new MCParameterGeneratorImpl(parameterDef);
        }

        public static explicit operator ParameterDefinition(MCParameterGeneratorImpl parameterGen)
        {
            return parameterGen.parameterDef;
        }

        public new ITypeGenerator ParameterType
        {
            get { return parameterTypeGen; }
        }
    }
}
