using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new LocalConstructorInjection Parent { get { return (LocalConstructorInjection)base.Parent; } }

        public LocalConstructorInjectionDefiner(LocalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            cachedConstructor = Parent.DeclaringTypeGenerator.AddField(
                LocalClass.CacheFieldPrefix + "Constructor", typeof(Action), FieldAttributes.Private | FieldAttributes.Static);

            int fieldForDeclaringTypeIndex = 0;
            foreach (var injectionField in Parent.FieldSet)
            {
                var field = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(field.DeclaringType))
                {
                    var fieldForDeclaringType = Parent.DeclaringTypeGenerator.AddField(
                            LocalClass.CacheFieldPrefix + "FieldForDeclaringType" + fieldForDeclaringTypeIndex++, field.DeclaringType, FieldAttributes.Private);
                    Parent.FieldsForDeclaringType.Add(field.DeclaringType, fieldForDeclaringType);
                    InitializedDeclaringTypeConstructor.Add(field.DeclaringType, false);
                }
            }


            newConstructor = Parent.DeclaringTypeGenerator.AddConstructor(
                                                    MethodAttributes.Public |
                                                    MethodAttributes.HideBySig |
                                                    MethodAttributes.SpecialName |
                                                    MethodAttributes.RTSpecialName,
                                                    CallingConventions.Standard,
                                                    new Type[] { });
        }

        IFieldGenerator cachedConstructor;
        public override IFieldGenerator CachedConstructor
        {
            get { return cachedConstructor; }
        }

        IConstructorGenerator newConstructor;
        public override IConstructorGenerator NewConstructor
        {
            get { return newConstructor; }
        }
    }
}
