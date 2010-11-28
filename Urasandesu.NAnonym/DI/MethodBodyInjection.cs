using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    abstract class MethodBodyInjection : BodyInjection
    {
        public new MethodInjectionBuilder ParentBuilder { get { return (MethodInjectionBuilder)base.ParentBuilder; } }
        public MethodBodyInjection(ExpressiveMethodBodyGenerator gen, MethodInjectionBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = GetMethodBodyDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GetMethodBodyBuilder(bodyDefiner);
            bodyBuilder.Construct();
        }

        protected abstract MethodBodyInjectionDefiner GetMethodBodyDefiner(MethodBodyInjection parentBody);
        protected virtual MethodBodyInjectionBuilder GetMethodBodyBuilder(MethodBodyInjectionDefiner parentBodyDefiner)
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
