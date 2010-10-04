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
    public abstract class LocalClass : MarshalByRefObject
    {
        LocalClass modified;

        internal void Register()
        {
            modified = OnRegister();
            if (modified != null)
            {
                modified.Register();
            }
        }

        protected virtual LocalClass OnRegister()
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

        protected virtual void OnLoad(LocalClass modified)
        {
        }
    }

    public sealed class LocalClass<TBase> : LocalClass
    {
        readonly Type tbaseType = typeof(TBase);

        internal HashSet<TargetInfo> TargetInfoSet { get; private set; }

        Type createdType;

        // TODO: SetupMode.Override 実装。
        // TODO: LocalClassAssembly キャッシュ機構追加。
        // DOING: 引数数を汎用デリゲート型まで拡張。
        // DONE: LocalClassBase もたぶん必要。Generic な型に、型パラメータ関係ない処理を括りだした I/F クラスがあると便利なのが世の常。
        // DONE: LocalMethod 生成メソッドの引数用デリゲート型追加。
        // DONE: フィールドへの参照許可を ExpressiveMethodBodyGenerator に追加。
        // DONE: EMBG リファクタリング。
        // DONE: 2項演算子（null 比較）実装。
        // DONE: If 文、Scope を導入。
        // DONE: 実際の処理を呼び出すための DynamicMethod 用キャッシュ追加。
        // DONE: 戻り値型、引数型の制限解除。
        protected override void OnLoad(LocalClass modified)
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
                switch (targetInfo.Mode)
                {
                    case SetupMode.Override:
                        throw new NotImplementedException();
                    case SetupMode.Replace:
                        throw new NotSupportedException();
                    case SetupMode.Implement:
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
                                gen.Eval(_ => _.ExpandStfld(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, targetInfo.DelegateType, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(_.Expand(targetInfo.DelegateType))));
                                gen.Eval(_ => _.EndIf());
                                var invoke = targetInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                                gen.Eval(_ => _.Return(_.ExpandInvoke(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, invoke, _.ExpandLdargs(targetInfo.OldMethod.ParameterNames()))));

                                // HACK: Expand ～ シリーズはもう少し種類があると良さげ。
                            },
                            new ParameterBuilder[] { parameterBuilder },
                            new FieldBuilder[] { CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCacheFieldBuilder });
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            createdType = localClassTypeBuilder.CreateType();
        }

        public LocalClass()
        {
            TargetInfoSet = new HashSet<TargetInfo>();
        }

        Action<LocalClass<TBase>> setupper;

        public void Setup(Action<LocalClass<TBase>> setupper)
        {
            Required.NotDefault(setupper, () => setupper);
            this.setupper = setupper;
        }

        protected override LocalClass OnRegister()
        {
            setupper(this);
            return null;
        }


        //// HACK: method は MethodInfo と間違いやすい。汎用デリゲートについてはもう、Action -> action、Func -> func みたいな変数命名規則にしたほうがいいかも？
        //public LocalMethod<TBase> Method(Action method)
        //{
        //    throw new NotImplementedException();
        //}

        //public LocalMethod<TBase, TResult> Method<TResult>(Func<TResult> method)
        //{
        //    throw new NotImplementedException();
        //}

        //public LocalMethod<TBase, T, TResult> Method<T, TResult>(Func<T, TResult> func)
        //{
        //    return new LocalMethod<TBase, T, TResult>(this, func);
        //}

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
