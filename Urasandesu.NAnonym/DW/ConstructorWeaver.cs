using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorWeaver : Weaver
    {
        public ITypeGenerator DeclaringTypeGenerator { get; private set; }
        public Dictionary<Type, IFieldGenerator> FieldsForDeclaringType { get; private set; }
        public HashSet<WeaveFieldInfo> FieldSet { get; private set; }

        public ConstructorWeaver(
            ITypeGenerator declaringTypeGenerator,
            HashSet<WeaveFieldInfo> fieldSet)
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

        protected abstract ConstructorWeaveDefiner GetConstructorDefiner(ConstructorWeaver parent);
        protected abstract ConstructorWeaveBuilder GetConstructorBuilder(ConstructorWeaveDefiner parentDefiner);
    }
}
