using System;
using System.Collections.Generic;
using Urasandesu.NAnonym.DI;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjection : ConstructorInjection
    {
        public UNI::ITypeGenerator DeclaringTypeGenerator { get; private set; }
        public Dictionary<Type, UNI::IFieldGenerator> FieldsForDeclaringType { get; private set; }

        readonly Type declaringType;

        public GlobalConstructorInjection(
            UNI::ITypeGenerator declaringTypeGen,
            HashSet<InjectionFieldInfo> fieldSet)
            : base(fieldSet)
        {
            DeclaringTypeGenerator = declaringTypeGen;
            FieldsForDeclaringType = new Dictionary<Type, UNI::IFieldGenerator>();
            declaringType = DeclaringTypeGenerator.Source;
        }

        protected override void ApplyContent()
        {
            var definer = new GlobalConstructorInjectionDefiner(this);
            definer.Create();

            var builder = new GlobalConstructorInjectionBuilder(definer);
            builder.Construct();
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override string GetFieldNameForDeclaringType(Type declaringType)
        {
            return FieldsForDeclaringType[declaringType].Name;
        }
    }
}
