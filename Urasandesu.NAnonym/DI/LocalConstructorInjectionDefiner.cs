using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class LocalConstructorInjectionDefiner : ConstructorInjectionDefiner
    {
        public new LocalConstructorInjection Parent { get { return (LocalConstructorInjection)base.Parent; } }
        public FieldBuilder CachedConstructBuilder { get; private set; }
        public ConstructorBuilder LocalClassConstructorBuilder { get; private set; }

        public LocalConstructorInjectionDefiner(LocalConstructorInjection parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            // ↓ここの処理は Injection.TargetFieldInfoSet がある場合だけで良い。
            CachedConstructBuilder = Parent.DeclaringTypeBuilder.DefineField(
                LocalClass.CacheFieldPrefix + "Construct", typeof(Action), FieldAttributes.Private | FieldAttributes.Static);

            int targetFieldDeclaringTypeIndex = 0;
            foreach (var targetFieldInfo in Parent.FieldSet)
            {
                var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(targetField.DeclaringType))
                {
                    var cachedTargetFieldDeclaringTypeBuilder = Parent.DeclaringTypeBuilder.DefineField(
                            LocalClass.CacheFieldPrefix + "TargetFieldDeclaringType" + targetFieldDeclaringTypeIndex++, targetField.DeclaringType, FieldAttributes.Private);
                    Parent.FieldsForDeclaringType.Add(targetField.DeclaringType, cachedTargetFieldDeclaringTypeBuilder);
                    InitializedDeclaringTypeConstructor.Add(targetField.DeclaringType, false);
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

        public override string CachedConstructName
        {
            get { return CachedConstructBuilder.Name; }
        }
    }
}
