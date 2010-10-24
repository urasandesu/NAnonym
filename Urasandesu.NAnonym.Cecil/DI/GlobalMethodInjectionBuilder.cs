using System;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    abstract class GlobalMethodInjectionBuilder
    {
        protected readonly TargetMethodInfo targetMethodInfo;
        public GlobalMethodInjectionBuilder(TargetMethodInfo targetMethodInfo)
        {
            this.targetMethodInfo = targetMethodInfo;
        }

        public static GlobalMethodInjectionBuilder Create(TargetMethodInfo targetMethodInfo)
        {
            if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
            {
                return new GlobalAnonymousInstanceMethodInjectionBuilder(targetMethodInfo);
            }
            else if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
            {
                return new GlobalAnonymousStaticMethodInjectionBuilder(targetMethodInfo);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public TypeDefinition TBaseTypeDef { get; set; }
        public MethodDefinition NewMethod { get; set; }
        public FieldDefinition CachedMethodDef { get; set; }
        public FieldDefinition CachedSettingDef { get; set; }

        public void Construct()
        {
            var methodBodyInjection = GetMethodBodyInjection();
            NewMethod.Body.InitLocals = true;
            NewMethod.ExpressBody(
            gen =>
            {
                methodBodyInjection.Apply(gen);
            });
        }

        protected abstract DependencyMethodBodyInjection GetMethodBodyInjection();
    }
}
