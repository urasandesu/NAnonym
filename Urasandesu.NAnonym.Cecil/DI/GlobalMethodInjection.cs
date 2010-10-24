using System.Linq;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodInjection : DependencyMethodInjection
    {
        protected readonly TypeDefinition tbaseTypeDef;
        public GlobalMethodInjection(TypeDefinition tbaseTypeDef)
        {
            this.tbaseTypeDef = tbaseTypeDef;
        }

        protected override string UniqueCacheMethodFieldName()
        {
            return GlobalClass.CacheFieldPrefix + "Method" + IncreaseSequence();
        }

        public override void Apply(TargetMethodInfo targetMethodInfo)
        {
            var cachedMethodDef = new FieldDefinition(
                                        UniqueCacheMethodFieldName(),
                                        MC::FieldAttributes.Private,
                                        tbaseTypeDef.Module.Import(targetMethodInfo.DelegateType));
            tbaseTypeDef.Fields.Add(cachedMethodDef);

            var cachedSettingDef = tbaseTypeDef.Fields.FirstOrDefault(
                field => field.FieldType.Resolve().GetFullName() == targetMethodInfo.NewMethod.DeclaringType.FullName);

            var definer = GlobalMethodInjectionDefiner.Create(targetMethodInfo);
            var methodDef = definer.DefineMethod(tbaseTypeDef);

            var injectionBuilder = GlobalMethodInjectionBuilder.Create(targetMethodInfo);
            injectionBuilder.TBaseTypeDef = tbaseTypeDef;
            injectionBuilder.NewMethod = methodDef;
            injectionBuilder.CachedMethodDef = cachedMethodDef;
            injectionBuilder.CachedSettingDef = cachedSettingDef;
            injectionBuilder.Construct();
        }
    }
}
