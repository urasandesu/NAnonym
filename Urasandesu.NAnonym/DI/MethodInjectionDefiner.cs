using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.DI
{
    abstract class MethodInjectionDefiner : InjectionDefiner
    {
        public new MethodInjection Parent { get { return (MethodInjection)base.Parent; } }
        public InjectionMethodInfo InjectionMethod { get; private set; }
        public Type[] ParameterTypes { get; private set; }

        public MethodInjectionDefiner(MethodInjection parent, InjectionMethodInfo injectionMethod)
            : base(parent)
        {
            InjectionMethod = injectionMethod;
            ParameterTypes = InjectionMethod.Destination.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        }

        public abstract override void Create();

        public abstract IFieldGenerator CachedMethod { get; }
        public abstract IFieldGenerator CachedSetting { get; }
        public abstract IMethodBaseGenerator MethodInterface { get; }
        public abstract ReadOnlyCollection<IParameterGenerator> MethodParameters { get; }
        public abstract FieldInfo AnonymousStaticMethodCache { get; }
        public abstract IMethodDeclaration BaseMethod { get; }
        protected abstract IMethodGenerator GetMethodInterface();
    }
}
