using System;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new GlobalConstructorInjection Parent { get { return (GlobalConstructorInjection)base.Parent; } }
        public FieldDefinition CachedConstructDef { get; private set; }

        public GlobalConstructorInjectionDefiner(GlobalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            CachedConstructDef = new FieldDefinition(
                    GlobalClass.CacheFieldPrefix + "Construct", MC::FieldAttributes.Private | MC::FieldAttributes.Static, Parent.DeclaringTypeDef.Module.Import(typeof(Action)));
            Parent.DeclaringTypeDef.Fields.Add(CachedConstructDef);

            int targetFieldDeclaringTypeIndex = 0;
            foreach (var targetFieldInfo in Parent.FieldSet)
            {
                var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(targetField.DeclaringType))
                {
                    var fieldForDeclaringType = new FieldDefinition(
                            GlobalClass.CacheFieldPrefix + "FieldForDeclaringType" + targetFieldDeclaringTypeIndex++,
                            MC::FieldAttributes.Private, Parent.DeclaringTypeDef.Module.Import(targetField.DeclaringType));
                    Parent.DeclaringTypeDef.Fields.Add(fieldForDeclaringType);

                    Parent.FieldsForDeclaringType.Add(targetField.DeclaringType, fieldForDeclaringType);
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
