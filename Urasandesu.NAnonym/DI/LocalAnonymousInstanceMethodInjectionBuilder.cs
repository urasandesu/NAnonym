
namespace Urasandesu.NAnonym.DI
{
    class LocalAnonymousInstanceMethodInjectionBuilder : LocalMethodInjectionBuilder
    {
        public LocalAnonymousInstanceMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        protected override DependencyMethodBodyInjection GetMethodBodyInjection()
        {
            var methodBodyInjection = new AnonymousInstanceMethodBodyInjection(
                targetMethodInfo, CachedMethodBuilder.Name, CachedSettingBuilder.Name, LocalClassTypeBuilder);
            return methodBodyInjection;
        }
    }
}
