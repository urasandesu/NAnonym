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
        public new LocalConstructorInjectionDefiner ParentDefiner { get { return (LocalConstructorInjectionDefiner)base.ParentDefiner; } }
        public LocalConstructorInjectionBuilder(LocalConstructorInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.LocalClassConstructorBuilder.ExpressBody(
            gen =>
            {
                var bodyInjection = new LocalConstructorBodyInjection(gen, this);
                bodyInjection.Apply();
                gen.Eval(_ => _.Base());
            },
            new FieldBuilder[] { ParentDefiner.CachedConstructBuilder }.Concat(ParentDefiner.Parent.FieldsForDeclaringType.Values).ToArray());
        }
    }
}
