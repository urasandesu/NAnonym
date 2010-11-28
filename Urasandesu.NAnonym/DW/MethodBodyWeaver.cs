using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodBodyWeaver : BodyWeaver
    {
        public new MethodWeaveBuilder ParentBuilder { get { return (MethodWeaveBuilder)base.ParentBuilder; } }
        public MethodBodyWeaver(ExpressiveMethodBodyGenerator gen, MethodWeaveBuilder parentBuilder)
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

        protected abstract MethodBodyWeaveDefiner GetMethodBodyDefiner(MethodBodyWeaver parentBody);
        protected virtual MethodBodyWeaveBuilder GetMethodBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
        {
            var injectionMethod = parentBodyDefiner.ParentBody.ParentBuilder.ParentDefiner.WeaveMethod;
            if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousInstanceWithBase) == MethodBodyWeaveBuilderType.AnonymousInstanceWithBase)
            {
                return new AnonymousInstanceMethodBodyWeaveBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousInstance) == MethodBodyWeaveBuilderType.AnonymousInstance)
            {
                return new AnonymousInstanceMethodBodyWeaveBuilder(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousStaticWithBase) == MethodBodyWeaveBuilderType.AnonymousStaticWithBase)
            {
                return new AnonymousStaticMethodBodyWeaveBuilderWithBase(parentBodyDefiner);
            }
            else if ((injectionMethod.DestinationType & MethodBodyWeaveBuilderType.AnonymousStatic) == MethodBodyWeaveBuilderType.AnonymousStatic)
            {
                return new AnonymousStaticMethodBodyWeaveBuilder(parentBodyDefiner);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
