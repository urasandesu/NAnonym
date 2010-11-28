using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    abstract class MethodInjection : Injection
    {
        public ConstructorInjection ConstructorInjection { get; private set; }
        public HashSet<InjectionMethodInfo> MethodSet { get; private set; }
        public int IncreaseMethodCacheSequence()
        {
            return MethodCacheSequence++;
        }
        protected int MethodCacheSequence { get; private set; }
        public MethodInjection(ConstructorInjection constructorInjection, HashSet<InjectionMethodInfo> methodSet)
        {
            ConstructorInjection = constructorInjection;
            MethodSet = methodSet;
        }

        public override void Apply()
        {
            foreach (var injectionMethod in MethodSet)
            {
                var definer = GetMethodDefiner(this, injectionMethod);
                definer.Create();

                var builder = GetMethodBuilder(definer);
                builder.Construct();
            }
        }

        protected abstract MethodInjectionDefiner GetMethodDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod);
        protected abstract MethodInjectionBuilder GetMethodBuilder(MethodInjectionDefiner parentDefiner);        
    }
}
