using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjection : ConstructorInjection
    {
        public TypeBuilder DeclaringTypeBuilder { get; private set; }
        public Dictionary<Type, FieldBuilder> FieldsForDeclaringType { get; private set; }

        public LocalConstructorInjection(
            TypeBuilder declaringTypeBuilder,
            HashSet<TargetFieldInfo> fieldSet,
            Dictionary<Type, FieldBuilder> fieldsForDeclaringType)
            : base(fieldSet)
        {
            DeclaringTypeBuilder = declaringTypeBuilder;
            FieldsForDeclaringType = fieldsForDeclaringType;
        }

        protected override void ApplyContent()
        {
            var definer = new LocalConstructorInjectionDefiner(this);
            definer.Create();

            var builder = new LocalConstructorInjectionBuilder(definer);
            builder.Construct();
        }

        public override Type DeclaringType
        {
            get { return DeclaringTypeBuilder; }
        }

        public override string GetFieldNameForDeclaringType(Type declaringType)
        {
            return FieldsForDeclaringType[declaringType].Name;
        }
    }
}
