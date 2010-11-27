using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjection : ConstructorInjection
    {
        public ITypeGenerator DeclaringTypeGenerator { get; private set; }
        public Dictionary<Type, IFieldGenerator> FieldsForDeclaringType { get; private set; }

        public LocalConstructorInjection(
            ITypeGenerator declaringTypeGenerator,
            HashSet<InjectionFieldInfo> fieldSet)
            : base(fieldSet)
        {
            DeclaringTypeGenerator = declaringTypeGenerator;
            FieldsForDeclaringType = new Dictionary<Type, IFieldGenerator>();
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
            get { return (Type)DeclaringTypeGenerator.Source; }
        }

        public override string GetFieldNameForDeclaringType(Type declaringType)
        {
            return FieldsForDeclaringType[declaringType].Name;
        }
    }
}
