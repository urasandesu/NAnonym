using System;
using Urasandesu.NAnonym.DW;
using SR = System.Reflection;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public GlobalConstructorInjectionDefiner(ConstructorInjection parent)
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

        public override UNI::IConstructorGenerator NewConstructor
        {
            get { throw new NotImplementedException(); }
        }
    }
}
