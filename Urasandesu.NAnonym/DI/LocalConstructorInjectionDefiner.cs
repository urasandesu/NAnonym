using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new LocalConstructorInjection Parent { get { return (LocalConstructorInjection)base.Parent; } }
        public FieldBuilder CachedConstructor { get; private set; }
        public ConstructorBuilder LocalClassConstructorBuilder { get; private set; }

        public LocalConstructorInjectionDefiner(LocalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            CachedConstructor = Parent.DeclaringTypeBuilder.DefineField(
                LocalClass.CacheFieldPrefix + "Constructor", typeof(Action), FieldAttributes.Private | FieldAttributes.Static);

            int fieldForDeclaringTypeIndex = 0;
            foreach (var injectionField in Parent.FieldSet)
            {
                var field = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(field.DeclaringType))
                {
                    var fieldForDeclaringType = Parent.DeclaringTypeBuilder.DefineField(
                            LocalClass.CacheFieldPrefix + "FieldForDeclaringType" + fieldForDeclaringTypeIndex++, field.DeclaringType, FieldAttributes.Private);
                    Parent.FieldsForDeclaringType.Add(field.DeclaringType, fieldForDeclaringType);
                    InitializedDeclaringTypeConstructor.Add(field.DeclaringType, false);
                }
            }

            LocalClassConstructorBuilder = Parent.DeclaringTypeBuilder.DefineConstructor(
                                                    MethodAttributes.Public |
                                                    MethodAttributes.HideBySig |
                                                    MethodAttributes.SpecialName |
                                                    MethodAttributes.RTSpecialName,
                                                    CallingConventions.Standard,
                                                    new Type[] { });
        }

        public override string CachedConstructorName
        {
            get { return CachedConstructor.Name; }
        }
    }
}
