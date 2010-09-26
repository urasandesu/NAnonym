using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Mono.Cecil;
using System.Reflection;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DI;
using System.Reflection.Emit;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.DI
{
    public class GlobalClass<TBase> : GlobalClassBase where TBase : class
    {
        internal HashSet<TargetInfo> TargetInfoSet { get; private set; }

        public GlobalClass()
        {
            TargetInfoSet = new HashSet<TargetInfo>();
        }

        public GlobalClass<TBase> Setup(Action<GlobalClass<TBase>> overrider)
        {
            if (overrider == null) throw new ArgumentNullException("overrider");
            overrider(this);
            return this;
        }

        public GlobalMethod<TBase, T, TResult> Method<T, TResult>(Func<T, TResult> func)
        {
            return new GlobalMethod<TBase, T, TResult>(this, func);
        }

        protected override GlobalClassBase OnSetup()
        {
            // Setup(Action<GlobalClass<TBase>>) で大方済んでるので、特にここでなにかやる必要は無さそう。
            return null;
        }




        protected override void OnLoad(GlobalClassBase modified)
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
                    case SetupMode.Instead:
                        {
                            var sourceMethodDef = tbaseTypeDef.Methods.FirstOrDefault(_methodDef => _methodDef.Equivalent(targetInfo.SourceMethodInfo));
                            string souceMethodName = sourceMethodDef.Name;
                            sourceMethodDef.Name = "__" + sourceMethodDef.Name;

                            // 元のメソッドと同じメソッドを追加（処理の中身は空にする）
                            var newMethod = sourceMethodDef.DuplicateWithoutBody();
                            newMethod.Name = souceMethodName;
                            tbaseTypeDef.Methods.Add(newMethod);

                            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymousByDirective(targetInfo.DestinationMethodInfo);
                            newMethod.Body.InitLocals = true;
                            newMethod.ExpressBody(
                            gen =>
                            {
                                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = default(DynamicMethod);
                                gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", typeof(string), new Type[] { typeof(string) }, true)));
                                var ctor3 = default(ConstructorInfo);
                                gen.Eval(_ => _.Addloc(ctor3, typeof(System.Func<,>).MakeGenericType(typeof(String), typeof(String)).GetConstructor(
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                    null,
                                                                    new Type[] 
                                                                    { 
                                                                        typeof(Object), 
                                                                        typeof(IntPtr) 
                                                                    }, null)));
                                var method4 = default(MethodInfo);
                                gen.Eval(_ => _.Addloc(method4, typeof(System.Func<,>).MakeGenericType(typeof(String), typeof(String)).GetMethod(
                                                                    "Invoke",
                                                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                                                    null,
                                                                    new Type[] 
                                                                    { 
                                                                        typeof(String) 
                                                                    }, null)));
                                var cacheField = default(FieldInfo);
                                gen.Eval(_ => _.Addloc(cacheField, Type.GetType(_.Expand(tmpCacheField.DeclaringType.AssemblyQualifiedName)).GetField(
                                                                    _.Expand(tmpCacheField.Name),
                                                                    BindingFlags.NonPublic | BindingFlags.Static)));
                                var targetMethod = default(MethodInfo);
                                gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(targetInfo.DestinationMethodInfo.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                                    _.Expand(targetInfo.DestinationMethodInfo.Name),
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
                                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker = default(Func<string, string>);
                                gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker, (Func<string, string>)CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(typeof(Func<string, string>))));
                                gen.Eval(_ => _.Return(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker.Invoke(_.ExpandAndLdarg<string>(targetInfo.DestinationMethodInfo.GetParameters()[0].Name))));

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

        protected internal override string AssemblyCodeBase
        {
            get { return typeof(TBase).Assembly.CodeBase; }
        }

        protected internal override string AssemblyLocation
        {
            get { return typeof(TBase).Assembly.Location; }
        }
    }
}
