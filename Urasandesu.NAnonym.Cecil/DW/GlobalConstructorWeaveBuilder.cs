using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorWeaveBuilder : ConstructorWeaveBuilder
    {
        public GlobalConstructorWeaveBuilder(ConstructorWeaveDefiner parentDefiner)
            : base(parentDefiner)
        {
        }

        public override void Construct()
        {
            foreach (var constructorGen in ParentDefiner.Parent.DeclaringTypeGenerator.Constructors)
            {
                var constructorDef = ((MCConstructorGeneratorImpl)constructorGen).MethodDef;
                var firstInstruction = constructorDef.Body.Instructions[0];
                constructorDef.ExpressBodyBefore(
                gen =>
                {
                    var bodyWeaver = new GlobalConstructorBodyWeaver(gen, this);
                    bodyWeaver.Apply();
                },
                firstInstruction);
            }
        }
    }
}
