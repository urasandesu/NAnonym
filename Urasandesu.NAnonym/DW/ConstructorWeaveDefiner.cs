using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorWeaveDefiner : WeaveDefiner
    {
        public new ConstructorWeaver Parent { get { return (ConstructorWeaver)base.Parent; } }
        public Dictionary<Type, bool> InitializedDeclaringTypeConstructor { get; private set; }
        public abstract IFieldGenerator CachedConstructor { get; }
        public abstract IConstructorGenerator NewConstructor { get; }

        public ConstructorWeaveDefiner(ConstructorWeaver parent)
            : base(parent)
        {
            InitializedDeclaringTypeConstructor = new Dictionary<Type, bool>();
        }

        public override abstract void Create();
    }
}
