using System;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    abstract class GlobalMethodInjectionDefiner
    {
        protected readonly TargetMethodInfo targetMethodInfo;
        public GlobalMethodInjectionDefiner(TargetMethodInfo targetMethodInfo)
        {
            this.targetMethodInfo = targetMethodInfo;
        }

        public static GlobalMethodInjectionDefiner Create(TargetMethodInfo targetMethodInfo)
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
                return new GlobalReplaceMethodInjectionDefiner(targetMethodInfo);
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

        public abstract MethodDefinition DefineMethod(TypeDefinition tbaseTypeDef);
    }
}
