using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    class MCParameterGeneratorImpl : MCParameterDeclarationImpl, UN::ILTools.IParameterGenerator
    {
        readonly ParameterDefinition parameterDef;

        readonly UN::ILTools.ITypeGenerator parameterTypeGen;
        public MCParameterGeneratorImpl(ParameterDefinition parameterDef)
            : base(parameterDef)
        {
            this.parameterDef = parameterDef;
            var typeDef = parameterDef.ParameterType as TypeDefinition;
            parameterTypeGen = typeDef == null ? null : new MCTypeGeneratorImpl(typeDef);
        }

        internal ParameterDefinition ParameterDef { get { return parameterDef; } }

        public new UN::ILTools.ITypeGenerator ParameterType
        {
            get { return parameterTypeGen; }
        }
    }
}
