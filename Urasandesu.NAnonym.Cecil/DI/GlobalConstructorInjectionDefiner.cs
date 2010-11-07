using System;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new GlobalConstructorInjection Injection { get { return (GlobalConstructorInjection)base.Injection; } }
        public FieldDefinition CachedConstructDef { get; private set; }

        public GlobalConstructorInjectionDefiner(GlobalConstructorInjection injection)
            : base(injection)
        {
        }

        public override void Create()
        {
            CachedConstructDef = new FieldDefinition(
                    GlobalClass.CacheFieldPrefix + "Construct", MC::FieldAttributes.Private | MC::FieldAttributes.Static, Injection.DeclaringTypeDef.Module.Import(typeof(Action)));
            Injection.DeclaringTypeDef.Fields.Add(CachedConstructDef);

            int targetFieldDeclaringTypeIndex = 0;
            foreach (var targetFieldInfo in Injection.FieldSet)
            {
                var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                if (!Injection.FieldsForDeclaringType.ContainsKey(targetField.DeclaringType))
                {
                    var fieldForDeclaringType = new FieldDefinition(
                            GlobalClass.CacheFieldPrefix + "FieldForDeclaringType" + targetFieldDeclaringTypeIndex++,
                            MC::FieldAttributes.Private, Injection.DeclaringTypeDef.Module.Import(targetField.DeclaringType));
                    Injection.DeclaringTypeDef.Fields.Add(fieldForDeclaringType);

                    Injection.FieldsForDeclaringType.Add(targetField.DeclaringType, fieldForDeclaringType);
                    InitializedDeclaringTypeConstructor.Add(targetField.DeclaringType, false);
                }
            }
        }

        public override string CachedConstructName
        {
            get { return CachedConstructDef.Name; }
        }
    }
}
