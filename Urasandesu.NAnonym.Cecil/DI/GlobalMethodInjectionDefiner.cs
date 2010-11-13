using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodInjectionDefiner : MethodInjectionDefiner
    {
        public new GlobalMethodInjection Parent { get { return (GlobalMethodInjection)base.Parent; } }
        public FieldDefinition CachedMethodField { get; private set; }
        public FieldDefinition CachedSettingField { get; private set; }
        public MethodDefinition MethodInterface { get; private set; }

        public GlobalMethodInjectionDefiner(GlobalMethodInjection parent, TargetMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(injectionMethod.NewMethod);
        }

        public static GlobalMethodInjectionDefiner GetInstance(GlobalMethodInjection parent, TargetMethodInfo injectionMethod)
        {
            if (injectionMethod.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.Implement)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.Replace)
            {
                return new GlobalReplaceMethodInjectionDefiner(parent, injectionMethod);
            }
            else if (injectionMethod.Mode == SetupModes.Before)
            {
                throw new NotImplementedException();
            }
            else if (injectionMethod.Mode == SetupModes.After)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void Create()
        {
            CachedMethodField = new FieldDefinition(
                                        GlobalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodFieldSequence(),
                                        MC::FieldAttributes.Private,
                                        Parent.ConstructorInjection.DeclaringTypeDef.Module.Import(InjectionMethod.DelegateType));
            Parent.ConstructorInjection.DeclaringTypeDef.Fields.Add(CachedMethodField);

            CachedSettingField = Parent.ConstructorInjection.DeclaringTypeDef.Fields.FirstOrDefault(
                field => field.FieldType.Resolve().GetFullName() == InjectionMethod.NewMethod.DeclaringType.FullName);

            MethodInterface = GetMethodInterface();
        }

        protected virtual MethodDefinition GetMethodInterface()
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
