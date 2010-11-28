using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorInjectionBuilder : ConstructorInjectionBuilder
    {
        public GlobalConstructorInjectionBuilder(ConstructorInjectionDefiner parentDefiner)
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
                    var bodyInjection = new GlobalConstructorBodyInjection(gen, this);
                    bodyInjection.Apply();
                },
                firstInstruction);
            }
        }
    }
}
