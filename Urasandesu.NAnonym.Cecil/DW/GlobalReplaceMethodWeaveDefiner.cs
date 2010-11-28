using System.Linq;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalReplaceMethodWeaveDefiner : GlobalMethodWeaveDefiner
    {
        public GlobalReplaceMethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override UNI::IMethodGenerator GetMethodInterface()
        {
            var declaringTypeDef = ((MCTypeGeneratorImpl)Parent.ConstructorWeaver.DeclaringTypeGenerator).TypeDef;
            var source = declaringTypeDef.Methods.FirstOrDefault(methodDef => methodDef.Equivalent(WeaveMethod.Source));
            string sourceName = source.Name;
            source.Name = "__" + source.Name;
            baseMethod = new MCMethodGeneratorImpl(source);

            var destination = source.DuplicateWithoutBody();
            destination.Name = sourceName;
            declaringTypeDef.Methods.Add(destination);
            var destinationGen = new MCMethodGeneratorImpl(destination);

            return destinationGen;
        }

        UNI::IMethodDeclaration baseMethod;
        public override UNI::IMethodDeclaration BaseMethod
        {
            get { return baseMethod; }
        }
    }
}
