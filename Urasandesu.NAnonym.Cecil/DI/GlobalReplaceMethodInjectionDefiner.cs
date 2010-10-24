using System.Linq;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalReplaceMethodInjectionDefiner : GlobalMethodInjectionDefiner
    {
        public GlobalReplaceMethodInjectionDefiner(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        public override MethodDefinition DefineMethod(TypeDefinition tbaseTypeDef)
        {
            var oldMethodDef = tbaseTypeDef.Methods.FirstOrDefault(_methodDef => _methodDef.Equivalent(targetMethodInfo.OldMethod));
            string oldMethodName = oldMethodDef.Name;
            oldMethodDef.Name = "__" + oldMethodDef.Name;

            var newMethod = oldMethodDef.DuplicateWithoutBody();
            newMethod.Name = oldMethodName;
            tbaseTypeDef.Methods.Add(newMethod);

            return newMethod;
        }
    }
}
