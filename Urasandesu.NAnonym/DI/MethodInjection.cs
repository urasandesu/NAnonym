using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class MethodInjection : Injection
    {
        public ConstructorInjection ConstructorInjection { get; private set; }
        public HashSet<TargetMethodInfo> MethodSet { get; private set; }
        int methodFieldSequence = 0;
        public int IncreaseMethodFieldSequence()
        {
            return methodFieldSequence++;
        }
        protected int MethodFieldSequence { get { return methodFieldSequence; } }
        public MethodInjection(ConstructorInjection constructorInjection, HashSet<TargetMethodInfo> methodSet)
        {
            ConstructorInjection = constructorInjection;
            MethodSet = methodSet;
        }

        public override void Apply()
        {
            foreach (var injectionMethod in MethodSet)
            {
                ApplyContent(injectionMethod);
            }
        }

        protected virtual void ApplyContent(TargetMethodInfo injectionMethod)
        {
            throw new NotImplementedException();
        }
    }
}
