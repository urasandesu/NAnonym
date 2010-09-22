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

        ConstructorBuilder constructorBuilder;
        DynamicMethod dynamicMethod;
        MethodBuilder methodBuilder;
        SRILOperatorImpl il;

        ReadOnlyCollection<ILocalGenerator> readonlyLocalGens;
        
        public SRMethodBodyGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            this.constructorBuilder = constructorBuilder;
            Initialize(new SRILOperatorImpl(constructorBuilder.GetILGenerator(), this));
        }

        public SRMethodBodyGeneratorImpl(DynamicMethod dynamicMethod)
        {
            this.dynamicMethod = dynamicMethod;
            Initialize(new SRILOperatorImpl(dynamicMethod.GetILGenerator(), this));
        }

        public SRMethodBodyGeneratorImpl(MethodBuilder methodBuilder)
        {
            this.methodBuilder = methodBuilder;
            Initialize(new SRILOperatorImpl(methodBuilder.GetILGenerator(), this));
        }

        void Initialize(SRILOperatorImpl il)
        {
            this.il = il;
            LocalGens = new List<SRLocalGeneratorImpl>();
            readonlyLocalGens = new ReadOnlyCollection<ILocalGenerator>(
                LocalGens.TransformEnumerateOnly(localGen => (ILocalGenerator)localGen));
        }

        public static explicit operator SRMethodBodyGeneratorImpl(ConstructorBuilder constructorBuilder)
        {
            return new SRMethodBodyGeneratorImpl(constructorBuilder);
        }

        public static explicit operator SRMethodBodyGeneratorImpl(DynamicMethod dynamicMethod)
        {
            return new SRMethodBodyGeneratorImpl(dynamicMethod);
        }

        public static explicit operator SRMethodBodyGeneratorImpl(MethodBuilder methodBuilder)
        {
            return new SRMethodBodyGeneratorImpl(methodBuilder);
        }

        public IILOperator GetILOperator()
        {
            return il;
        }

        public new ReadOnlyCollection<ILocalGenerator> Locals
        {
            get { return readonlyLocalGens; }
        }

        public new ReadOnlyCollection<IDirectiveGenerator> Directives
        {
            get { return il.Directives; }
        }

        internal List<SRLocalGeneratorImpl> LocalGens { get; private set; }
    }
}
