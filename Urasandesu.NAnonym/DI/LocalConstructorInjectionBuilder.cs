using System.Linq;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjectionBuilder : ConstructorInjectionBuilder
    {
        public LocalConstructorInjectionBuilder(ConstructorInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.NewConstructor.ExpressBody(
            gen =>
            {
                var bodyInjection = new LocalConstructorBodyInjection(gen, this);
                bodyInjection.Apply();
                gen.Eval(_ => _.Base());
            });
        }
    }
}
