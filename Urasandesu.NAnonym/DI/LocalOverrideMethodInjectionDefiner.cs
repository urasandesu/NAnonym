using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalOverrideMethodInjectionDefiner : LocalMethodInjectionDefiner
    {
        public LocalOverrideMethodInjectionDefiner(LocalMethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override MethodBuilder GetMethodInterface()
        {
            const MethodAttributes @override = MethodAttributes.Public |
                                               MethodAttributes.HideBySig |
                                               MethodAttributes.Virtual;
            var source = InjectionMethod.Source;
            var name = source.Name;
            var returnType = source.ReturnType;
            var parameterTypes = source.ParameterTypes();
            baseMethod = new SRMethodDeclarationImpl(source);
            return Parent.ConstructorInjection.DeclaringTypeBuilder.DefineMethod(
                name, @override, CallingConventions.HasThis, returnType, parameterTypes);
        }

        IMethodDeclaration baseMethod;
        public override IMethodDeclaration BaseMethod
        {
            get { return baseMethod; }
        }
    }
}
