using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class MethodInjectionDefiner : InjectionDefiner
    {
        public new MethodInjection Parent { get { return (MethodInjection)base.Parent; } }
        public TargetMethodInfo InjectionMethod { get; private set; }
        public virtual string CachedMethodFieldName { get { throw new NotImplementedException(); } }
        public virtual string CachedSettingFieldName { get { throw new NotImplementedException(); } }
        public virtual Type OwnerType { get { throw new NotImplementedException(); } }
        public virtual FieldInfo AnonymousStaticMethodCacheField { get { throw new NotImplementedException(); } }

        public virtual Type ReturnType { get { return InjectionMethod.OldMethod.ReturnType; } }

        readonly Type[] parameterTypes;
        public virtual Type[] ParameterTypes { get { return parameterTypes; } }

        public MethodInjectionDefiner(MethodInjection parent, TargetMethodInfo injectionMethod)
            : base(parent)
        {
            InjectionMethod = injectionMethod;
            parameterTypes = InjectionMethod.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
