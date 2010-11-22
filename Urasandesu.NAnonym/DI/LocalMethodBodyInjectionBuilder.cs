using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodBodyInjectionBuilder : MethodBodyInjectionBuilder
    {
        public new LocalMethodBodyInjectionDefiner ParentBodyDefiner { get { return (LocalMethodBodyInjectionDefiner)base.ParentBodyDefiner; } }
        protected LocalMethodBodyInjectionBuilder(LocalMethodBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public static MethodBodyInjectionBuilder GetInstance(LocalMethodBodyInjectionDefiner parentBodyDefiner)
        {
            var injectionMethod = parentBodyDefiner.ParentBody.ParentBuilder.ParentDefiner.InjectionMethod;
            if ((injectionMethod.DestinationType & MethodBodyInjectionBuilderType.AnonymousInstance) == MethodBodyInjectionBuilderType.AnonymousInstance)
            {
                return new AnonymousInstanceMethodBodyInjectionBuilder(parentBodyDefiner);
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
