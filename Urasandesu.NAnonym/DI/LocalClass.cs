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

    public abstract class LocalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UND$<>0__Cached";
    }

    public sealed class LocalClass<TBase> : LocalClass
    {
        Type createdType;

        Action<LocalClass<TBase>> setupper;
        public void Setup(Action<LocalClass<TBase>> setupper)
        {
            Required.NotDefault(setupper, () => setupper);
            this.setupper = setupper;
        }

        protected override DependencyClass OnRegister()
        {
            setupper(this);
            return null;
        }

        public LocalFunc<TBase, TResult> Method<TResult>(Expression<FuncReference<TBase, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, TResult>(this, oldMethod);
        }

        public LocalFunc<TBase, T, TResult> Method<T, TResult>(Expression<FuncReference<TBase, T, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T, TResult>(this, oldMethod);
        }

        public LocalFunc<TBase, T1, T2, TResult> Method<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, TResult>(this, oldMethod);
        }

        public LocalFunc<TBase, T1, T2, T3, TResult> Method<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, T3, TResult>(this, oldMethod);
        }

        public LocalFunc<TBase, T1, T2, T3, T4, TResult> Method<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, T3, T4, TResult>(this, oldMethod);
        }

        public LocalAction<TBase> Method(Expression<ActionReference<TBase>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase>(this, oldMethod);
        }

        public LocalAction<TBase, T> Method<T>(Expression<ActionReference<TBase, T>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T>(this, oldMethod);
        }

        public LocalAction<TBase, T1, T2> Method<T1, T2>(Expression<ActionReference<TBase, T1, T2>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2>(this, oldMethod);
        }

        public LocalAction<TBase, T1, T2, T3> Method<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2, T3>(this, oldMethod);
        }

        public LocalAction<TBase, T1, T2, T3, T4> Method<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2, T3, T4>(this, oldMethod);
        }


        public LocalPropertyGet<TBase, T> Property<T>(Func<T> propertyGet)
        {
            throw new NotImplementedException();
        }

        public LocalPropertySet<TBase, T> Property<T>(Action<T> propertySet)
        {
            throw new NotImplementedException();
        }

        protected override void OnLoad(DependencyClass modified)
        {
            var localClassAssemblyName = new AssemblyName("LocalClassAssembly");
            var localClassAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(localClassAssemblyName, AssemblyBuilderAccess.RunAndSave);
            var localClassModuleBuilder = localClassAssemblyBuilder.DefineDynamicModule("LocalClassModule", "LocalClassModule.dll");
            var localClassTypeBuilder = localClassModuleBuilder.DefineType("LocalClassType");
            if (typeof(TBase).IsInterface)
            {
                localClassTypeBuilder.AddInterfaceImplementation(typeof(TBase));
            }
            else
            {
                throw new NotImplementedException();
            }

            var localClassConstructorBuilder = localClassTypeBuilder.DefineConstructor(
                                                    MethodAttributes.Public |
                                                    MethodAttributes.HideBySig |
                                                    MethodAttributes.SpecialName |
                                                    MethodAttributes.RTSpecialName,
                                                    CallingConventions.Standard,
                                                    new Type[] { });
            localClassConstructorBuilder.ExpressBody(
            gen =>
            {
                gen.Eval(_ => _.Base());
            });

            int targetMethodInfoSetIndex = 0;
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                if (targetMethodInfo.Mode == SetupModes.Override)
                {
                    throw new NotImplementedException();
                }
                else if (targetMethodInfo.Mode == SetupModes.Implement)
                {

                    var cacheMethodBuilder =
                        localClassTypeBuilder.DefineField(
                            CacheFieldPrefix + "Method" + targetMethodInfoSetIndex++,
                            targetMethodInfo.DelegateType,
                            FieldAttributes.Private);

                    var methodBuilder = localClassTypeBuilder.DefineMethod(
                                            targetMethodInfo.OldMethod.Name,
                                            MethodAttributes.Public |
                                            MethodAttributes.HideBySig |
                                            MethodAttributes.NewSlot |
                                            MethodAttributes.Virtual |
                                            MethodAttributes.Final,
                                            CallingConventions.HasThis,
                                            targetMethodInfo.OldMethod.ReturnType,
                                            targetMethodInfo.OldMethod.ParameterTypes());

                    var parameterBuilders = new List<ParameterBuilder>();
                    foreach (var parameterName in targetMethodInfo.OldMethod.ParameterNames())
                    {
                        parameterBuilders.Add(methodBuilder.DefineParameter(1, ParameterAttributes.In, parameterName));
                    }

                    methodBuilder.ExpressBody(
                    gen =>
                    {
                        gen.Eval(_ => _.If(_.Ldfld(_.Extract(cacheMethodBuilder.Name, targetMethodInfo.DelegateType)) == null));
                        var dynamicMethod = default(DynamicMethod);
                        var returnType = targetMethodInfo.OldMethod.ReturnType;
                        var parameterTypes = targetMethodInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                        gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod("dynamicMethod", _.Expand(returnType), _.Expand(parameterTypes), true)));
                        var method4 = default(MethodInfo);
                        gen.Eval(_ => _.Addloc(method4, _.Expand(targetMethodInfo.DelegateType).GetMethod(
                                                            "Invoke",
                                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                            null, _.Expand(parameterTypes), null)));
                        var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(targetMethodInfo.NewMethod);
                        var cacheField = default(FieldInfo);
                        gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                            _.Expand(tmpCacheField.Name),
                                                            BindingFlags.NonPublic | BindingFlags.Static)));
                        var targetMethod = default(MethodInfo);
                        gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetMethodInfo.OldMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                            _.Expand(targetMethodInfo.OldMethod.Name),
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
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, method4));
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                        gen.Eval(_ => _.Stfld(_.Extract(cacheMethodBuilder.Name, targetMethodInfo.DelegateType), _.Extract(targetMethodInfo.DelegateType), dynamicMethod.CreateDelegate(_.Expand(targetMethodInfo.DelegateType))));
                        gen.Eval(_ => _.EndIf());
                        var invoke = targetMethodInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                        gen.Eval(_ => _.Return(_.Invoke(_.Ldfld(_.Extract(cacheMethodBuilder.Name, targetMethodInfo.DelegateType)), _.Extract(invoke), _.Ldarg(_.Extract<object[]>(targetMethodInfo.OldMethod.ParameterNames())))));

                        // HACK: Expand ～ シリーズはもう少し種類があると良さげ。
                    },
                    parameterBuilders.ToArray(),
                    new FieldBuilder[] { cacheMethodBuilder });
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            createdType = localClassTypeBuilder.CreateType();
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }

    //abstract class LocalMethodInjection
    //{
    //    public static LocalMethodInjection CreateInstance<TBase>(TypeBuilder localClassTypeBuilder, TargetMethodInfo targetMethodInfo)
    //    {
    //        // MEMO: 先に NewMethod の定義先情報で振り分けたほうが共通化できる処理が多そう。
    //        if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousInstance) == NewMethodType.AnonymousInstance)
    //        {
    //            return LocalAnonymousInstanceMethodInjection.CreateInstance<TBase>(localClassTypeBuilder, targetMethodInfo);
    //        }
    //        else if ((targetMethodInfo.NewMethodType & NewMethodType.AnonymousStatic) == NewMethodType.AnonymousStatic)
    //        {
    //            return LocalAnonymousStaticMethodInjection.CreateInstance<TBase>(localClassTypeBuilder, targetMethodInfo);
    //        }
    //        else
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    protected readonly TypeBuilder localClassTypeBuilder;
    //    protected readonly TargetMethodInfo targetMethodInfo;
    //    public LocalMethodInjection(TypeBuilder localClassTypeBuilder, TargetMethodInfo targetMethodInfo)
    //    {
    //        this.localClassTypeBuilder = localClassTypeBuilder;
    //        this.targetMethodInfo = targetMethodInfo;
    //    }

    //    public abstract void Apply(FieldDefinition cachedSettingDef, FieldDefinition cachedMethodDef);
    //}
}
