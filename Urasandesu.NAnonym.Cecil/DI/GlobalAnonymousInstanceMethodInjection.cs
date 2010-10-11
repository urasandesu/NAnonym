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
            if (targetMethodInfo.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.Implement)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.Replace)
            {
                return new GlobalReplaceAnonymousInstanceMethodInjection<TBase>(tbaseTypeDef, targetMethodInfo);
            }
            else if (targetMethodInfo.Mode == SetupModes.Before)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.After)
            {
                throw new NotImplementedException();
            }
            else
            {
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
