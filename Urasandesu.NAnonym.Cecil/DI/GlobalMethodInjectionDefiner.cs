using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodInjectionDefiner : MethodInjectionDefiner
    {
        public new GlobalMethodInjection Parent { get { return (GlobalMethodInjection)base.Parent; } }
        public FieldDefinition CachedMethod { get; private set; }
        public FieldDefinition CachedSetting { get; private set; }
        public MethodDefinition MethodInterface { get; private set; }

        public GlobalMethodInjectionDefiner(GlobalMethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
            anonymousStaticMethodCache = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(injectionMethod.Destination);
        }

        public static GlobalMethodInjectionDefiner GetInstance(GlobalMethodInjection parent, InjectionMethodInfo injectionMethod)
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
            CachedMethod = new FieldDefinition(
                                        GlobalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(),
                                        MC::FieldAttributes.Private,
                                        Parent.ConstructorInjection.DeclaringTypeDef.Module.Import(InjectionMethod.DelegateType));
            Parent.ConstructorInjection.DeclaringTypeDef.Fields.Add(CachedMethod);

            CachedSetting = Parent.ConstructorInjection.DeclaringTypeDef.Fields.FirstOrDefault(
                fieldDef => fieldDef.FieldType.Resolve().GetFullName() == InjectionMethod.Destination.DeclaringType.FullName);

            MethodInterface = GetMethodInterface();
        }

        protected virtual MethodDefinition GetMethodInterface()
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
