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
            if ((injectionMethod.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
            {
                return new AnonymousInstanceMethodBodyInjectionBuilder(parentBodyDefiner);
            }
            else if ((injectionMethod.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
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
