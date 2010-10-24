using System;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalAnonymousInstanceMethodInjectionBuilder : GlobalMethodInjectionBuilder
    {
        public GlobalAnonymousInstanceMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        protected override DependencyMethodBodyInjection GetMethodBodyInjection()
        {
            var methodBodyInjection = new AnonymousInstanceMethodBodyInjection(
                targetMethodInfo, CachedMethodDef.Name, CachedSettingDef.Name, Type.GetType(TBaseTypeDef.GetAssemblyQualifiedName()));
            return methodBodyInjection;
        }
    }
}
