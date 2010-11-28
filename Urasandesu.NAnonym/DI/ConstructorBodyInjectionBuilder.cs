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
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    abstract class ConstructorBodyInjectionBuilder : BodyInjectionBuilder
    {
        public new ConstructorBodyInjectionDefiner ParentBodyDefiner { get { return (ConstructorBodyInjectionDefiner)base.ParentBodyDefiner; } }
        public ConstructorBodyInjectionBuilder(ConstructorBodyInjectionDefiner parentBodyDefiner)
            : base(parentBodyDefiner)
        {
        }

        public override void Construct()
        {
            var bodyInjection = ParentBodyDefiner.ParentBody;
            var gen = bodyInjection.Gen;
            var injectionDefiner = bodyInjection.ParentBuilder.ParentDefiner;
            var injection = injectionDefiner.Parent;

            gen.Eval(_ => _.If(_.Ld(_.X(injectionDefiner.CachedConstructor.Name)) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.St(dynamicMethod).As(new DynamicMethod(
                                                            "dynamicMethod",
                                                            typeof(void),
                                                            new Type[] { _.X(injection.DeclaringType) },
                                                            _.X(injection.DeclaringType),
                                                            true)));
                var il = default(ILGenerator);
                gen.Eval(_ => _.St(il).As(dynamicMethod.GetILGenerator()));
                foreach (var injectionField in injection.FieldSet)
                {
                    var targetField = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                    if (!injectionDefiner.InitializedDeclaringTypeConstructor[targetField.DeclaringType])
                    {
                        injectionDefiner.InitializedDeclaringTypeConstructor[targetField.DeclaringType] = true;

                        var declaringTypeConstructor = default(ConstructorInfo);
                        gen.Eval(_ => _.St(declaringTypeConstructor).As(
                                               _.X(targetField.DeclaringType).GetConstructor(
                                                                    BindingFlags.Public | BindingFlags.Instance,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null)));

                        gen.Eval(_ => _.St(_.X(injection.GetFieldNameForDeclaringType(targetField.DeclaringType))).As(
                                               _.X(injection.DeclaringType).GetField(
                                                                    _.X(injection.GetFieldNameForDeclaringType(targetField.DeclaringType)),
                                                                    BindingFlags.Instance | BindingFlags.NonPublic)));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, declaringTypeConstructor));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, _.Ld<FieldInfo>(_.X(injection.GetFieldNameForDeclaringType(targetField.DeclaringType)))));
                    }

                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, _.Ld<FieldInfo>(_.X(injection.GetFieldNameForDeclaringType(targetField.DeclaringType)))));
                    var actualTargetField = default(FieldInfo);
                    gen.Eval(_ => _.St(actualTargetField).As(
                                           _.X(targetField.DeclaringType).GetField(
                                                                    _.X(targetField.Name),
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));

                    var macro = new ExpressiveMethodBodyGeneratorMacro(gen);
                    macro.EvalEmitDirectives(TypeSavable.GetName(() => il), gen.ToDirectives(injectionField.Initializer));

                    gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, actualTargetField));
                }
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                gen.Eval(_ => _.St(_.X(injectionDefiner.CachedConstructor.Name)).As(dynamicMethod.CreateDelegate(typeof(Action), _.This())));
            }
            gen.Eval(_ => _.EndIf());
            gen.Eval(_ => _.Ld<Action>(_.X(injectionDefiner.CachedConstructor.Name)).Invoke());
        }
    }
}
