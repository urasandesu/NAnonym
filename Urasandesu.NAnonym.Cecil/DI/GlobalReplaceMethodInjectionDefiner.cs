using System.Linq;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalReplaceMethodInjectionDefiner : GlobalMethodInjectionDefiner
    {
        public GlobalReplaceMethodInjectionDefiner(GlobalMethodInjection parent, TargetMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override MethodDefinition GetMethodInterface()
        {
            var oldMethodDef = Parent.ConstructorInjection.DeclaringTypeDef.Methods.FirstOrDefault(_methodDef => _methodDef.Equivalent(InjectionMethod.OldMethod));
            string oldMethodName = oldMethodDef.Name;
            oldMethodDef.Name = "__" + oldMethodDef.Name;

            var newMethod = oldMethodDef.DuplicateWithoutBody();
            newMethod.Name = oldMethodName;
            Parent.ConstructorInjection.DeclaringTypeDef.Methods.Add(newMethod);

            return newMethod;
        }
    }
}
