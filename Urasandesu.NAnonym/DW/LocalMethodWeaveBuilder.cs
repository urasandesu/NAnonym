using System.Linq;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DW
{
    class LocalMethodWeaveBuilder : MethodWeaveBuilder
    {
        public LocalMethodWeaveBuilder(MethodWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.MethodInterface.ExpressBody(
            gen =>
            {
                var bodyWeaver = new LocalMethodBodyWeaver(gen, this);
                bodyWeaver.Apply();
            });
        }
    }
}
