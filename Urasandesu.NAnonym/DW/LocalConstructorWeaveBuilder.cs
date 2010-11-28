using System.Linq;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalConstructorWeaveBuilder : ConstructorWeaveBuilder
    {
        public LocalConstructorWeaveBuilder(ConstructorWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            ParentDefiner.NewConstructor.ExpressBody(
            gen =>
            {
                var bodyWeaver = new LocalConstructorBodyWeaver(gen, this);
                bodyWeaver.Apply();
                gen.Eval(_ => _.Base());
            });
        }
    }
}
