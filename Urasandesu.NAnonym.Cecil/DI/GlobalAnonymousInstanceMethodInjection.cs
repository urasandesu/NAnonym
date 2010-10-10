using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    abstract class GlobalAnonymousInstanceMethodInjection : GlobalMethodInjection
    {
        public new static GlobalAnonymousInstanceMethodInjection CreateInstance<TBase>(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
        {
            switch (targetMethodInfo.Mode)
            {
                case SetupMode.Override:
                    throw new NotImplementedException();
                case SetupMode.Implement:
                    throw new NotImplementedException();
                case SetupMode.Replace:
                    return new GlobalReplaceAnonymousInstanceMethodInjection<TBase>(tbaseTypeDef, targetMethodInfo);
                case SetupMode.Before:
                    throw new NotImplementedException();
                case SetupMode.After:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException();
            }
        }

        public GlobalAnonymousInstanceMethodInjection(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
            : base(tbaseTypeDef, targetMethodInfo)
        {
        }

        public override abstract void Apply(FieldDefinition cachedSettingDef, FieldDefinition cachedMethodDef);
    }
}
