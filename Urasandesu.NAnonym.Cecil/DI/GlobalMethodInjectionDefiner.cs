using System;
using System.Linq;
using System.Reflection;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;
using SR = System.Reflection;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalMethodInjectionDefiner : MethodInjectionDefiner
    {
        public new GlobalMethodInjection Parent { get { return (GlobalMethodInjection)base.Parent; } }
        public UNI::IFieldGenerator CachedMethod { get; private set; }
        public UNI::IFieldGenerator CachedSetting { get; private set; }
        public UNI::IMethodGenerator MethodInterface { get; private set; }

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
            CachedMethod = Parent.ConstructorInjection.DeclaringTypeGenerator.AddField(
                                        GlobalClass.CacheFieldPrefix + "Method" + Parent.IncreaseMethodCacheSequence(),
                                        InjectionMethod.DelegateType, 
                                        SR::FieldAttributes.Private);

            var declaringTypeDef = ((MCTypeGeneratorImpl)Parent.ConstructorInjection.DeclaringTypeGenerator).TypeDef;
            CachedSetting = new MCFieldGeneratorImpl(declaringTypeDef.Fields.FirstOrDefault(
                fieldDef => fieldDef.FieldType.Resolve().GetFullName() == InjectionMethod.Destination.DeclaringType.FullName));

            MethodInterface = GetMethodInterface();
        }

        protected virtual UNI::IMethodGenerator GetMethodInterface()
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
