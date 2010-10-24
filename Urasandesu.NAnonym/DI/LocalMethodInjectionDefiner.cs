using System;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    abstract class LocalMethodInjectionDefiner
    {
        protected readonly TargetMethodInfo targetMethodInfo;
        public LocalMethodInjectionDefiner(TargetMethodInfo targetMethodInfo)
        {
            this.targetMethodInfo = targetMethodInfo;
        }

        public static LocalMethodInjectionDefiner Create(TargetMethodInfo targetMethodInfo)
        {
            if (targetMethodInfo.Mode == SetupModes.Override)
            {
                throw new NotImplementedException();
            }
            else if (targetMethodInfo.Mode == SetupModes.Implement)
            {
                return new LocalImplementMethodInjectionDefiner(targetMethodInfo);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public abstract MethodBuilder DefineMethod(TypeBuilder localClassTypeBuilder);
    }
}
