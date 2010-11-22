using System.Linq;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalReplaceMethodInjectionDefiner : GlobalMethodInjectionDefiner
    {
        public GlobalReplaceMethodInjectionDefiner(GlobalMethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override MethodDefinition GetMethodInterface()
        {
            var source = Parent.ConstructorInjection.DeclaringTypeDef.Methods.FirstOrDefault(methodDef => methodDef.Equivalent(InjectionMethod.Source));
            string sourceName = source.Name;
            source.Name = "__" + source.Name;
            baseMethod = new MCMethodDeclarationImpl(source);

            var destination = source.DuplicateWithoutBody();
            destination.Name = sourceName;
            Parent.ConstructorInjection.DeclaringTypeDef.Methods.Add(destination);

            return destination;
        }

        IMethodDeclaration baseMethod;
        public override IMethodDeclaration BaseMethod
        {
            get { return baseMethod; }
        }
    }
}
