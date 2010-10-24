using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalAnonymousStaticMethodInjectionBuilder : LocalMethodInjectionBuilder
    {
        public LocalAnonymousStaticMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        protected override DependencyMethodBodyInjection GetMethodBodyInjection()
        {
            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(targetMethodInfo.NewMethod);
            var methodBodyInjection = new AnonymousStaticMethodBodyInjection(targetMethodInfo, CachedMethodBuilder.Name, tmpCacheField);
            return methodBodyInjection;
        }
    }
}
