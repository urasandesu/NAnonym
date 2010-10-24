using System;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    abstract class LocalMethodInjectionBuilder
    {
        protected readonly TargetMethodInfo targetMethodInfo;
        public LocalMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
        {
            this.targetMethodInfo = targetMethodInfo;
        }

        public static LocalMethodInjectionBuilder Create(TargetMethodInfo targetMethodInfo)
        {
            if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
            {
                return new LocalAnonymousInstanceMethodInjectionBuilder(targetMethodInfo);
            }
            else if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
            {
                return new LocalAnonymousStaticMethodInjectionBuilder(targetMethodInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public TypeBuilder LocalClassTypeBuilder { get; set; }
        public FieldBuilder CachedSettingBuilder { get; set; }
        public MethodBuilder MethodBuilder { get; set; }
        public FieldBuilder CachedMethodBuilder { get; set; }
        public ParameterBuilder[] ParameterBuilders { get; set; }

        public void Construct()
        {
            var methodBodyInjection = GetMethodBodyInjection();
            MethodBuilder.ExpressBody(
            gen =>
            {
                methodBodyInjection.Apply(gen);
            },
            ParameterBuilders,
            new FieldBuilder[] { CachedMethodBuilder });
        }

        protected abstract DependencyMethodBodyInjection GetMethodBodyInjection();
    }
}
