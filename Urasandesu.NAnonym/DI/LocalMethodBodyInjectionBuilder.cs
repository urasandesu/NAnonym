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
