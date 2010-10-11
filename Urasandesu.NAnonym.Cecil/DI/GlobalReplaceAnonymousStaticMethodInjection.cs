using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using SRE = System.Reflection.Emit;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;

namespace Urasandesu.NAnonym.Cecil.DI
{
    class GlobalReplaceAnonymousStaticMethodInjection : GlobalAnonymousStaticMethodInjection
    {
        public GlobalReplaceAnonymousStaticMethodInjection(TypeDefinition tbaseTypeDef, TargetMethodInfo targetMethodInfo)
            : base(tbaseTypeDef, targetMethodInfo)
        {
        }

        public override void Apply(FieldDefinition cachedSettingDef, FieldDefinition cachedMethodDef)
        {
            var oldMethodDef = tbaseTypeDef.Methods.FirstOrDefault(_methodDef => _methodDef.Equivalent(targetMethodInfo.OldMethod));
            string oldMethodName = oldMethodDef.Name;
            oldMethodDef.Name = "__" + oldMethodDef.Name;

            // 元のメソッドと同じメソッドを追加（処理の中身は空にする）
            var newMethod = oldMethodDef.DuplicateWithoutBody();
            newMethod.Name = oldMethodName;
            tbaseTypeDef.Methods.Add(newMethod);

            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(targetMethodInfo.NewMethod);
            newMethod.Body.InitLocals = true;
            newMethod.ExpressBody(
            gen =>
            {
                var returnType = targetMethodInfo.OldMethod.ReturnType;
                var parameterTypes = targetMethodInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                gen.Eval(_ => _.If(_.Ldfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType)) == null));
                {
                    var dynamicMethod = default(DynamicMethod);
                    gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod("dynamicMethod", _.Expand(returnType), _.Expand(parameterTypes), true)));

                    var invokeForLocal = default(MethodInfo);
                    gen.Eval(_ => _.Addloc(invokeForLocal, _.Expand(targetMethodInfo.DelegateType).GetMethod(
                                                        "Invoke",
                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                        null, _.Expand(parameterTypes), null)));

                    var cacheField = default(FieldInfo);
                    gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                        _.Expand(tmpCacheField.Name),
                                                        BindingFlags.NonPublic | BindingFlags.Static)));


                    var il = default(ILGenerator);
                    gen.Eval(_ => _.Addloc(il, dynamicMethod.GetILGenerator()));
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
                    gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, invokeForLocal));
                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                    gen.Eval(_ => _.Stfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType),
                                          _.Extract(targetMethodInfo.DelegateType),
                                          dynamicMethod.CreateDelegate(_.Expand(targetMethodInfo.DelegateType))));
                }
                gen.Eval(_ => _.EndIf());
                var invokeForInvoke = targetMethodInfo.DelegateType.GetMethod(
                                                            "Invoke",
                                                            BindingFlags.Public | BindingFlags.Instance,
                                                            null,
                                                            parameterTypes,
                                                            null);
                gen.Eval(_ => _.Return(_.Invoke(_.Ldfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType)),
                                                _.Extract(invokeForInvoke),
                                                _.Ldarg(_.Extract<object[]>(targetMethodInfo.OldMethod.ParameterNames())))));
            });
        }
    }
}
