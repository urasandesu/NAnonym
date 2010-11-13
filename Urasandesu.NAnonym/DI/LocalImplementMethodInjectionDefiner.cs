using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalImplementMethodInjectionDefiner : LocalMethodInjectionDefiner
    {
        public LocalImplementMethodInjectionDefiner(LocalMethodInjection parent, TargetMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override MethodBuilder GetMethodInterface()
        {
            const MethodAttributes implement = MethodAttributes.Public |
                                               MethodAttributes.HideBySig |
                                               MethodAttributes.NewSlot |
                                               MethodAttributes.Virtual |
                                               MethodAttributes.Final;
            var oldMethod = InjectionMethod.OldMethod;
            var name = oldMethod.Name;
            var returnType = oldMethod.ReturnType;
            var parameterTypes = oldMethod.ParameterTypes();
            return Parent.ConstructorInjection.DeclaringTypeBuilder.DefineMethod(
                name, implement, CallingConventions.HasThis, returnType, parameterTypes);
        }
    }
}
