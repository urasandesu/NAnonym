using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.DI
{
    class LocalMethodInjectionDefiner : MethodInjectionDefiner
    {
        public new LocalMethodInjection Parent { get { return (LocalMethodInjection)base.Parent; } }
        public IFieldGenerator CachedMethod { get; private set; }
        public IFieldGenerator CachedSetting { get; private set; }
        public IMethodBaseGenerator MethodInterface { get; private set; }
        public ReadOnlyCollection<IParameterGenerator> MethodParameters { get; private set; }

        protected LocalMethodInjectionDefiner(LocalMethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(injectionMethod.Destination);
        }

        public static LocalMethodInjectionDefiner GetInstance(LocalMethodInjection parent, InjectionMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                return new LocalOverrideMethodInjectionDefiner(parent, injectionMethod);
            }
            else if (injectionMethod.Mode == SetupModes.Implement)
            {
                return new LocalImplementMethodInjectionDefiner(parent, injectionMethod);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void Create()
        {
            CachedMethod = Parent.ConstructorInjection.DeclaringTypeGenerator.AddField(
                LocalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(), InjectionMethod.DelegateType, FieldAttributes.Private);

            CachedSetting = Parent.ConstructorInjection.FieldsForDeclaringType.ContainsKey(InjectionMethod.Destination.DeclaringType) ?
                                            Parent.ConstructorInjection.FieldsForDeclaringType[InjectionMethod.Destination.DeclaringType] :
                                            default(IFieldGenerator);

            MethodInterface = GetMethodInterface();

            int parameterPosition = 1;
            var methodParameters = new List<IParameterGenerator>();
            foreach (var parameterName in InjectionMethod.Source.ParameterNames())
            {
                methodParameters.Add(MethodInterface.AddParameter(parameterPosition++, ParameterAttributes.In, parameterName));
            }
            MethodParameters = new ReadOnlyCollection<IParameterGenerator>(methodParameters);
        }

        protected virtual IMethodGenerator GetMethodInterface()
        {
            throw new NotImplementedException();
        }

        public override string CachedMethodName
        {
            get { return CachedMethod.Name; }
        }

        public override string CachedSettingName
        {
            get { return CachedSetting.Name; }
        }

        public override Type OwnerType
        {
            get { return Parent.ConstructorInjection.DeclaringType; }
        }

        readonly FieldInfo anonymousStaticMethodCache;
        public override FieldInfo AnonymousStaticMethodCache
        {
            get { return anonymousStaticMethodCache; }
        }
    }
}
