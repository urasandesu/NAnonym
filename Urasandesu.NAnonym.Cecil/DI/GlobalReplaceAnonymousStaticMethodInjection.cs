﻿using System;
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

            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(targetMethodInfo.NewMethod);
            newMethod.Body.InitLocals = true;
            newMethod.ExpressBody(
            gen =>
            {
                gen.Eval(_ => _.If(_.Ldfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType)) == null));
                var dynamicMethod = default(DynamicMethod);
                var returnType = targetMethodInfo.OldMethod.ReturnType;
                var parameterTypes = targetMethodInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod("dynamicMethod", _.Expand(returnType), _.Expand(parameterTypes), true)));

                var ctor3 = default(ConstructorInfo);
                var method4 = default(MethodInfo);
                gen.Eval(_ => _.Addloc(ctor3, _.Expand(targetMethodInfo.DelegateType).GetConstructor(
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null,
                                                    new Type[] 
                                                                    { 
                                                                        typeof(Object), 
                                                                        typeof(IntPtr) 
                                                                    }, null)));
                gen.Eval(_ => _.Addloc(method4, _.Expand(targetMethodInfo.DelegateType).GetMethod(
                                                    "Invoke",
                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                    null, _.Expand(parameterTypes), null)));

                var cacheField = default(FieldInfo);
                gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                    _.Expand(tmpCacheField.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));

                var targetMethod = default(MethodInfo);
                gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetMethodInfo.NewMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                    _.Expand(targetMethodInfo.NewMethod.Name),
                                                    BindingFlags.NonPublic | BindingFlags.Static)));


                var il = default(ILGenerator);
                gen.Eval(_ => _.Addloc(il, dynamicMethod.GetILGenerator()));
                var label27 = default(Label);
                gen.Eval(_ => _.Addloc(label27, il.DefineLabel()));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldsfld, cacheField));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Brtrue_S, label27));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldnull));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldftn, targetMethod));
                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, ctor3));
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
                gen.Eval(_ => _.Stfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType), _.Extract(targetMethodInfo.DelegateType), dynamicMethod.CreateDelegate(_.Expand(targetMethodInfo.DelegateType))));
                gen.Eval(_ => _.EndIf());
                var invoke = targetMethodInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                gen.Eval(_ => _.Return(_.Invoke(_.Ldfld(_.Extract(cachedMethodDef.Name, targetMethodInfo.DelegateType)), _.Extract(invoke), _.Ldarg(_.Extract<object[]>(targetMethodInfo.OldMethod.ParameterNames())))));
            });
        }
    }
}