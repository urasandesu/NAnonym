using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.DI
{
    class ConstructorInjectionDefiner : InjectionDefiner
    {
        public new ConstructorInjection Injection { get { return (ConstructorInjection)base.Injection; } }
        public Dictionary<Type, bool> InitializedDeclaringTypeConstructor { get; private set; }
        public virtual string CachedConstructName { get { throw new NotImplementedException(); } }

        public ConstructorInjectionDefiner(ConstructorInjection injection)
            : base(injection)
        {
            InitializedDeclaringTypeConstructor = new Dictionary<Type, bool>();
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
