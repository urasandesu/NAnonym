using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodWeaver : Weaver
    {
        public ConstructorWeaver ConstructorWeaver { get; private set; }
        public HashSet<WeaveMethodInfo> MethodSet { get; private set; }
        public int IncreaseMethodCacheSequence()
        {
            return MethodCacheSequence++;
        }
        protected int MethodCacheSequence { get; private set; }
        public MethodWeaver(ConstructorWeaver constructorWeaver, HashSet<WeaveMethodInfo> methodSet)
        {
            ConstructorWeaver = constructorWeaver;
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

        protected abstract MethodWeaveDefiner GetMethodDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod);
        protected abstract MethodWeaveBuilder GetMethodBuilder(MethodWeaveDefiner parentDefiner);        
    }
}
