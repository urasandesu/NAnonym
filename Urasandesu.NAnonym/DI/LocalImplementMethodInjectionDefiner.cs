using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalImplementMethodInjectionDefiner : LocalMethodInjectionDefiner
    {
        public LocalImplementMethodInjectionDefiner(LocalMethodInjection parent, InjectionMethodInfo injectionMethod)
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
            var source = InjectionMethod.Source;
            var name = source.Name;
            var returnType = source.ReturnType;
            var parameterTypes = source.ParameterTypes();
            return Parent.ConstructorInjection.DeclaringTypeBuilder.DefineMethod(
                name, implement, CallingConventions.HasThis, returnType, parameterTypes);
        }
    }
}
