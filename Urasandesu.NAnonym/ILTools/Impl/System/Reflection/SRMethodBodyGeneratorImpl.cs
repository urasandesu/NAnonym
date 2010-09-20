using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    sealed class SRMethodBodyGeneratorImpl : SRMethodBodyDeclarationImpl, IMethodBodyGenerator
    {
        // TODO: SR ～、MC ～ がメンバに持つ変数の型は、無理にインターフェースにする必要はない。

        readonly ConstructorBuilder constructorBuilder;
        readonly DynamicMethod dynamicMethod;
        SRILOperatorImpl il;
        public SRMethodBodyGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            Initialize((SRILOperatorImpl)constructorBuilder.GetILGenerator());
        }

        public SRMethodBodyGeneratorImpl(DynamicMethod dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            Initialize((SRILOperatorImpl)dynamicMethod.GetILGenerator());
        }

        void Initialize(SRILOperatorImpl il)
        {
            this.il = il;
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
            get { return il.Directives; }
        }
    }
}
