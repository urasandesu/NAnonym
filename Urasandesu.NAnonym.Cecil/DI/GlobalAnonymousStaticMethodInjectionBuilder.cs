using Urasandesu.NAnonym.DI;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalAnonymousStaticMethodInjectionBuilder : GlobalMethodInjectionBuilder
    {
        public GlobalAnonymousStaticMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        protected override DependencyMethodBodyInjection GetMethodBodyInjection()
        {
            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(targetMethodInfo.NewMethod);
            var methodBodyInjection = new AnonymousStaticMethodBodyInjection(targetMethodInfo, CachedMethodDef.Name, tmpCacheField);
            return methodBodyInjection;
        }
    }
}
