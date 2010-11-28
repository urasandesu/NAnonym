using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Urasandesu.NAnonym.ILTools;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodWeaveDefiner : WeaveDefiner
    {
        public new MethodWeaver Parent { get { return (MethodWeaver)base.Parent; } }
        public WeaveMethodInfo WeaveMethod { get; private set; }
        public Type[] ParameterTypes { get; private set; }

        public MethodWeaveDefiner(MethodWeaver parent, WeaveMethodInfo injectionMethod)
            : base(parent)
        {
            WeaveMethod = injectionMethod;
            ParameterTypes = WeaveMethod.Destination.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
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
