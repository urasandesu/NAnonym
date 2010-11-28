using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    class LocalOverrideMethodWeaveDefiner : LocalMethodWeaveDefiner
    {
        public LocalOverrideMethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override IMethodGenerator GetMethodInterface()
        {
            const MethodAttributes @override = MethodAttributes.Public |
                                               MethodAttributes.HideBySig |
                                               MethodAttributes.Virtual;
            var source = WeaveMethod.Source;
            var name = source.Name;
            var returnType = source.ReturnType;
            var parameterTypes = source.ParameterTypes();
            baseMethod = new SRMethodDeclarationImpl(source);
            return Parent.ConstructorWeaver.DeclaringTypeGenerator.AddMethod(
                name, @override, CallingConventions.HasThis, returnType, parameterTypes);
        }

        IMethodDeclaration baseMethod;
        public override IMethodDeclaration BaseMethod
        {
            get { return baseMethod; }
        }
    }
}
