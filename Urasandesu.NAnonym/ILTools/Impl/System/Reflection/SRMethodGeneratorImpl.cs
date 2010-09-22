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
    class SRMethodGeneratorImpl : SRMethodDeclarationImpl, IMethodGenerator
    {
        MethodBuilder methodBuilder;
        DynamicMethod dynamicMethod;

        IMethodBodyGenerator bodyGen;
        ITypeGenerator declaringTypeGen;

        List<ParameterBuilder> parameterBuilders;
        ReadOnlyCollection<IParameterGenerator> parameters;

        public SRMethodGeneratorImpl(MethodBuilder methodBuilder, params ParameterBuilder[] parameterBuilders)
            : base(methodBuilder)
        {
            this.methodBuilder = methodBuilder;
            Initialize(methodBuilder, parameterBuilders);
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod, params ParameterBuilder[] parameterBuilders)
            : base(dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            Initialize(dynamicMethod, parameterBuilders);
        }

        void Initialize(MethodBuilder methodBuilder, params ParameterBuilder[] parameterBuilders)
        {
            bodyGen = (SRMethodBodyGeneratorImpl)methodBuilder;
            // TODO: ParameterBuilder 自身から型情報を取得する機能はない。MethodBuilder とセットにして使わないといけない。
            //parameters = 
            //    new ReadOnlyCollection<IParameterGenerator>(
            //        methodBuilder.GetParameters().TransformEnumerateOnly(parameter => (IParameterGenerator)(SRParameterGeneratorImpl)parameter));
            Initialize(methodBuilder.DeclaringType as TypeBuilder, parameterBuilders);
        }

        void Initialize(DynamicMethod dynamicMethod, params ParameterBuilder[] parameterBuilders)
        {
            bodyGen = (SRMethodBodyGeneratorImpl)dynamicMethod;
            Initialize(dynamicMethod.DeclaringType as TypeBuilder, parameterBuilders);
        }

        void Initialize(TypeBuilder declaringTypeBuilder, params ParameterBuilder[] parameterBuilders)
        {
            declaringTypeGen = declaringTypeBuilder == null ? null : (SRTypeGeneratorImpl)declaringTypeBuilder;
            this.parameterBuilders = new List<ParameterBuilder>(parameterBuilders);
            parameters = new ReadOnlyCollection<IParameterGenerator>(
                this.parameterBuilders.TransformEnumerateOnly(parameterBuilder => (IParameterGenerator)new SRParameterGeneratorImpl(parameterBuilder)));
        }

        //public static explicit operator SRMethodGeneratorImpl(DynamicMethod dynamicMethod)
        //{
        //    return new SRMethodGeneratorImpl(dynamicMethod);
        //}

        //public static explicit operator DynamicMethod(SRMethodGeneratorImpl methodGen)
        //{
        //    return methodGen.dynamicMethod;
        //}

        //public static explicit operator SRMethodGeneratorImpl(MethodBuilder methodBuilder)
        //{
        //    return new SRMethodGeneratorImpl(methodBuilder);
        //}

        #region IMethodGenerator メンバ

        public new ITypeGenerator ReturnType
        {
            get { throw new NotImplementedException(); }
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

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
