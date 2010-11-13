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
        public FieldBuilder CachedMethodField { get; private set; }
        public FieldBuilder CachedSettingField { get; private set; }
        public MethodBuilder MethodInterface { get; private set; }
        public ReadOnlyCollection<ParameterBuilder> MethodParameters { get; private set; }

        protected LocalMethodInjectionDefiner(LocalMethodInjection parent, TargetMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(injectionMethod.NewMethod);
        }

        public static LocalMethodInjectionDefiner GetInstance(LocalMethodInjection parent, TargetMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
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
            CachedMethodField = Parent.ConstructorInjection.DeclaringTypeBuilder.DefineField(
                LocalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodFieldSequence(), InjectionMethod.DelegateType, FieldAttributes.Private);

            CachedSettingField = Parent.ConstructorInjection.FieldsForDeclaringType.ContainsKey(InjectionMethod.NewMethod.DeclaringType) ?
                                            Parent.ConstructorInjection.FieldsForDeclaringType[InjectionMethod.NewMethod.DeclaringType] :
                                            default(FieldBuilder);

            MethodInterface = GetMethodInterface();

            int parameterPosition = 1;
            var methodParameters = new List<ParameterBuilder>();
            foreach (var parameterName in InjectionMethod.OldMethod.ParameterNames())
            {
                methodParameters.Add(MethodInterface.DefineParameter(parameterPosition++, ParameterAttributes.In, parameterName));
            }
            MethodParameters = new ReadOnlyCollection<ParameterBuilder>(methodParameters);
        }

        protected virtual MethodBuilder GetMethodInterface()
        {
            throw new NotImplementedException();
        }

        public override string CachedMethodFieldName
        {
            get { return CachedMethodField.Name; }
        }

        public override string CachedSettingFieldName
        {
            get { return CachedSettingField.Name; }
        }

        public override Type OwnerType
        {
            get { return Parent.ConstructorInjection.DeclaringType; }
        }

        readonly FieldInfo anonymousStaticMethodCacheField;
        public override FieldInfo AnonymousStaticMethodCacheField
        {
            get { return anonymousStaticMethodCacheField; }
        }
    }
}
