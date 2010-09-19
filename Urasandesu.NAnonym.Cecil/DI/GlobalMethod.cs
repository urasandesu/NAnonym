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

namespace Urasandesu.NAnonym.Cecil.DI
{
    // HACK: あれ？ where とかいらなくね？もはやなんでも良い気がしてきた。
    public class GlobalMethod<TBase, T, TResult> where TBase : class
    {
        readonly ModuleDefinition tbaseModule;
        readonly TypeDefinition tbaseType;
        readonly MethodDefinition method;

        public GlobalMethod(MethodDefinition method)
        {
            this.method = method;
            tbaseModule = this.method.Module;
            tbaseType = this.method.DeclaringType;
        }

        public GlobalClass<TBase> As(Func<T, TResult> method)
        {
            //var methodType = method.GetType().ToTypeRef();

            // DynamicMethod で作成するデリゲートをキャッシュするためのフィールドを追加
            // HACK: 重複しない名前にしたい。
            var __cachedField = new FieldDefinition("__cached" + this.method.Name, Mono.Cecil.FieldAttributes.Static, tbaseModule.Import(method.GetType()));
            tbaseType.Fields.Add(__cachedField);

            // 元のメソッド名はリネーム
            // HACK: 重複しない名前にしたい。
            string souceMethodName = this.method.Name;
            this.method.Name = "__" + this.method.Name; 

            // 元のメソッドと同じメソッドを追加（処理の中身は空にする）
            var newMethod = this.method.DuplicateWithoutBody();
            newMethod.Name = souceMethodName;
            tbaseType.Methods.Add(newMethod);

            var tmpCacheField = TypeAnalyzer.GetCacheFieldIfAnonymous(method.Method);
            newMethod.Body.InitLocals = true;
            newMethod.ExpressBody(
            gen =>
            {
                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = default(DynamicMethod);
                gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method, new DynamicMethod("CS$<>9__CachedAnonymousMethodDelegate1Method", typeof(TResult), new Type[] { typeof(T) }, true)));
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
                gen.Eval(_ => _.Addloc(targetMethod, Type.GetType(_.Expand(method.Method.DeclaringType.AssemblyQualifiedName)).GetMethod(
                                                    _.Expand(method.Method.Name),
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
                var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker = default(Func<T, TResult>);
                gen.Eval(_ => _.Addloc(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker, (Func<T, TResult>)CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.CreateDelegate(typeof(Func<T, TResult>))));
                T value = default(T);
                gen.Eval(_ => _.Return(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker.Invoke(value)));
            });

            return null;
        }


        public GlobalClass<TBase> Override(Func<TBase, Func<T, TResult>> method)
        {
            throw new NotImplementedException();
        }

        public GlobalClass<TBase> Instead(Func<TBase, Func<T, TResult>> method)
        {
            throw new NotImplementedException();
        }
    }
}
