using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Urasandesu.NAnonym.CREUtilities;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Urasandesu.NAnonym.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SR = System.Reflection;
using MC = Mono.Cecil;

namespace Urasandesu.NAnonym.DI
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

            var cacheField = TypeAnalyzer.GetCacheFieldIfAnonymous(method.Method);
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
                // TODO: その場で評価しちゃえる処理が欲しい。ただし、最終的な情報が CIL レベルで埋め込めるものであるという制限は必要！
                // TODO: 
                //var cacheField = default(FieldInfo);
                //gen.Eval(_=>_.Addloc(
                var il = default(ILGenerator);
                gen.Eval(_ => _.Addloc(il, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method.GetILGenerator()));
                var label27 = default(Label);
                gen.Eval(_ => _.Addloc(label27, il.DefineLabel()));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ldsfld, cacheField)); // TODO: 変数のスコープが違いすぎる。。。TotableScope で Bind しようにも、できるタイミングではすでに情報が見えないところに。。。
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Brtrue_S, label27));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ldnull));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ldftn, method.Method));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Newobj, ctor3));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Stsfld, cacheField));
                gen.Eval(_ => il.MarkLabel(label27));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ldsfld, cacheField));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ldarg_0));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Callvirt, method4));
                gen.Eval(_ => il.Emit(SR.Emit.OpCodes.Ret));
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



    // NOTE: ExpressiveMethodBodyGenerator で完全代替可能。廃止。
    //// HACK: このクラスもどちらかというと CRUtilities 系。
    //// HACK: もう少し一般化が必要？I/F を同じにするとか、意識して作成するローカル変数以外（現状 tmp で指定してるやつとか）を隠蔽するとか。
    //public class MethodBuilder<T, TResult>
    //{
    //    readonly ModuleDefinition tbaseModule;

    //    public MethodBuilder(ModuleDefinition tbaseModule)
    //    {
    //        this.tbaseModule = tbaseModule;
    //    }

    //    public void PushNewDynamicMethod(ILProcessor gen, string name, VariableDefinition tmp)
    //    {
    //        Required.Assert(tmp, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp);

    //        // HACK: TResult や T を使う以外の部分は一般化できるはず（他の Func<TResult>, Action<T>, とかへの対策）
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, name);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newobj, tbaseModule.Import(References.DynamicMethod.DynamicMethodConstructor));
    //    }

    //    // HACK: 型指定 + GetConstructor で分けられそう。
    //    public void PushFuncGetConstructor(ILProcessor gen, VariableDefinition tmp1, VariableDefinition tmp2)
    //    {
    //        Required.Assert(tmp1, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp1);
    //        Required.Assert(tmp2, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp2);

    //        // HACK: TResult や T を使う以外の部分は一般化できるはず（他の Func<TResult>, Action<T>, とかへの対策）
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Func<,>)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.MakeGenericType));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Object)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(IntPtr)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.GetConstructor));
    //    }

    //    // HACK: 型指定 + GetMethod で分けられそう。
    //    public void PushFuncInvoke(ILProcessor gen, VariableDefinition tmp1, VariableDefinition tmp2)
    //    {
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Func<,>)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.MakeGenericType));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, "Invoke");
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(String)));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
    //        gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.GetMethod));
    //    }
    //}


    // NOTE: ExpressiveMethodBodyGenerator で完全代替可能。廃止。
    //// HACK: このクラスもどちらかというと CRUtilities 系。
    //public static class References
    //{
    //    public static class Type
    //    {
    //        public static readonly MethodInfo GetTypeFromHandle = 
    //            typeof(System.Type).GetMethod(
    //                "GetTypeFromHandle",
    //                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
    //                null,
    //                new System.Type[] 
    //                {
    //                    typeof(RuntimeTypeHandle) 
    //                },
    //                null);

    //        public static readonly MethodInfo MakeGenericType = 
    //            typeof(System.Type).GetMethod(
    //                "MakeGenericType",
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //                null,
    //                new System.Type[] 
    //                { 
    //                    typeof(System.Type[]) 
    //                },
    //                null);

    //        public static readonly MethodInfo GetConstructor =
    //            typeof(System.Type).GetMethod(
    //                "GetConstructor",
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //                null,
    //                new System.Type[] 
    //                { 
    //                    typeof(BindingFlags), 
    //                    typeof(Binder), 
    //                    typeof(System.Type[]), 
    //                    typeof(ParameterModifier[]) 
    //                },
    //                null);

    //        public static readonly MethodInfo GetMethod =
    //            typeof(System.Type).GetMethod(
    //                "GetMethod",
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //                null,
    //                new System.Type[] 
    //                { 
    //                    typeof(String), 
    //                    typeof(BindingFlags), 
    //                    typeof(Binder), 
    //                    typeof(System.Type[]), 
    //                    typeof(ParameterModifier[]) 
    //                },
    //                null);
    //    }

    //    public static class DynamicMethod
    //    {
    //        public static readonly ConstructorInfo DynamicMethodConstructor = 
    //            typeof(System.Reflection.Emit.DynamicMethod).GetConstructor(
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //                null,
    //                new System.Type[] 
    //                { 
    //                    typeof(String), 
    //                    typeof(System.Type), 
    //                    typeof(System.Type[]), 
    //                    typeof(Boolean) 
    //                },
    //                null);

    //        public static readonly MethodInfo GetILGenerator =
    //            typeof(System.Reflection.Emit.DynamicMethod).GetMethod(
    //                "GetILGenerator", 
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
    //                null, 
    //                new System.Type[] { }, 
    //                null);
    //    }

    //    public static class ILGenerator
    //    {
    //        public static readonly MethodInfo DefineLabel = 
    //            typeof(System.Reflection.Emit.ILGenerator).GetMethod(
    //                "DefineLabel", 
    //                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
    //                null, 
    //                new System.Type[] { }, 
    //                null);
    //    }

    //    public static class OpCodes
    //    {

    //    }
    //}

















    //class Hoge
    //{
    //    static void Main()
    //    {
    //        var hogeDef = typeof(Hoge).ToTypeDef();

    //        var testDef = new MethodDefinition("Test", 
    //            Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.Static, hogeDef.Module.Import(typeof(void)));
    //        hogeDef.Methods.Add(testDef);

    //        // もっと直接的な指定（LambdaExpression から、とか）で行けるんじゃない？
    //        // 例えば、↓みたいに。
    //        var egen = new ExpressiveILProcessor(testDef);
    //        egen.Emit(_ => Console.WriteLine("aiueo"));

    //        // 現状の式木だと、変数の宣言ができません。
    //        // MEMO: 初めて代入が行われた際に変数を宣言してしまう。Javascript 方式もいいかも？
    //        // MEMO: ただし、通常手順的に、「1. 一通り書く」「2. Excel に貼り付け」「3. 一括変換」みたいにやりたいから、Addlocがやはり必要。
    //        // → こんな感じで騙し騙し
    //        int a = 0;
    //        egen.Emit(_ => _.Addloc(a, default(int)));
    //        egen.Emit(_ => _.Stloc(a, 100));
    //        //egen.Emit(_ => _.Addloc(() => a, default(int)));
    //        //egen.Emit(_ => _.Stloc(() => a, 100));

    //        // ローカル変数に入れた情報にアクセスしたい場合はどうするのだ。
    //        // → こんな感じで騙し騙し
    //        var cachedAnonymousMethod = default(DynamicMethod);
    //        var gen = default(ILGenerator);
    //        var label27 = default(Label);
    //        egen.Emit(_ => _.Addloc(cachedAnonymousMethod, new DynamicMethod("cachedAnonymousMethod", typeof(string), new Type[] { typeof(string) }, true)));
    //        egen.Emit(_ => _.Addloc(gen, _.Ldloc(cachedAnonymousMethod).GetILGenerator()));
    //        egen.Emit(_ => _.Addloc(label27, _.Ldloc(gen).DefineLabel()));
    //        egen.Emit(_ => _.Ldloc(gen).Emit(System.Reflection.Emit.OpCodes.Brtrue_S, _.Ldloc(label27)));

    //        // 通常の Emit 処理混合できるよね、当然？
    //        // → こんな感じで騙し騙し
    //        egen.Emit(_ => _.Direct.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)100));
    //        egen.Emit(_ => _.Direct.Emit(Mono.Cecil.Cil.OpCodes.Stloc, _.Locals(() => a)));
    //    }
    //}


}
