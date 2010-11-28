using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorInjection : Injection
    {
        public ITypeGenerator DeclaringTypeGenerator { get; private set; }
        public Dictionary<Type, IFieldGenerator> FieldsForDeclaringType { get; private set; }
        public HashSet<InjectionFieldInfo> FieldSet { get; private set; }

        public ConstructorInjection(
            ITypeGenerator declaringTypeGenerator,
            HashSet<InjectionFieldInfo> fieldSet)
        {
            DeclaringTypeGenerator = declaringTypeGenerator;
            FieldsForDeclaringType = new Dictionary<Type, IFieldGenerator>();
            FieldSet = fieldSet;
        }

        public Type DeclaringType { get { return DeclaringTypeGenerator.Source; } }

        public string GetFieldNameForDeclaringType(Type declaringType)
        {
            return FieldsForDeclaringType[declaringType].Name;
        }

        public override void Apply()
        {
            if (0 < FieldSet.Count)
            {
                var definer = GetConstructorDefiner(this);
                definer.Create();

                var builder = GetConstructorBuilder(definer);
                builder.Construct();
            }
        }

        protected abstract ConstructorInjectionDefiner GetConstructorDefiner(ConstructorInjection parent);
        protected abstract ConstructorInjectionBuilder GetConstructorBuilder(ConstructorInjectionDefiner parentDefiner);
    }
}
