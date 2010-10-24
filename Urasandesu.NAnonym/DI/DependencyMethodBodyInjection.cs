using System;
using System.Linq;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    abstract class DependencyMethodBodyInjection
    {
        protected readonly TargetMethodInfo targetMethodInfo;
        protected readonly string cachedMethodName;
        protected readonly Type returnType;
        protected readonly Type[] parameterTypes;
        public DependencyMethodBodyInjection(TargetMethodInfo targetMethodInfo, string cachedMethodName)
        {
            this.targetMethodInfo = targetMethodInfo;
            this.cachedMethodName = cachedMethodName;
            returnType = targetMethodInfo.OldMethod.ReturnType;
            parameterTypes = targetMethodInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public abstract void Apply(ExpressiveMethodBodyGenerator gen);
    }
}
