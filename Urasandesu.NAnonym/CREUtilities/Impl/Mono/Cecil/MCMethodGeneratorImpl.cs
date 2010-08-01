using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using MCO = Mono.Collections;
using MC = Mono.Cecil;
using System.Collections.ObjectModel;
using System.Collections;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    sealed class MCMethodGeneratorImpl : MCMethodDeclarationImpl, IMethodGenerator
    {
        readonly MethodDefinition methodDef;

        readonly IMethodBodyGenerator bodyGen;
        readonly ITypeGenerator returnTypeGen;
        readonly ITypeGenerator declaringTypeGen;
        readonly ReadOnlyCollection<IParameterGenerator> parameters;
        public MCMethodGeneratorImpl(MethodDefinition methodDef)
            : base(methodDef)
        {
            this.methodDef = methodDef;
            bodyGen = (MCMethodBodyGeneratorImpl)methodDef.Body;
            var typeDef = methodDef.ReturnType as TypeDefinition;
            returnTypeGen = typeDef == null ? null : (MCTypeGeneratorImpl)typeDef;
            declaringTypeGen = (MCTypeGeneratorImpl)methodDef.DeclaringType;
            parameters = new ReadOnlyCollection<IParameterGenerator>(
                methodDef.Parameters.TransformEnumerateOnly(parameter => (IParameterGenerator)(MCParameterGeneratorImpl)parameter));
        }

        public static explicit operator MCMethodGeneratorImpl(MethodDefinition methodDef)
        {
            return new MCMethodGeneratorImpl(methodDef);
        }

        public static explicit operator MethodDefinition(MCMethodGeneratorImpl methodGen)
        {
            return methodGen.methodDef;
        }

        #region IMethodGenerator メンバ

        public new ITypeGenerator ReturnType
        {
            get { return returnTypeGen; }
        }

        #endregion

        #region IMethodBaseGenerator メンバ

        public new IMethodBodyGenerator Body
        {
            get { return bodyGen; }
        }

        public new ITypeGenerator DeclaringType
        {
            get { return declaringTypeGen; }
        }

        public new ReadOnlyCollection<IParameterGenerator> Parameters
        {
            get { return parameters; }
        }

        #endregion
    }
}
