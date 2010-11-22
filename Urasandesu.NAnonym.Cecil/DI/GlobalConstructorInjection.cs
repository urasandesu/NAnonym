using System;
using System.Collections.Generic;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.DI;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjection : ConstructorInjection
    {
        public TypeDefinition DeclaringTypeDef { get; private set; }
        public Dictionary<Type, FieldDefinition> FieldsForDeclaringType { get; private set; }

        readonly Type declaringType;

        public GlobalConstructorInjection(
            TypeDefinition declaringTypeDef,
            HashSet<InjectionFieldInfo> fieldSet)
            : base(fieldSet)
        {
            DeclaringTypeDef = declaringTypeDef;
            FieldsForDeclaringType = new Dictionary<Type,FieldDefinition>();
            declaringType = DeclaringTypeDef.ToType();
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
