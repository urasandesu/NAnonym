using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class MethodInjectionDefiner : InjectionDefiner
    {
        public new MethodInjection Parent { get { return (MethodInjection)base.Parent; } }
        public InjectionMethodInfo InjectionMethod { get; private set; }

        public virtual string CachedMethodName { get { throw new NotImplementedException(); } }
        public virtual string CachedSettingName { get { throw new NotImplementedException(); } }
        public virtual Type OwnerType { get { throw new NotImplementedException(); } }
        public virtual FieldInfo AnonymousStaticMethodCache { get { throw new NotImplementedException(); } }
        public virtual Type ReturnType { get { return InjectionMethod.Source.ReturnType; } }
        readonly Type[] parameterTypes;
        public virtual Type[] ParameterTypes { get { return parameterTypes; } }
        public virtual IMethodDeclaration BaseMethod { get { throw new NotImplementedException(); } }

        public MethodInjectionDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent)
        {
            InjectionMethod = injectionMethod;
            parameterTypes = InjectionMethod.Destination.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
