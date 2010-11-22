using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjection : MethodInjection
    {
        public new LocalConstructorInjection ConstructorInjection { get { return (LocalConstructorInjection)base.ConstructorInjection; } }
        public LocalMethodInjection(
            LocalConstructorInjection constructorInjection,
            HashSet<InjectionMethodInfo> methodSet)
            : base(constructorInjection, methodSet)
        {
        }

        protected override void ApplyContent(InjectionMethodInfo injectionMethod)
        {
            var definer = LocalMethodInjectionDefiner.GetInstance(this, injectionMethod);
            definer.Create();

            var builder = new LocalMethodInjectionBuilder(definer);
            builder.Construct();
        }
    }
}
