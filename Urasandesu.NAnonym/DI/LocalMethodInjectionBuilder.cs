using System.Linq;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjectionBuilder : MethodInjectionBuilder
    {
        public LocalMethodInjectionBuilder(MethodInjectionDefiner parentDefiner)
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
            });
        }
    }
}
