using System;
using Mono.Cecil;
using Urasandesu.NAnonym.DI;
using MC = Mono.Cecil;
using UNI = Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new GlobalConstructorInjection Parent { get { return (GlobalConstructorInjection)base.Parent; } }
        //// これも一段上に上げられる。
        ////public FieldDefinition CachedConstructor { get; private set; }
        //public UNI::IFieldGenerator CachedConstructor { get; private set; }

        public GlobalConstructorInjectionDefiner(GlobalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            var cachedConstructorDef = new FieldDefinition(
                    GlobalClass.CacheFieldPrefix + "Constructor", MC::FieldAttributes.Private | MC::FieldAttributes.Static, Parent.DeclaringTypeDef.Module.Import(typeof(Action)));
            Parent.DeclaringTypeDef.Fields.Add(cachedConstructorDef);
            cachedConstructor = new MCFieldGeneratorImpl(cachedConstructorDef);

            int fieldForDeclaringTypeIndex = 0;
            foreach (var injectionField in Parent.FieldSet)
            {
                var field = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(field.DeclaringType))
                {
                    var fieldForDeclaringType = new FieldDefinition(
                            GlobalClass.CacheFieldPrefix + "FieldForDeclaringType" + fieldForDeclaringTypeIndex++,
                            MC::FieldAttributes.Private, Parent.DeclaringTypeDef.Module.Import(field.DeclaringType));
                    Parent.DeclaringTypeDef.Fields.Add(fieldForDeclaringType);

                    Parent.FieldsForDeclaringType.Add(field.DeclaringType, fieldForDeclaringType);
                    InitializedDeclaringTypeConstructor.Add(field.DeclaringType, false);
                }
            }
        }

        UNI::IFieldGenerator cachedConstructor;
        public override UNI::IFieldGenerator CachedConstructor
        {
            get { return cachedConstructor; }
        }

        //// これは必要なくなる
        //public override string CachedConstructorName
        //{
        //    get { return CachedConstructor.Name; }
        //}
    }
}
