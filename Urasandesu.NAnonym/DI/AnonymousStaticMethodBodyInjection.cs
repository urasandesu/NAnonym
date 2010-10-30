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
    class AnonymousStaticMethodBodyInjection : DependencyMethodBodyInjection
    {
        protected readonly FieldInfo tmpCacheField;
        public AnonymousStaticMethodBodyInjection(TargetMethodInfo targetMethodInfo, string cachedMethodName, FieldInfo tmpCacheField)
            : base(targetMethodInfo, cachedMethodName)
        {
            this.tmpCacheField = tmpCacheField;
        }

        public override void Apply(ExpressiveMethodBodyGenerator gen)
        {
            gen.Eval(_ => _.If(_.Ld(_.X(cachedMethodName)) == null));
            {
                var dynamicMethod = default(DynamicMethod);
                gen.Eval(_ => _.St(dynamicMethod).As(new DynamicMethod("dynamicMethod", _.X(returnType), _.X(parameterTypes), true)));

                var invokeForLocal = default(ConstructorInfo);
                var method4 = default(MethodInfo);
                gen.Eval(_ => _.St(invokeForLocal).As(_.X(targetMethodInfo.DelegateType).GetConstructor(
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null,
                                                    new Type[] 
                                                    { 
                                                        typeof(Object), 
                                                        typeof(IntPtr) 
                                                    }, null)));
                gen.Eval(_ => _.St(method4).As(_.X(targetMethodInfo.DelegateType).GetMethod(
                                                    "Invoke",
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null, _.X(parameterTypes), null)));

                var cacheField = default(FieldInfo);
                gen.Eval(_ => _.St(cacheField).As(Type.GetType(_.X(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                    _.X(tmpCacheField.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));

                var targetMethod = default(MethodInfo);
                gen.Eval(_ => _.St(targetMethod).As(Type.GetType(_.X(targetMethodInfo.NewMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                    _.X(targetMethodInfo.NewMethod.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));

                // MEMO: LocalClass の場合、null 側の分岐に入ることは無いが、共通化のためにこの部分をそのまま使えるのであれば使う。
                var il = default(ILGenerator);
                gen.Eval(_ => _.St(il).As(dynamicMethod.GetILGenerator()));
                var label27 = default(Label);
                gen.Eval(_ => _.St(label27).As(il.DefineLabel()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldsfld, cacheField));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Brtrue_S, label27));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldnull));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldftn, targetMethod));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, invokeForLocal));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Stsfld, cacheField));
                gen.Eval(_ => il.MarkLabel(label27));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldsfld, cacheField));
                for (int parametersIndex = 0; parametersIndex < parameterTypes.Length; parametersIndex++)
                {
                    switch (parametersIndex)
                    {
                        case 0:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                            break;
                        case 1:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_1));
                            break;
                        case 2:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_2));
                            break;
                        case 3:
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_3));
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, method4));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                gen.Eval(_ => _.St(_.X(cachedMethodName)).As(dynamicMethod.CreateDelegate(_.X(targetMethodInfo.DelegateType))));
            }
            gen.Eval(_ => _.EndIf());
            var invokeForInvoke = targetMethodInfo.DelegateType.GetMethod(
                                                "Invoke",
                                                BindingFlags.Public | BindingFlags.Instance,
                                                null,
                                                parameterTypes,
                                                null);
            gen.Eval(_ => _.Return(_.Invoke(_.Ld(_.X(cachedMethodName)), _.X(invokeForInvoke), _.Ld(_.X(targetMethodInfo.OldMethod.ParameterNames())))));
        }
    }
}
