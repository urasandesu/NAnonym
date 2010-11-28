using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    abstract class LocalMethodInjectionDefiner : MethodInjectionDefiner
    {
        public LocalMethodInjectionDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(injectionMethod.Destination);
        }

        public override void Create()
        {
            cachedMethod = Parent.ConstructorInjection.DeclaringTypeGenerator.AddField(
                LocalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(), InjectionMethod.DelegateType, FieldAttributes.Private);

            cachedSetting = Parent.ConstructorInjection.FieldsForDeclaringType.ContainsKey(InjectionMethod.Destination.DeclaringType) ?
                                            Parent.ConstructorInjection.FieldsForDeclaringType[InjectionMethod.Destination.DeclaringType] :
                                            default(IFieldGenerator);

            methodInterface = GetMethodInterface();

            int parameterPosition = 1;
            var methodParameters = new List<IParameterGenerator>();
            foreach (var parameterName in InjectionMethod.Source.ParameterNames())
            {
                methodParameters.Add(MethodInterface.AddParameter(parameterPosition++, ParameterAttributes.In, parameterName));
            }
            this.methodParameters = new ReadOnlyCollection<IParameterGenerator>(methodParameters);
        }

        readonly FieldInfo anonymousStaticMethodCache;
        public override FieldInfo AnonymousStaticMethodCache
        {
            get { return anonymousStaticMethodCache; }
        }

        IFieldGenerator cachedMethod;
        public override IFieldGenerator CachedMethod
        {
            get { return cachedMethod; }
        }

        IFieldGenerator cachedSetting;
        public override IFieldGenerator CachedSetting
        {
            get { return cachedSetting; }
        }

        IMethodBaseGenerator methodInterface;
        public override IMethodBaseGenerator MethodInterface
        {
            get { return methodInterface; }
        }

        ReadOnlyCollection<IParameterGenerator> methodParameters;
        public override ReadOnlyCollection<IParameterGenerator> MethodParameters
        {
            get { return methodParameters; }
        }
    }
}
