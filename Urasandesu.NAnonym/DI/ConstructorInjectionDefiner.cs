using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    abstract class ConstructorInjectionDefiner : InjectionDefiner
    {
        public new ConstructorInjection Parent { get { return (ConstructorInjection)base.Parent; } }
        public Dictionary<Type, bool> InitializedDeclaringTypeConstructor { get; private set; }
        public abstract IFieldGenerator CachedConstructor { get; }
        public abstract IConstructorGenerator NewConstructor { get; }

        public ConstructorInjectionDefiner(ConstructorInjection parent)
            : base(parent)
        {
            InitializedDeclaringTypeConstructor = new Dictionary<Type, bool>();
        }

        public override abstract void Create();
    }
}
