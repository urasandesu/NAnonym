using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    class MCMethodBaseDeclarationImpl : MCMemberDeclarationImpl, IMethodBaseDeclaration
    {
        readonly MethodReference methodRef;

        readonly IMethodBodyDeclaration bodyDecl;
        readonly ITypeDeclaration declaringTypeDecl;
        readonly ReadOnlyCollection<IParameterDeclaration> parameters;
        public MCMethodBaseDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
            this.methodRef = methodRef;
            var methodDef = methodRef as MethodDefinition;
            bodyDecl = methodDef == null ? null : (MCMethodBodyGeneratorImpl)methodDef.Body;
            declaringTypeDecl = (MCTypeDeclarationImpl)methodRef.DeclaringType;
            parameters = new ReadOnlyCollection<IParameterDeclaration>(
                methodRef.Parameters.TransformEnumerateOnly(parameter => (IParameterDeclaration)(MCParameterGeneratorImpl)parameter));
        }



        #region IMethodBaseDeclaration メンバ

        public IMethodBodyDeclaration Body
        {
            get { return bodyDecl; }
        }

        public ITypeDeclaration DeclaringType
        {
            get { return declaringTypeDecl; }
        }

        public ReadOnlyCollection<IParameterDeclaration> Parameters
        {
            get { return parameters; }
        }

        #endregion
    }
}
