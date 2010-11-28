using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    class LocalImplementMethodWeaveDefiner : LocalMethodWeaveDefiner
    {
        public LocalImplementMethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent, injectionMethod)
        {
        }

        protected override IMethodGenerator GetMethodInterface()
        //protected override MethodBuilder GetMethodInterface()
        {
            const MethodAttributes implement = MethodAttributes.Public |
                                               MethodAttributes.HideBySig |
                                               MethodAttributes.NewSlot |
                                               MethodAttributes.Virtual |
                                               MethodAttributes.Final;
            var source = WeaveMethod.Source;
            var name = source.Name;
            var returnType = source.ReturnType;
            var parameterTypes = source.ParameterTypes();
            return Parent.ConstructorWeaver.DeclaringTypeGenerator.AddMethod(
                name, implement, CallingConventions.HasThis, returnType, parameterTypes);
            //return Parent.ConstructorWeaver.DeclaringTypeBuilder.DefineMethod(
            //    name, implement, CallingConventions.HasThis, returnType, parameterTypes);
        }

        public override IMethodDeclaration BaseMethod
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
