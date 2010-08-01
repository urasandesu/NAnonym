using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.CREUtilities.Impl.System.Reflection
{
    sealed class SRMethodBodyGeneratorImpl : SRMethodBodyDeclarationImpl, IMethodBodyGenerator
    {
        readonly ConstructorBuilder constructorBuilder;
        readonly DynamicMethod dynamicMethod;
        readonly SRILOperatorImpl il;
        public SRMethodBodyGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            il = (SRILOperatorImpl)constructorBuilder.GetILGenerator();
        }

        public SRMethodBodyGeneratorImpl(DynamicMethod dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            il = (SRILOperatorImpl)dynamicMethod.GetILGenerator();
        }

        public static explicit operator SRMethodBodyGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            return new SRMethodBodyGeneratorImpl(constructorBuilder);
        }

        public static explicit operator SRMethodBodyGeneratorImpl(DynamicMethod dynamicMethod)
        {
            return new SRMethodBodyGeneratorImpl(dynamicMethod);
        }

        public IILOperator GetILOperator()
        {
            return il;
        }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return new ReadOnlyCollection<ILocalGenerator>(new ILocalGenerator[] { }); }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { throw new NotImplementedException(); }
        }
    }
}
