using System.Linq;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjectionBuilder : MethodInjectionBuilder
    {
        public new LocalMethodInjectionDefiner ParentDefiner { get { return (LocalMethodInjectionDefiner)base.ParentDefiner; } }
        public LocalMethodInjectionBuilder(LocalMethodInjectionDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.MethodInterface.ExpressBody(
            gen =>
            {
                var bodyInjection = new LocalMethodBodyInjection(gen, this);
                bodyInjection.Apply();
            },
            ParentDefiner.MethodParameters.ToArray(),
            new FieldBuilder[] { ParentDefiner.CachedMethodField });
        }
    }
}
