using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjection : DependencyMethodInjection
    {
        protected readonly TypeBuilder localClassTypeBuilder;
        readonly Dictionary<Type, FieldBuilder> targetFieldDeclaringTypeDictionary;
        public LocalMethodInjection(TypeBuilder localClassTypeBuilder, Dictionary<Type, FieldBuilder> targetFieldDeclaringTypeDictionary)
        {
            this.localClassTypeBuilder = localClassTypeBuilder;
            this.targetFieldDeclaringTypeDictionary = targetFieldDeclaringTypeDictionary;
        }

        protected override string UniqueCacheMethodFieldName()
        {
            return LocalClass.CacheFieldPrefix + "Method" + IncreaseSequence();
        }

        public override void Apply(TargetMethodInfo targetMethodInfo)
        {
            var cachedMethodBuilder = localClassTypeBuilder.DefineField(
                UniqueCacheMethodFieldName(), targetMethodInfo.DelegateType, FieldAttributes.Private);

            var cachedSettingBuilder = targetFieldDeclaringTypeDictionary.ContainsKey(targetMethodInfo.NewMethod.DeclaringType) ?
                                            targetFieldDeclaringTypeDictionary[targetMethodInfo.NewMethod.DeclaringType] :
                                            default(FieldBuilder);

            var definer = LocalMethodInjectionDefiner.Create(targetMethodInfo);
            var methodBuilder = definer.DefineMethod(localClassTypeBuilder);

            int parameterPosition = 1;
            var parameterBuilders = new List<ParameterBuilder>();
            foreach (var parameterName in targetMethodInfo.OldMethod.ParameterNames())
            {
                parameterBuilders.Add(methodBuilder.DefineParameter(parameterPosition++, ParameterAttributes.In, parameterName));
            }

            var injectionBuilder = LocalMethodInjectionBuilder.Create(targetMethodInfo);
            injectionBuilder.LocalClassTypeBuilder = localClassTypeBuilder;
            injectionBuilder.CachedSettingBuilder = cachedSettingBuilder;
            injectionBuilder.MethodBuilder = methodBuilder;
            injectionBuilder.CachedMethodBuilder = cachedMethodBuilder;
            injectionBuilder.ParameterBuilders = parameterBuilders.ToArray();
            injectionBuilder.Construct();
        }
    }
}
