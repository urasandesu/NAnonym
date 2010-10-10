using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Urasandesu.NAnonym.ILTools;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Urasandesu.NAnonym.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClass : MarshalByRefObject
    {
        public static readonly string CacheFieldPrefix = "UNCD$<>0__Cached";

        //internal HashSet<TargetFieldInfo> TargetFieldInfoSet { get; private set; }
        internal HashSet<TargetMethodInfo> TargetInfoSet { get; private set; }
        GlobalClass modified;

        public GlobalClass()
        {
            //TargetFieldInfoSet = new HashSet<TargetFieldInfo>();
            TargetInfoSet = new HashSet<TargetMethodInfo>();
        }

        internal void Register()
        {
            modified = OnRegister();
            if (modified != null)
            {
                modified.Register();
            }
        }

        protected virtual GlobalClass OnRegister()
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

        protected virtual void OnLoad(GlobalClass modified)
        {
        }

        protected internal abstract string CodeBase { get; }
        protected internal abstract string Location { get; }
    }

    public class GlobalClass<TBase> : GlobalClass
    {
        Action<GlobalClass<TBase>> setupper;
        public void Setup(Action<GlobalClass<TBase>> setupper)
        {
            this.setupper = Required.NotDefault(setupper, () => setupper);
        }

        protected override GlobalClass OnRegister()
        {
            setupper(this);
            return null;
        }

        //public T Field<T>(Expression<Func<T>> variable, T value)
        //{
        //    var oldField = TypeSavable.GetFieldInfo(variable);
        //    TargetFieldInfoSet.Add(new TargetFieldInfo(oldField, value));
        //    return value;
        //}

        public GlobalFunc<TBase, TResult> Method<TResult>(Expression<FuncReference<TBase, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase,TResult>(this, oldMethod);
        }

        public GlobalFunc<TBase, T, TResult> Method<T, TResult>(Expression<FuncReference<TBase, T, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T, TResult>(this, oldMethod);
        }

        public GlobalFunc<TBase, T1, T2, TResult> Method<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, TResult>(this, oldMethod);
        }

        public GlobalFunc<TBase, T1, T2, T3, TResult> Method<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, T3, TResult>(this, oldMethod);
        }

        public GlobalFunc<TBase, T1, T2, T3, T4, TResult> Method<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, T3, T4, TResult>(this, oldMethod);
        }

        public GlobalAction<TBase> Method(Expression<ActionReference<TBase>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase>(this, oldMethod);
        }

        public GlobalAction<TBase, T> Method<T>(Expression<ActionReference<TBase, T>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T>(this, oldMethod);
        }

        public GlobalAction<TBase, T1, T2> Method<T1, T2>(Expression<ActionReference<TBase, T1, T2>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2>(this, oldMethod);
        }

        public GlobalAction<TBase, T1, T2, T3> Method<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2, T3>(this, oldMethod);
        }

        public GlobalAction<TBase, T1, T2, T3, T4> Method<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> reference)
        {
            var method = DependencyUtil.ExtractMethod(reference);
            var oldMethod = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2, T3, T4>(this, oldMethod);
        }




        protected override void OnLoad(GlobalClass modified)
        {
            // MEMO: ここで modified に来るのは、OnSetup() の戻り値なので、ここでは特に使う必要はない。
            var tbaseModuleDef = ModuleDefinition.ReadModule(new Uri(typeof(TBase).Assembly.CodeBase).LocalPath, new ReaderParameters() { ReadSymbols = true });
            var tbaseTypeDef = tbaseModuleDef.GetType(typeof(TBase).FullName);

            var UNCD_d__lt__rt_0__CachedSettingDef =
                        new FieldDefinition(
                            CacheFieldPrefix + "Setting",
                            MC::FieldAttributes.Private,
                            tbaseModuleDef.Import(typeof(GlobalClass)));
            tbaseTypeDef.Fields.Add(UNCD_d__lt__rt_0__CachedSettingDef);


            // TODO: FieldInfo の初期化はコンストラクタでやることになりそう。
            // TODO: 初期化に使われるのが class だとまずい。
            // TODO: ばらばらに初期化は難しいか。
            // TODO: 対象のクラスが何度も new されることを考えると、設定クラスを new して Register する一連の処理を DynamicMethod 化し、キャッシュしたほうが良い。
            var constructorDef = tbaseTypeDef.Methods.First(methodDef => methodDef.Name == ".ctor");
            var firstInstruction = constructorDef.Body.Instructions[0];
            constructorDef.ExpressBodyBefore(
            gen =>
            {
                var settingClassConstructorInfo = default(ConstructorInfo);
                gen.Eval(_ => _.Addloc(settingClassConstructorInfo, _.Expand(setupper.Method.DeclaringType).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null)));
                gen.Eval(_ => _.Stfld(_.Extract<GlobalClass>(UNCD_d__lt__rt_0__CachedSettingDef.Name), (GlobalClass)settingClassConstructorInfo.Invoke(null)));
                var settingClassMethodInfo = default(MethodInfo);
                gen.Eval(_ => _.Addloc(settingClassMethodInfo, _.Expand(setupper.Method.DeclaringType).GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null)));
                gen.Eval(_ => settingClassMethodInfo.Invoke(_.Ldfld(_.Extract<GlobalClass>(UNCD_d__lt__rt_0__CachedSettingDef.Name)), null));
            },
            firstInstruction);

            foreach (var targetInfo in TargetInfoSet)
            {
                switch (targetInfo.Mode)
                {
                    case SetupMode.Override:
                        throw new NotImplementedException();
                    case SetupMode.Replace:
                        {
                            var UNCD_d__lt__rt_0__CachedMethodDef =
                                new FieldDefinition(
                                    CacheFieldPrefix + "Method",
                                    MC::FieldAttributes.Private,
                                    tbaseModuleDef.Import(targetInfo.DelegateType));
                            tbaseTypeDef.Fields.Add(UNCD_d__lt__rt_0__CachedMethodDef);

                            var sourceMethodDef = tbaseTypeDef.Methods.FirstOrDefault(_methodDef => _methodDef.Equivalent(targetInfo.OldMethod));
                            string souceMethodName = sourceMethodDef.Name;
                            sourceMethodDef.Name = "__" + sourceMethodDef.Name;

                            // 元のメソッドと同じメソッドを追加（処理の中身は空にする）
                            var newMethod = sourceMethodDef.DuplicateWithoutBody();
                            newMethod.Name = souceMethodName;
                            tbaseTypeDef.Methods.Add(newMethod);

                            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(targetInfo.NewMethod);
                            newMethod.Body.InitLocals = true;
                            newMethod.ExpressBody(
                            gen =>
                            {
                                gen.Eval(_ => _.If(_.Ldfld(_.Extract(UNCD_d__lt__rt_0__CachedMethodDef.Name, targetInfo.DelegateType)) == null));
                                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = default(DynamicMethod);
                                var returnType = targetInfo.OldMethod.ReturnType;
                                var parameterTypes = targetInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                                if (tmpCacheField != null)
                                {
                                    gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", _.Expand(returnType), _.Expand(parameterTypes), true)));
                                }
                                else
                                {
                                    gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", _.Expand(returnType), _.Expand(new Type[] { typeof(TBase) }.Concat(parameterTypes).ToArray()), typeof(TBase), true)));
                                }

                                var ctor3 = default(ConstructorInfo);
                                var method4 = default(MethodInfo);
                                if (tmpCacheField != null)
                                {
                                    gen.Eval(_ => _.Addloc(ctor3, _.Expand(targetInfo.DelegateType).GetConstructor(
                                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                        null,
                                                                        new Type[] 
                                                                    { 
                                                                        typeof(Object), 
                                                                        typeof(IntPtr) 
                                                                    }, null)));
                                    gen.Eval(_ => _.Addloc(method4, _.Expand(targetInfo.DelegateType).GetMethod(
                                                                        "Invoke",
                                                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                        null, _.Expand(parameterTypes), null)));
                                }
                                
                                var cacheField = default(FieldInfo);
                                var cacheField2 = default(FieldInfo);
                                if (tmpCacheField != null)
                                {
                                    gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                                        _.Expand(tmpCacheField.Name),
                                                                        BindingFlags.NonPublic | BindingFlags.Static)));
                                }
                                else
                                {
                                    gen.Eval(_ => _.Addloc(cacheField2, _.Expand(typeof(TBase)).GetField(
                                                                            _.Expand(UNCD_d__lt__rt_0__CachedSettingDef.Name),
                                                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));
                                }

                                var targetMethod = default(MethodInfo);
                                var targetMethod2 = default(MethodInfo);
                                if (tmpCacheField != null)
                                {
                                    gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetInfo.NewMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                                        _.Expand(targetInfo.NewMethod.Name),
                                                                        BindingFlags.NonPublic | BindingFlags.Static)));
                                }
                                else
                                {
                                    gen.Eval(_ => _.Addloc(targetMethod2, _.Expand(targetInfo.NewMethod.DeclaringType).GetMethod(
                                                                            _.Expand(targetInfo.NewMethod.Name), 
                                                                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
                                }


                                var il = default(ILGenerator);
                                gen.Eval(_ => _.Addloc(il, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.GetILGenerator()));
                                if (tmpCacheField != null)
                                {
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
                                    gen.Eval(_ => _.Stfld(_.Extract(UNCD_d__lt__rt_0__CachedMethodDef.Name, targetInfo.DelegateType), _.Extract(targetInfo.DelegateType), CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(_.Expand(targetInfo.DelegateType))));
                                    gen.Eval(_ => _.EndIf());
                                }
                                else
                                {
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, cacheField2));
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
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Callvirt, targetMethod2));
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                                    gen.Eval(_ => _.Stfld(_.Extract(UNCD_d__lt__rt_0__CachedMethodDef.Name, targetInfo.DelegateType), _.Extract(targetInfo.DelegateType), CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(_.Expand(targetInfo.DelegateType), _.This())));
                                    gen.Eval(_ => _.EndIf());
                                }
                                var invoke = targetInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                                gen.Eval(_ => _.Return(_.Invoke(_.Ldfld(_.Extract(UNCD_d__lt__rt_0__CachedMethodDef.Name, targetInfo.DelegateType)), _.Extract(invoke), _.Ldarg(_.Extract<object[]>(targetInfo.OldMethod.ParameterNames())))));

                                // HACK: Expand ～ シリーズはもう少し種類があると良さげ。
                            });

                        }
                        break;
                    case SetupMode.Implement:
                        throw new NotImplementedException();
                    default:
                        throw new NotSupportedException();
                }
            }

            tbaseModuleDef.Write(new Uri(typeof(TBase).Assembly.CodeBase).LocalPath, new WriterParameters() { WriteSymbols = true });
        }

        protected internal override string CodeBase
        {
            get { return typeof(TBase).Assembly.CodeBase; }
        }

        protected internal override string Location
        {
            get { return typeof(TBase).Assembly.Location; }
        }
    }





    //public class TargetFieldInfo
    //{
    //    public FieldInfo OldField { get; set; }
    //    public object Value { get; set; }

    //    public TargetFieldInfo()
    //    {
    //    }

    //    public TargetFieldInfo(FieldInfo oldField, object value)
    //    {
    //        OldField = oldField;
    //        Value = value;
    //    }
    //}
}
