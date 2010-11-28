using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionBuilder : ConstructorInjectionBuilder
    {
        public new GlobalConstructorInjectionDefiner ParentDefiner { get { return (GlobalConstructorInjectionDefiner)base.ParentDefiner; } }
        public GlobalConstructorInjectionBuilder(GlobalConstructorInjectionDefiner parentDefiner)
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
