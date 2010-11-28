using System;
using Urasandesu.NAnonym.DI;
using SR = System.Reflection;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new GlobalConstructorInjection Parent { get { return (GlobalConstructorInjection)base.Parent; } }

        public GlobalConstructorInjectionDefiner(GlobalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            cachedConstructor = Parent.DeclaringTypeGenerator.AddField(
                    GlobalClass.CacheFieldPrefix + "Constructor", typeof(Action), SR::FieldAttributes.Private | SR::FieldAttributes.Static);

            int fieldForDeclaringTypeIndex = 0;
            foreach (var injectionField in Parent.FieldSet)
            {
                var field = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(field.DeclaringType))
                {
                    var fieldForDeclaringType = Parent.DeclaringTypeGenerator.AddField(
                            GlobalClass.CacheFieldPrefix + "FieldForDeclaringType" + fieldForDeclaringTypeIndex++,
                            field.DeclaringType, SR::FieldAttributes.Private);

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
    }
}
