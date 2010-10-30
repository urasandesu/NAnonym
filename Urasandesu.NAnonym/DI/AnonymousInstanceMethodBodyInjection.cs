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
using Urasandesu.NAnonym.ILTools.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    class AnonymousInstanceMethodBodyInjection : DependencyMethodBodyInjection
    {
        protected readonly string cachedSettingName;
        protected readonly Type ownerType;
        public AnonymousInstanceMethodBodyInjection(TargetMethodInfo targetMethodInfo, string cachedMethodName, string cachedSettingName, Type ownerType)
            : base(targetMethodInfo, cachedMethodName)
        {
            this.cachedSettingName = cachedSettingName;
            this.ownerType = ownerType;
        }

        public override void Apply(ExpressiveMethodBodyGenerator gen)
        {
            gen.Eval(_ => _.If(_.Ld(_.X(cachedMethodName)) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.St(dynamicMethod).As(new DynamicMethod(
                                                            "dynamicMethod",
                                                            _.X(returnType),
                                                            new Type[] { _.X(ownerType) }.Concat(_.X(parameterTypes)).ToArray(),
                                                            _.X(ownerType),
                                                            true)));


                var cacheField = default(FieldInfo);
                gen.Eval(_ => _.St(cacheField).As(_.X(ownerType).GetField(
                                                        _.X(cachedSettingName),
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));

                var targetMethod = default(MethodInfo);
                gen.Eval(_ => _.St(targetMethod).As(_.X(targetMethodInfo.NewMethod.DeclaringType).GetMethod(
                                                        _.X(targetMethodInfo.NewMethod.Name),
                                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));


                var il = default(ILGenerator);
                gen.Eval(_ => _.St(il).As(dynamicMethod.GetILGenerator()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, cacheField));
                for (int parametersIndex = 0; parametersIndex < parameterTypes.Length; parametersIndex++)
                {
                    switch (parametersIndex)
                    {
                        case 0:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_1));
                            break;
                        case 1:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_2));
                            break;
                        case 2:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_3));
                            break;
                        case 3:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg, (short)4));
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, targetMethod));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                gen.Eval(_ => _.St(_.X(cachedMethodName)).As(dynamicMethod.CreateDelegate(_.X(targetMethodInfo.DelegateType), _.This())));
            }
            gen.Eval(_ => _.EndIf());
            var invoke = targetMethodInfo.DelegateType.GetMethod(
                                                        "Invoke",
                                                        BindingFlags.Public | BindingFlags.Instance,
                                                        null,
                                                        parameterTypes,
                                                        null);
            gen.Eval(_ => _.Return(_.Invoke(_.Ld(_.X(cachedMethodName)), _.X(invoke), _.Ld(_.X(targetMethodInfo.OldMethod.ParameterNames())))));
        }
    }
}
