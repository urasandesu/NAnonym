using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class MethodInjection : Injection
    {
        public ConstructorInjection ConstructorInjection { get; private set; }
        public HashSet<InjectionMethodInfo> MethodSet { get; private set; }
        int methodCacheSequence = 0;
        public int IncreaseMethodCacheSequence()
        {
            return methodCacheSequence++;
        }
        protected int MethodCacheSequence { get { return methodCacheSequence; } }
        public MethodInjection(ConstructorInjection constructorInjection, HashSet<InjectionMethodInfo> methodSet)
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

        protected virtual void ApplyContent(InjectionMethodInfo injectionMethod)
        {
            throw new NotImplementedException();
        }
    }
}
