using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    abstract class LocalMethodWeaveDefiner : MethodWeaveDefiner
    {
        public LocalMethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(injectionMethod.Destination);
        }

        public override void Create()
        {
            cachedMethod = Parent.ConstructorWeaver.DeclaringTypeGenerator.AddField(
                LocalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(), WeaveMethod.DelegateType, FieldAttributes.Private);

            cachedSetting = Parent.ConstructorWeaver.FieldsForDeclaringType.ContainsKey(WeaveMethod.Destination.DeclaringType) ?
                                            Parent.ConstructorWeaver.FieldsForDeclaringType[WeaveMethod.Destination.DeclaringType] :
                                            default(IFieldGenerator);

            methodInterface = GetMethodInterface();

            int parameterPosition = 1;
            var methodParameters = new List<IParameterGenerator>();
            foreach (var parameterName in WeaveMethod.Source.ParameterNames())
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
