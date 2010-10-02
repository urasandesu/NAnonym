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

        public SRMethodGeneratorImpl(MethodBuilder methodBuilder)
            : this(methodBuilder, new ParameterBuilder[] { }, new FieldBuilder[] { })
        {
        }

        public SRMethodGeneratorImpl(MethodBuilder methodBuilder, ParameterBuilder[] parameterBuilders)
            : this(methodBuilder, parameterBuilders, new FieldBuilder[] { })
        {
        }

        public SRMethodGeneratorImpl(MethodBuilder methodBuilder, FieldBuilder[] fieldBuilders)
            : this(methodBuilder, new ParameterBuilder[] { }, fieldBuilders)
        {
        }

        public SRMethodGeneratorImpl(MethodBuilder methodBuilder, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
            : base(methodBuilder)
        {
            this.methodBuilder = methodBuilder;
            Initialize(methodBuilder, parameterBuilders, fieldBuilders);
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod)
            : this(dynamicMethod, new ParameterBuilder[] { }, new FieldBuilder[] { })
        {
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod, ParameterBuilder[] parameterBuilders)
            : this(dynamicMethod, parameterBuilders, new FieldBuilder[] { })
        {
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod, FieldBuilder[] fieldBuilders)
            : this(dynamicMethod, new ParameterBuilder[] { }, fieldBuilders)
        {
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
            : base(dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            Initialize(dynamicMethod, parameterBuilders, fieldBuilders);
        }
        
        void Initialize(MethodBuilder methodBuilder, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
        {
            bodyGen = new SRMethodBodyGeneratorImpl(methodBuilder);
            // TODO: ParameterBuilder 自身から型情報を取得する機能はない。MethodBuilder とセットにして使わないといけない。
            //parameters = 
            //    new ReadOnlyCollection<IParameterGenerator>(
            //        methodBuilder.GetParameters().TransformEnumerateOnly(parameter => (IParameterGenerator)(SRParameterGeneratorImpl)parameter));
            Initialize(methodBuilder.DeclaringType as TypeBuilder, parameterBuilders, fieldBuilders);
        }

        void Initialize(DynamicMethod dynamicMethod, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
        {
            bodyGen = new SRMethodBodyGeneratorImpl(dynamicMethod);
            Initialize(dynamicMethod.DeclaringType as TypeBuilder, parameterBuilders, fieldBuilders);
        }

        void Initialize(TypeBuilder declaringTypeBuilder, ParameterBuilder[] parameterBuilders, FieldBuilder[] fieldBuilders)
        {
            declaringTypeGen = declaringTypeBuilder == null ? null : new SRTypeGeneratorImpl(declaringTypeBuilder, fieldBuilders);
            this.parameterBuilders = new List<ParameterBuilder>(parameterBuilders);
            parameters = new ReadOnlyCollection<IParameterGenerator>(
                this.parameterBuilders.TransformEnumerateOnly(parameterBuilder => (IParameterGenerator)new SRParameterGeneratorImpl(parameterBuilder)));
        }

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
