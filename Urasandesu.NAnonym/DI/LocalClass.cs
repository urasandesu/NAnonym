using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using Urasandesu.NAnonym.Mixins.System.Reflection;
using Urasandesu.NAnonym.ILTools.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyClass : MarshalByRefObject
    {
        DependencyClass modified;

        internal HashSet<TargetMethodInfo> TargetMethodInfoSet { get; private set; }

        public DependencyClass()
        {
            TargetMethodInfoSet = new HashSet<TargetMethodInfo>();
        }

        internal void Register()
        {
            modified = OnRegister();
            if (modified != null)
            {
                modified.Register();
            }
        }

        protected virtual DependencyClass OnRegister()
        {
            return null;
        }

        internal void Load()
        {
            OnLoad(modified);
            if (modified != null)
            {
                modified.Load();
            }
        }

        protected virtual void OnLoad(DependencyClass modified)
        {
        }
    }

    public abstract class LocalClass : DependencyClass
    {
    }

    public sealed class LocalClass<TBase> : LocalClass
    {
        readonly Type tbaseType = typeof(TBase);

        internal HashSet<TargetMethodInfo> TargetInfoSet { get; private set; }

        Type createdType;

        protected override void OnLoad(DependencyClass modified)
        {
            var localClassAssemblyName = new AssemblyName("LocalClassAssembly");
            var localClassAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(localClassAssemblyName, AssemblyBuilderAccess.Run);
            var localClassModuleBuilder = localClassAssemblyBuilder.DefineDynamicModule("LocalClassModule");
            var localClassTypeBuilder = localClassModuleBuilder.DefineType("LocalClassType");
            if (tbaseType.IsInterface)
            {
                localClassTypeBuilder.AddInterfaceImplementation(tbaseType);
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

            foreach (var targetInfo in TargetInfoSet)
            {
                if (targetInfo.Mode == SetupModes.Override)
                {
                    throw new NotImplementedException();
                }
                else if (targetInfo.Mode == SetupModes.Implement)
                {

                    var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache = default(object);
                    var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCacheFieldBuilder =
                        localClassTypeBuilder.DefineField(
                            TypeSavable.GetName(() => CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache),
                            targetInfo.DelegateType,
                            FieldAttributes.Private);

                    var methodBuilder = localClassTypeBuilder.DefineMethod(
                                            targetInfo.OldMethod.Name,
                                            MethodAttributes.Public |
                                            MethodAttributes.HideBySig |
                                            MethodAttributes.NewSlot |
                                            MethodAttributes.Virtual |
                                            MethodAttributes.Final,
                                            CallingConventions.HasThis,
                                            targetInfo.OldMethod.ReturnType,
                                            targetInfo.OldMethod.ParameterTypes());
                    // とりあえず
                    var parameterBuilder = methodBuilder.DefineParameter(1, ParameterAttributes.In, targetInfo.OldMethod.ParameterNames()[0]);
                    methodBuilder.ExpressBody(
                    gen =>
                    {
                        gen.Eval(_ => _.If(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache == null));
                        var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = default(DynamicMethod);
                        var returnType = targetInfo.OldMethod.ReturnType;
                        var parameterTypes = targetInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                        gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", _.Expand(returnType), _.Expand(parameterTypes), true)));
                        var method4 = default(MethodInfo);
                        gen.Eval(_ => _.Addloc(method4, _.Expand(targetInfo.DelegateType).GetMethod(
                                                            "Invoke",
                                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                            null, _.Expand(parameterTypes), null)));
                        var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByRunningState(targetInfo.NewMethod);
                        var cacheField = default(FieldInfo);
                        gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                            _.Expand(tmpCacheField.Name),
                                                            BindingFlags.NonPublic | BindingFlags.Static)));
                        var targetMethod = default(MethodInfo);
                        gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetInfo.OldMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                            _.Expand(targetInfo.OldMethod.Name),
                                                            BindingFlags.NonPublic | BindingFlags.Static)));
                        var il = default(ILGenerator);
                        gen.Eval(_ => _.Addloc(il, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.GetILGenerator()));
                        gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldsfld, cacheField));
                        gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldarg_0));
                        gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Callvirt, method4));
                        gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ret));
                        gen.Eval(_ => _.Stfld(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, _.Extract(targetInfo.DelegateType), CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(_.Expand(targetInfo.DelegateType))));
                        gen.Eval(_ => _.EndIf());
                        var invoke = targetInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                        gen.Eval(_ => _.Return(_.Invoke(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, _.Extract(invoke), _.Ldarg(_.Extract<object[]>(targetInfo.OldMethod.ParameterNames())))));

                        // HACK: Expand ～ シリーズはもう少し種類があると良さげ。
                    },
                    new ParameterBuilder[] { parameterBuilder },
                    new FieldBuilder[] { CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCacheFieldBuilder });
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            createdType = localClassTypeBuilder.CreateType();
        }

        public LocalClass()
        {
            TargetInfoSet = new HashSet<TargetMethodInfo>();
        }

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

        public LocalMethod<TBase, T, TResult> Method<T, TResult>(Expression<FuncReference<TBase, T, TResult>> expression)
        {
            var method = DependencyUtil.ExtractMethod(expression);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new LocalMethod<TBase, T, TResult>(this, oldMethod);
        }


        public LocalPropertyGet<TBase, T> Property<T>(Func<T> propertyGet)
        {
            throw new NotImplementedException();
        }

        public LocalPropertySet<TBase, T> Property<T>(Action<T> propertySet)
        {
            throw new NotImplementedException();
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}
