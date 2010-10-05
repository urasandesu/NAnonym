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
        internal HashSet<TargetInfo> TargetInfoSet { get; private set; }
        GlobalClass modified;

        public GlobalClass()
        {
            TargetInfoSet = new HashSet<TargetInfo>();
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

            foreach (var targetInfo in TargetInfoSet)
            {
                switch (targetInfo.Mode)
                {
                    case SetupMode.Override:
                        throw new NotImplementedException();
                    case SetupMode.Replace:
                        {
                            var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache = default(object);
                            var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCacheDef =
                                new FieldDefinition(TypeSavable.GetName(
                                    () => CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache),
                                    MC::FieldAttributes.Private,
                                    tbaseModuleDef.Import(targetInfo.DelegateType));
                            tbaseTypeDef.Fields.Add(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCacheDef);

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
                                gen.Eval(_ => _.If(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache == null));
                                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = default(DynamicMethod);
                                var returnType = targetInfo.OldMethod.ReturnType;
                                var parameterTypes = targetInfo.OldMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                                gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", _.Expand(returnType), _.Expand(parameterTypes), true)));
                                var ctor3 = default(ConstructorInfo);
                                gen.Eval(_ => _.Addloc(ctor3, _.Expand(targetInfo.DelegateType).GetConstructor(
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                    null,
                                                                    new Type[] 
                                                                    { 
                                                                        typeof(Object), 
                                                                        typeof(IntPtr) 
                                                                    }, null)));
                                var method4 = default(MethodInfo);
                                gen.Eval(_ => _.Addloc(method4, _.Expand(targetInfo.DelegateType).GetMethod(
                                                                    "Invoke",
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                    null, _.Expand(parameterTypes), null)));
                                var cacheField = default(FieldInfo);
                                gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                                    _.Expand(tmpCacheField.Name),
                                                                    BindingFlags.NonPublic | BindingFlags.Static)));
                                var targetMethod = default(MethodInfo);
                                gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetInfo.NewMethod.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                                    _.Expand(targetInfo.NewMethod.Name),
                                                                    BindingFlags.NonPublic | BindingFlags.Static)));
                                var il = default(ILGenerator);
                                gen.Eval(_ => _.Addloc(il, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.GetILGenerator()));
                                var label27 = default(Label);
                                gen.Eval(_ => _.Addloc(label27, il.DefineLabel()));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldsfld, cacheField));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Brtrue_S, label27));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldnull));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldftn, targetMethod));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Newobj, ctor3));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Stsfld, cacheField));
                                gen.Eval(_ => il.MarkLabel(label27));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldsfld, cacheField));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ldarg_0));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Callvirt, method4));
                                gen.Eval(_ => il.Emit(SR::Emit.OpCodes.Ret));
                                gen.Eval(_ => _.ExpandStfld(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, targetInfo.DelegateType, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(_.Expand(targetInfo.DelegateType))));
                                gen.Eval(_ => _.EndIf());
                                var invoke = targetInfo.DelegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
                                gen.Eval(_ => _.Return(_.ExpandInvoke(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1MethodCache, invoke, _.ExpandLdargs(targetInfo.OldMethod.ParameterNames()))));

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
}
