using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalImplementMethodInjectionDefiner : LocalMethodInjectionDefiner
    {
        public LocalImplementMethodInjectionDefiner(TargetMethodInfo targetMethodInfo)
            : base(targetMethodInfo)
        {
        }

        public const MethodAttributes Implement = MethodAttributes.Public |
                                                  MethodAttributes.HideBySig |
                                                  MethodAttributes.NewSlot |
                                                  MethodAttributes.Virtual |
                                                  MethodAttributes.Final;

        public override MethodBuilder DefineMethod(TypeBuilder localClassTypeBuilder)
        {
            var oldMethod = targetMethodInfo.OldMethod;
            var name = oldMethod.Name;
            var returnType = oldMethod.ReturnType;
            var parameterTypes = oldMethod.ParameterTypes();
            return localClassTypeBuilder.DefineMethod(name, Implement, CallingConventions.HasThis, returnType, parameterTypes);
        }
    }
}
