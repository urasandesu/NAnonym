using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRMethodGeneratorImpl : SRMethodDeclarationImpl, IMethodGenerator
    {
        readonly MethodBuilder methodBuilder;
        readonly DynamicMethod dynamicMethod;

        readonly IMethodBodyGenerator bodyGen;
        readonly ITypeGenerator declaringTypeGen;
        public SRMethodGeneratorImpl(MethodBuilder methodBuilder)
            : base(methodBuilder)
        {
            this.methodBuilder = methodBuilder;
        }

        public SRMethodGeneratorImpl(DynamicMethod dynamicMethod)
            : base(dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            bodyGen = (SRMethodBodyGeneratorImpl)dynamicMethod;
            var declaringTypeBuilder = dynamicMethod.DeclaringType as TypeBuilder;
            declaringTypeGen = declaringTypeBuilder == null ? null : (SRTypeGeneratorImpl)declaringTypeBuilder;
        }

        public static explicit operator SRMethodGeneratorImpl(DynamicMethod dynamicMethod)
        {
            return new SRMethodGeneratorImpl(dynamicMethod);
        }

        public static explicit operator DynamicMethod(SRMethodGeneratorImpl methodGen)
        {
            return methodGen.dynamicMethod;
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
            get { throw new NotImplementedException(); }
        }

        public IPortableScopeItem AddPortableScopeItem(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
