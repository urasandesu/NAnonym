using System.Linq;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionBuilder : ConstructorInjectionBuilder
    {
        public new GlobalConstructorInjectionDefiner Definer { get { return (GlobalConstructorInjectionDefiner)base.Definer; } }
        public GlobalConstructorInjectionBuilder(GlobalConstructorInjectionDefiner definer)
            : base(definer)
        {
        }

        public override void Construct()
        {
            foreach (var constructorDef in Definer.Injection.DeclaringTypeDef.Methods.Where(methodDef => methodDef.Name == ".ctor"))
            {
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
