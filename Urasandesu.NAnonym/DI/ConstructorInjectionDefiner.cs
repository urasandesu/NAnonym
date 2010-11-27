using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorInjectionDefiner : InjectionDefiner
    {
        public new ConstructorInjection Parent { get { return (ConstructorInjection)base.Parent; } }
        public Dictionary<Type, bool> InitializedDeclaringTypeConstructor { get; private set; }
        public virtual IFieldGenerator CachedConstructor { get { throw new NotImplementedException(); } }
        public virtual IConstructorGenerator NewConstructor { get { throw new NotImplementedException(); } }

        public ConstructorInjectionDefiner(ConstructorInjection parent)
            : base(parent)
        {
            InitializedDeclaringTypeConstructor = new Dictionary<Type, bool>();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
