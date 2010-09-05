using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;
using System.Runtime.Serialization;
using MC = Mono.Cecil;
using System.Reflection;
using Mono.Cecil.Cil;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    [Serializable]
    class MCMethodBaseDeclarationImpl : MCMemberDeclarationImpl, IMethodBaseDeclaration
    {
        [NonSerialized]
        MethodReference methodRef;

        [NonSerialized]
        MethodDefinition methodDef;
        string methodName;
        MC::MethodAttributes methodAttr;
        string[] parameterTypeFullNames;

        ITypeDeclaration declaringTypeDecl;

        [NonSerialized]
        IMethodBodyDeclaration bodyDecl;
        [NonSerialized]
        ReadOnlyCollection<IParameterDeclaration> parameters;

        public MCMethodBaseDeclarationImpl(MethodReference methodRef)
            : base(methodRef)
        {
            Initialize(methodRef);
        }

        void Initialize(MethodReference methodRef)
        {
            this.methodRef = methodRef;
            this.methodDef = methodRef.Resolve();
            methodName = methodDef.Name;
            methodAttr = methodDef.Attributes;
            parameterTypeFullNames = methodDef.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray();
            bodyDecl = (MCMethodBodyGeneratorImpl)methodDef.Body;
            declaringTypeDecl = (MCTypeGeneratorImpl)methodRef.DeclaringType.Resolve();
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

        public IPortableScopeItem NewPortableScopeItem(PortableScopeItemRawData itemRawData, object value)
        {
            var fieldDef = MethodDef.DeclaringType.Fields.First(field => field.Name == itemRawData.FieldName);
            var variableDef = MethodDef.Body.Variables.First(variable => variable.Index == itemRawData.LocalIndex);
            return new MCPortableScopeItemImpl(itemRawData, value, fieldDef, variableDef);
        }

        #endregion

        protected MethodDefinition MethodDef { get { return methodDef; } }
        protected IMethodBodyDeclaration BodyDecl { get { return bodyDecl; } }
        protected ITypeDeclaration DeclaringTypeDecl { get { return declaringTypeDecl; } }

        //[OnDeserialized]
        //internal new void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        var declaringTypeGen = (MCTypeGeneratorImpl)this.declaringTypeDecl;
        //        declaringTypeGen.OnDeserialized(context);
        //        var typeDef = (TypeDefinition)declaringTypeGen;
        //        Initialize(typeDef.Methods.First(
        //            method =>
        //                method.Name == methodName &&
        //                method.Attributes == methodAttr &&
        //                method.Parameters.Select(parameter => parameter.ParameterType.FullName).Equivalent(parameterTypeFullNames)));
        //        base.OnDeserialized(new StreamingContext(context.State, methodRef));
        //    }
        //}

        //public override void OnDeserialization(object sender)
        //{
        //    var declaringTypeGen = (MCTypeGeneratorImpl)this.declaringTypeDecl;
        //    var typeDef = (TypeDefinition)declaringTypeGen;
        //    var methodDef = typeDef.Methods.First(
        //        method =>
        //            method.Name == methodName &&
        //            method.Attributes == methodAttr &&
        //            method.Parameters.Select(parameter => parameter.ParameterType.FullName).Equivalent(parameterTypeFullNames)); 
        //    Initialize(methodDef);
        //    base.OnDeserialization(methodDef);
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var declaringTypeGen = (MCTypeGeneratorImpl)this.declaringTypeDecl;
            declaringTypeGen.OnDeserialized(context);
            var typeDef = (TypeDefinition)declaringTypeGen;
            var methodDef = typeDef.Methods.First(
                method =>
                    method.Name == methodName &&
                    method.Attributes == methodAttr &&
                    method.Parameters.Select(parameter => parameter.ParameterType.FullName).Equivalent(parameterTypeFullNames));
            Initialize(methodDef);
            base.OnDeserializedManually(new StreamingContext(context.State, methodDef));
        }
    }
}
