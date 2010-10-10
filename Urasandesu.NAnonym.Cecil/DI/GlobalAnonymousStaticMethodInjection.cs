using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    abstract class GlobalAnonymousStaticMethodInjection : GlobalMethodInjection
    {
        public new static GlobalAnonymousStaticMethodInjection CreateInstance<TBase>(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
        {
            switch (targetMethodInfo.Mode)
            {
                case SetupMode.Override:
                    throw new NotImplementedException();
                case SetupMode.Implement:
                    throw new NotImplementedException();
                case SetupMode.Replace:
                    return new GlobalReplaceAnonymousStaticMethodInjection(tbaseTypeDef, targetMethodInfo);
                case SetupMode.Before:
                    throw new NotImplementedException();
                case SetupMode.After:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException();
            }
        }

        public GlobalAnonymousStaticMethodInjection(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
            : base(tbaseTypeDef, targetMethodInfo)
        {
        }

        public override abstract void Apply(FieldDefinition cachedSettingDef, FieldDefinition cachedMethodDef);
    }
}
