using System;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodBodyInjectionBuilder : MethodBodyInjectionBuilder
    {
        public new GlobalMethodBodyInjectionDefiner ParentBodyDefiner { get { return (GlobalMethodBodyInjectionDefiner)base.ParentBodyDefiner; } }
        protected GlobalMethodBodyInjectionBuilder(GlobalMethodBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public static MethodBodyInjectionBuilder GetInstance(GlobalMethodBodyInjectionDefiner parentBodyDefiner)
        {
            var injectionMethod = parentBodyDefiner.ParentBody.ParentBuilder.ParentDefiner.InjectionMethod;
            if ((injectionMethod.DestinationType & MethodBodyInjectionBuilderType.AnonymousInstanceWithBase) == MethodBodyInjectionBuilderType.AnonymousInstanceWithBase)
            {
                return new AnonymousInstanceMethodBodyInjectionBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyInjectionBuilderType.AnonymousInstance) == MethodBodyInjectionBuilderType.AnonymousInstance)
            {
                return new AnonymousInstanceMethodBodyInjectionBuilder(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyInjectionBuilderType.AnonymousStaticWithBase) == MethodBodyInjectionBuilderType.AnonymousStaticWithBase)
            {
                return new AnonymousStaticMethodBodyInjectionBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyInjectionBuilderType.AnonymousStatic) == MethodBodyInjectionBuilderType.AnonymousStatic)
            {
                return new AnonymousStaticMethodBodyInjectionBuilder(parentBodyDefiner);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
