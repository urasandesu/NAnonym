using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjectionBuilder : ConstructorInjectionBuilder
    {
        public new LocalConstructorInjectionDefiner Definer { get { return (LocalConstructorInjectionDefiner)base.Definer; } }
        public LocalConstructorInjectionBuilder(LocalConstructorInjectionDefiner definer)
            : base(definer)
        {
        }

        public override void Construct()
        {
            Definer.LocalClassConstructorBuilder.ExpressBody(
            gen =>
            {
                var bodyInjection = new LocalConstructorBodyInjection(gen, this);
                bodyInjection.Apply();
                gen.Eval(_ => _.Base());
            },
            new FieldBuilder[] { Definer.CachedConstructBuilder }.Concat(Definer.Injection.FieldsForDeclaringType.Values).ToArray());
        }
    }
}
