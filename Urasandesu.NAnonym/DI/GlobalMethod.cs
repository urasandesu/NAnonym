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

namespace Urasandesu.NAnonym.DI
{
    // HACK: あれ？ where とかいらなくね？もはやなんでも良い気がしてきた。
    public class GlobalMethod<TBase, T, TResult> where TBase : class
    {
        private readonly ModuleDefinition tbaseModule;
        private readonly TypeDefinition tbaseType;
        private readonly MethodDefinition method;

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

            // MEMO: やっぱり 1 処理ずつやってくのがいいね・・・。
            // Preparing Reflection instances
            //var methods = new ParameterMethods(tbaseModule);
            var builder = new MethodBuilder<T, TResult>(tbaseModule);
            var cacheField = TypeAnalyzer.GetCacheFieldIfAnonymous(method.Method);

            newMethod.Body.InitLocals = true;
            var CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = 
                new VariableDefinition("CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method", tbaseModule.Import(typeof(DynamicMethod)));
            newMethod.Body.Variables.Add(CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method);
            var CS_0_0000 = new VariableDefinition("CS$0$0000", tbaseModule.Import(typeof(Type[])));
            newMethod.Body.Variables.Add(CS_0_0000);
            var CS_0_0001 = new VariableDefinition("CS$0$0001", tbaseModule.Import(typeof(Type[])));
            newMethod.Body.Variables.Add(CS_0_0001);
            var CS_0_0002 = new VariableDefinition("CS$0$0002", tbaseModule.Import(typeof(Type[])));
            newMethod.Body.Variables.Add(CS_0_0002);
            var CS_0_0003 = new VariableDefinition("CS$0$0003", tbaseModule.Import(typeof(Type[])));
            newMethod.Body.Variables.Add(CS_0_0003);
            var CS_0_0004 = new VariableDefinition("CS$0$0004", tbaseModule.Import(typeof(Type[])));
            newMethod.Body.Variables.Add(CS_0_0004);

            var gen = newMethod.Body.GetILProcessor();

            builder.PushNewDynamicMethod(gen, "CS_<>9__CachedAnonymousMethodDelegate1Method", CS_0_0000);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method);

            builder.PushFuncGetConstructor(gen, CS_0_0001, CS_0_0002);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Pop);

            builder.PushFuncInvoke(gen, CS_0_0003, CS_0_0004);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Pop);

            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.DynamicMethod.GetILGenerator));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Pop);

            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ret);



            #region 古い2
            /*
            // Preparing Reflection instances
            // いやー、ほんと 1 行ずつのイメージで作成したほうがいいんじゃね？
            var method1 = typeof(Type).ToTypeDef().GetMethod(
                "GetTypeFromHandle", 
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(RuntimeTypeHandle) 
                });
            var ctor2 = typeof(DynamicMethod).ToTypeDef().GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(String), 
                    typeof(Type), 
                    typeof(Type[]), 
                    typeof(Boolean) 
                });
            var method3 = typeof(Type).ToTypeDef().GetMethod(
                "GetField", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(String), 
                    typeof(BindingFlags) 
                });
            var method4 = typeof(Type).ToTypeDef().GetMethod(
                "GetMethod", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(String), 
                    typeof(BindingFlags), 
                    typeof(Binder), 
                    typeof(Type[]), 
                    typeof(ParameterModifier[]) 
                });
            var method5 = typeof(Type).ToTypeDef().GetMethod(
                "MakeGenericType", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(Type[]) 
                });
            var method6 = typeof(Type).ToTypeDef().GetMethod(
                "GetConstructor", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                new Type[] 
                { 
                    typeof(BindingFlags), 
                    typeof(Binder), 
                    typeof(Type[]), 
                    typeof(ParameterModifier[]) 
                });
            var method7 = typeof(DynamicMethod).ToTypeDef().GetMethod(
                "GetILGenerator",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                new Type[]{
            }
                );
            MethodInfo method8 = typeof(ILGenerator).GetMethod(
                "DefineLabel",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            },
                null
                );
            FieldInfo field9 = typeof(System.Reflection.Emit.OpCodes).GetField("Ldsfld", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method10 = typeof(ILGenerator).GetMethod(
                "Emit",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(System.Reflection.Emit.OpCode),
            typeof(FieldInfo)
            },
                null
                );
            FieldInfo field11 = typeof(System.Reflection.Emit.OpCodes).GetField("Brtrue_S", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method12 = typeof(ILGenerator).GetMethod(
                "Emit",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(System.Reflection.Emit.OpCode),
            typeof(Label)
            },
                null
                );
            FieldInfo field13 = typeof(System.Reflection.Emit.OpCodes).GetField("Ldnull", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method14 = typeof(ILGenerator).GetMethod(
                "Emit",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(System.Reflection.Emit.OpCode)
            },
                null
                );
            FieldInfo field15 = typeof(System.Reflection.Emit.OpCodes).GetField("Ldftn", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method16 = typeof(ILGenerator).GetMethod(
                "Emit",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(System.Reflection.Emit.OpCode),
            typeof(MethodInfo)
            },
                null
                );
            FieldInfo field17 = typeof(System.Reflection.Emit.OpCodes).GetField("Newobj", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method18 = typeof(ILGenerator).GetMethod(
                "Emit",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(System.Reflection.Emit.OpCode),
            typeof(ConstructorInfo)
            },
                null
                );
            FieldInfo field19 = typeof(System.Reflection.Emit.OpCodes).GetField("Stsfld", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method20 = typeof(ILGenerator).GetMethod(
                "MarkLabel",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(Label)
            },
                null
                );
            FieldInfo field21 = typeof(System.Reflection.Emit.OpCodes).GetField("Ldarg_0", BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo field22 = typeof(System.Reflection.Emit.OpCodes).GetField("Callvirt", BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo field23 = typeof(System.Reflection.Emit.OpCodes).GetField("Ret", BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo method24 = typeof(DynamicMethod).GetMethod(
                "CreateDelegate",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(Type)
            },
                null
                );
            MethodInfo method25 = typeof(System.Func<>).MakeGenericType(typeof(String), typeof(String)).GetMethod(
                "Invoke",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(string)
            },
                null
                );
            MethodInfo method26 = typeof(Console).GetMethod(
                "WriteLine",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
            typeof(String),
            typeof(Object)
            },
                null
                );

            ILGenerator gen = default(ILGenerator);// rmethod.GetILGenerator();
            var gen2 = newMethod.Body.GetILProcessor();
            // Preparing locals
            LocalBuilder CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Method = gen.DeclareLocal(typeof(DynamicMethod));
            LocalBuilder field1 = gen.DeclareLocal(typeof(FieldInfo));
            LocalBuilder method2 = gen.DeclareLocal(typeof(MethodInfo));
            LocalBuilder ctor3 = gen.DeclareLocal(typeof(ConstructorInfo));
            LocalBuilder method4_ = gen.DeclareLocal(typeof(MethodInfo));
            LocalBuilder gen_ = gen.DeclareLocal(typeof(ILGenerator));
            LocalBuilder label27 = gen.DeclareLocal(typeof(Label));
            LocalBuilder CS_d__lt__rt_9__CachedAnonymousMethodDelegate1Invoker = gen.DeclareLocal(typeof(System.Func<>).MakeGenericType(typeof(String), typeof(String)));
            LocalBuilder CS_0_0000 = gen.DeclareLocal(typeof(Type[]));
            LocalBuilder CS_0_0001 = gen.DeclareLocal(typeof(Type[]));
            LocalBuilder CS_0_0002 = gen.DeclareLocal(typeof(Type[]));
            LocalBuilder CS_0_0003 = gen.DeclareLocal(typeof(Type[]));
            LocalBuilder CS_0_0004 = gen.DeclareLocal(typeof(Type[]));
            LocalBuilder CS_0_0005 = gen.DeclareLocal(typeof(Type[]));
            // Writing body
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "CS_<>9__CachedAnonymousMethodDelegate1Method");
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 8);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 8);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 8);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Newobj, ctor2);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(int));  // Piyo
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "CS_<>9__CachedAnonymousMethodDelegate1");
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_S, 56);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method3);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(int));   // Piyo
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "<Print>b__0");
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_S, 56);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 9);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 9);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 9);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method4);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_2);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(Func<T, TResult>));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_2);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 10);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_S, 52);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_2);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 11);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 11);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(Object));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 11);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(IntPtr));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 11);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method6);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_3);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(Func<T, TResult>));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_2);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 12);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 12);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 12);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 12);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "Invoke");
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_S, 52);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(Type));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 13);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 13);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(String));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Stelem_Ref);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 13);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldnull);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method4);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 4);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method7);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method8);
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 6);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field9);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field11);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 6);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method12);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field13);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method14);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field15);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_2);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method16);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field17);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_3);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method18);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field19);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 6);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method20);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field9);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_1);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method10);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field21);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method14);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field22);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 4);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method16);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 5);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldsfld, field23);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method14);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, typeof(Func<String, String>));
            gen2.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method24);
            gen.Emit(System.Reflection.Emit.OpCodes.Castclass, typeof(Func<String, String>));
            gen.Emit(System.Reflection.Emit.OpCodes.Stloc_S, 7);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "Result2: {0}");
            gen.Emit(System.Reflection.Emit.OpCodes.Ldloc_S, 7);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldstr, "sasisuseso");
            gen.Emit(System.Reflection.Emit.OpCodes.Callvirt, method25);
            gen.Emit(System.Reflection.Emit.OpCodes.Call, method26);
            gen.Emit(System.Reflection.Emit.OpCodes.Ret);
            */
#endregion

            #region 古い1
            /*
            var methodModule = ModuleDefinition.ReadModule(new Uri(method.Method.DeclaringType.Assembly.CodeBase).LocalPath);
            var methodType = methodModule.GetType(method.Method.DeclaringType.FullName);
            var methodField = new FieldDefinition("new" + this.method.Name, Mono.Cecil.FieldAttributes.Private, tbaseModule.Import(methodType));
            tbaseType.Fields.Add(methodField);
            var delegateType = new TypeDefinition(methodType.Namespace, "DynamicNewPrint", 
                Mono.Cecil.TypeAttributes.NestedPrivate, typeof(MulticastDelegate).ToTypeDef());
            methodType.NestedTypes.Add(delegateType);
            Mono.Cecil.MethodAttributes methodAttributes = 
                  Mono.Cecil.MethodAttributes.Public
                | Mono.Cecil.MethodAttributes.Virtual
                | Mono.Cecil.MethodAttributes.HideBySig
                | Mono.Cecil.MethodAttributes.NewSlot;
            var delegateTypeInvoke = new MethodDefinition("Invoke", methodAttributes, typeof(string).ToTypeDef());
            var delegateTypeInvokeValue = new ParameterDefinition("value", Mono.Cecil.ParameterAttributes.None, typeof(string).ToTypeDef());
            delegateTypeInvoke.Parameters.Add(delegateTypeInvokeValue);
            delegateType.Methods.Add(delegateTypeInvoke);

            var callingMethod = methodType.Methods.FirstOrDefault(_method => _method.Name == method.Method.Name);
            
            var newMethod = new MethodDefinition(this.method.Name, this.method.Attributes, this.method.ReturnType);

            var value = new ParameterDefinition("value", this.method.Parameters[0].Attributes, this.method.Parameters[0].ParameterType);
            newMethod.Parameters.Add(value);


            var method1 = 
                typeof(Type).ToTypeDef().GetMethod(
                    "GetTypeFromHandle",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(RuntimeTypeHandle)
                    }
                );
            var method2 = 
                typeof(Type).ToTypeDef().GetMethod(
                    "GetField",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(String),
                        typeof(BindingFlags)
                    }
                );
            var method3 = 
                typeof(Type).ToTypeDef().GetMethod(
                    "MakeGenericType",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(Type[])
                    }
                );
            var method4 = 
                typeof(Type).ToTypeDef().GetMethod(
                    "GetMethod",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(String),
                        typeof(BindingFlags),
                        typeof(Binder),
                        typeof(Type[]),
                        typeof(ParameterModifier[])
                    }
                );
            var ctor5 = 
                typeof(DynamicMethod).ToTypeDef().GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(String),
                        typeof(Type),
                        typeof(Type[])
                    }
                );
            var method6 = 
                typeof(DynamicMethod).ToTypeDef().GetMethod(
                    "GetILGenerator",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                    }
                );
            var field7 = typeof(System.Reflection.Emit.OpCodes).ToTypeDef().GetField("Ldarg_0", BindingFlags.Public | BindingFlags.NonPublic);
            var method8 = 
                typeof(ILGenerator).ToTypeDef().GetMethod(
                    "Emit",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(System.Reflection.Emit.OpCode)
                    }
                );
            var field9 = typeof(System.Reflection.Emit.OpCodes).ToTypeDef().GetField("Ldfld", BindingFlags.Public | BindingFlags.NonPublic);
            var ldfldMethod = 
                typeof(ILGenerator).ToTypeDef().GetMethod(
                    "Emit",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[]{
                        typeof(System.Reflection.Emit.OpCode),
                        typeof(FieldInfo)
                    }
                );
            var field11 = typeof(System.Reflection.Emit.OpCodes).ToTypeDef().GetField("Ldarg_1", BindingFlags.Public | BindingFlags.NonPublic);
            var field12 = typeof(System.Reflection.Emit.OpCodes).ToTypeDef().GetField("Callvirt", BindingFlags.Public | BindingFlags.NonPublic);
            var callvirtMethod = 
                typeof(ILGenerator).ToTypeDef().GetMethod(
                    "Emit",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[]{
                        typeof(System.Reflection.Emit.OpCode),
                        typeof(MethodInfo)
                    }
                );
            var field14 = typeof(System.Reflection.Emit.OpCodes).ToTypeDef().GetField("Ret", BindingFlags.Public | BindingFlags.NonPublic);
            var method15 = 
                typeof(DynamicMethod).ToTypeDef().GetMethod(
                    "CreateDelegate",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(Type)
                    }
                );
            var method16 = 
                delegateType.GetMethod(
                    "Invoke",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new Type[] {
                        typeof(String)
                    }
                );





            var field1 = new VariableDefinition("field1", typeof(System.Reflection.FieldInfo).ToTypeDef());
            newMethod.Body.Variables.Add(field1);
            var _method2 = new VariableDefinition("method2", typeof(System.Reflection.FieldInfo).ToTypeDef());
            newMethod.Body.Variables.Add(_method2);
            var dynamicMethod = new VariableDefinition("dynamicMethod", typeof(System.Reflection.Emit.DynamicMethod).ToTypeDef());
            newMethod.Body.Variables.Add(dynamicMethod);
            var ilGenerator = new VariableDefinition("ilGenerator", typeof(System.Reflection.Emit.ILGenerator).ToTypeDef());
            newMethod.Body.Variables.Add(ilGenerator);
            var dynamicNewPrint = new VariableDefinition("dynamicNewPrint", delegateType);
            newMethod.Body.Variables.Add(dynamicNewPrint);
            var CS_0_0000 =  new VariableDefinition("CS_0_0000", typeof(Type[]).ToTypeDef());
            newMethod.Body.Variables.Add(CS_0_0000);
            var CS_0_0001 = new VariableDefinition("CS_0_0001", typeof(Type[]).ToTypeDef());
            newMethod.Body.Variables.Add(CS_0_0001);
            var CS_0_0002 = new VariableDefinition("CS_0_0002", typeof(Type[]).ToTypeDef());
            newMethod.Body.Variables.Add(CS_0_0002);

            
            var ilProcessor = newMethod.Body.GetILProcessor();
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, methodType);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, methodField.Name);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.NonPublic | BindingFlags.Public));
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method2);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_0);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(Func<>).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Newarr, typeof(Type).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, CS_0_0000);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0000);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(String).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0000);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(String).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0000);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, "Invoke");
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Newarr, typeof(Type).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, CS_0_0001);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0001);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(String).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0001);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method4);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, "_newPrint");
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(String).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Newarr, typeof(Type).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, CS_0_0002);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0002);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, typeof(String).ToTypeDef());
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, CS_0_0002);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Newobj, ctor5);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_2);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_2);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method6);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, field7);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method8);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, field9);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_0);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, ldfldMethod);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, field11);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method8);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, field12);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, callvirtMethod);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_3);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, field14);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method8);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_2);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, delegateType);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Call, method1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method15);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Castclass, delegateType);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, dynamicNewPrint);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, dynamicNewPrint);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, method16);
            ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);



            //ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            //ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, methodField);
            //ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);
            //ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(callingMethod));
            //ilProcessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);

            
            
            
            
            
            
            
            
            
            
            
            
            
            
            this.method.Name = "__" + this.method.Name;
            tbaseType.Methods.Add(newMethod);

            return null;
             */
            #endregion

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

    // HACK: このクラスもどちらかというと CRUtilities 系。
    public static class TypeAnalyzer
    {
        public static bool IsAnonymous(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(false).OfType<CompilerGeneratedAttribute>().FirstOrDefault() != null &&
                methodInfo.Name.IndexOf("<") == 0;
        }

        public static bool IsCandidateAnonymousMethodCache(FieldDefinition field)
        {
            return -1 < field.Name.IndexOf("__CachedAnonymousMethodDelegate") &&
                field.CustomAttributes.FirstOrDefault(customAttribute =>
                                                    customAttribute.AttributeType.Equivalent(typeof(CompilerGeneratedAttribute))) != null;
        }

        public static FieldDefinition GetCacheFieldIfAnonymous(MethodInfo methodInfo)
        {
            if (!IsAnonymous(methodInfo)) return null;


            var cacheField = default(FieldDefinition);
            var declaringTypeDef = methodInfo.DeclaringType.ToTypeDef();

            var candidateNameCacheFieldDictionary = new Dictionary<string, FieldDefinition>();
            foreach (var candidateCacheField in declaringTypeDef.Fields.Where(field => IsCandidateAnonymousMethodCache(field)))
            {
                candidateNameCacheFieldDictionary.Add(candidateCacheField.Name, candidateCacheField);
            }


            string declaringMethodName = methodInfo.Name.Substring(methodInfo.Name.IndexOf("<") + 1, methodInfo.Name.IndexOf(">") - 1);
            foreach (var candidateMethod in declaringTypeDef.Methods.Where(method => -1 < method.Name.IndexOf(declaringMethodName)))
            {
                int candidatePoint = 0; // HACK: enum 化
                var candidateCacheField = default(FieldDefinition);
                foreach (var instruction in candidateMethod.Body.Instructions)
                {
                    if (candidatePoint == 0 &&
                        instruction.OpCode == Mono.Cecil.Cil.OpCodes.Ldsfld &&
                        candidateNameCacheFieldDictionary.ContainsKey((candidateCacheField = (FieldDefinition)instruction.Operand).Name))
                    {
                        candidatePoint = 1;
                    }
                    else if (candidatePoint == 1 &&
                        instruction.OpCode == Mono.Cecil.Cil.OpCodes.Brtrue_S)
                    {
                        candidatePoint = 2;
                    }
                    else if (candidatePoint == 2 &&
                        instruction.OpCode == Mono.Cecil.Cil.OpCodes.Ldnull)
                    {
                        candidatePoint = 3;
                    }
                    else if (candidatePoint == 3 &&
                        instruction.OpCode == Mono.Cecil.Cil.OpCodes.Ldftn &&
                        ((MethodDefinition)instruction.Operand).Equivalent(methodInfo))
                    {
                        candidatePoint = 4;
                        break;
                    }
                    else if (candidatePoint == 3 || candidatePoint == 2 || candidatePoint == 1)
                    {
                        candidatePoint = 0;
                    }
                }
                if (candidatePoint == 4)
                {
                    cacheField = candidateCacheField;
                    break;
                }
            }
            return cacheField;
        }
    }


    // HACK: このクラスもどちらかというと CRUtilities 系。
    // HACK: もう少し一般化が必要？I/F を同じにするとか、意識して作成するローカル変数以外（現状 tmp で指定してるやつとか）を隠蔽するとか。
    public class MethodBuilder<T, TResult>
    {
        readonly ModuleDefinition tbaseModule;

        public MethodBuilder(ModuleDefinition tbaseModule)
        {
            this.tbaseModule = tbaseModule;
        }

        public void PushNewDynamicMethod(ILProcessor gen, string name, VariableDefinition tmp)
        {
            Required.Assert(tmp, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp);

            // HACK: TResult や T を使う以外の部分は一般化できるはず（他の Func<TResult>, Action<T>, とかへの対策）
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, name);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newobj, tbaseModule.Import(References.DynamicMethod.DynamicMethodConstructor));
        }

        // HACK: 型指定 + GetConstructor で分けられそう。
        public void PushFuncGetConstructor(ILProcessor gen, VariableDefinition tmp1, VariableDefinition tmp2)
        {
            Required.Assert(tmp1, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp1);
            Required.Assert(tmp2, _ => _.VariableType.Equivalent(typeof(Type[])), () => tmp2);

            // HACK: TResult や T を使う以外の部分は一般化できるはず（他の Func<TResult>, Action<T>, とかへの対策）
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Func<,>)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.MakeGenericType));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Object)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(IntPtr)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.GetConstructor));
        }

        // HACK: 型指定 + GetMethod で分けられそう。
        public void PushFuncInvoke(ILProcessor gen, VariableDefinition tmp1, VariableDefinition tmp2)
        {
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(Func<,>)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(T)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(TResult)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.MakeGenericType));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldstr, "Invoke");
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_1);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Newarr, tbaseModule.Import(typeof(Type)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_0);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldtoken, tbaseModule.Import(typeof(String)));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Call, tbaseModule.Import(References.Type.GetTypeFromHandle));
            gen.Emit(Mono.Cecil.Cil.OpCodes.Stelem_Ref);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, tmp2);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Ldnull);
            gen.Emit(Mono.Cecil.Cil.OpCodes.Callvirt, tbaseModule.Import(References.Type.GetMethod));
        }
    }


    // HACK: このクラスもどちらかというと CRUtilities 系。
    public static class References
    {
        public static class Type
        {
            public static readonly MethodInfo GetTypeFromHandle = 
                typeof(System.Type).GetMethod(
                    "GetTypeFromHandle",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    {
                        typeof(RuntimeTypeHandle) 
                    },
                    null);

            public static readonly MethodInfo MakeGenericType = 
                typeof(System.Type).GetMethod(
                    "MakeGenericType",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    { 
                        typeof(System.Type[]) 
                    },
                    null);

            public static readonly MethodInfo GetConstructor =
                typeof(System.Type).GetMethod(
                    "GetConstructor",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    { 
                        typeof(BindingFlags), 
                        typeof(Binder), 
                        typeof(System.Type[]), 
                        typeof(ParameterModifier[]) 
                    },
                    null);

            public static readonly MethodInfo GetMethod =
                typeof(System.Type).GetMethod(
                    "GetMethod",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    { 
                        typeof(String), 
                        typeof(BindingFlags), 
                        typeof(Binder), 
                        typeof(System.Type[]), 
                        typeof(ParameterModifier[]) 
                    },
                    null);
        }

        public static class DynamicMethod
        {
            public static readonly ConstructorInfo DynamicMethodConstructor = 
                typeof(System.Reflection.Emit.DynamicMethod).GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new System.Type[] 
                    { 
                        typeof(String), 
                        typeof(System.Type), 
                        typeof(System.Type[]), 
                        typeof(Boolean) 
                    },
                    null);

            public static readonly MethodInfo GetILGenerator =
                typeof(System.Reflection.Emit.DynamicMethod).GetMethod(
                    "GetILGenerator", 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                    null, 
                    new System.Type[] { }, 
                    null);
        }

        public static class ILGenerator
        {
            public static readonly MethodInfo DefineLabel = 
                typeof(System.Reflection.Emit.ILGenerator).GetMethod(
                    "DefineLabel", 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                    null, 
                    new System.Type[] { }, 
                    null);
        }

        public static class OpCodes
        {

        }
    }

















    class Hoge
    {
        static void Main()
        {
            var hogeDef = typeof(Hoge).ToTypeDef();

            var testDef = new MethodDefinition("Test", 
                Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.Static, hogeDef.Module.Import(typeof(void)));
            hogeDef.Methods.Add(testDef);

            // もっと直接的な指定（LambdaExpression から、とか）で行けるんじゃない？
            // 例えば、↓みたいに。
            var egen = new ExpressiveILProcessor(testDef);
            egen.Emit(_ => Console.WriteLine("aiueo"));

            // 現状の式木だと、変数の宣言ができません。
            // MEMO: 初めて代入が行われた際に変数を宣言してしまう。Javascript 方式もいいかも？
            // MEMO: ただし、通常手順的に、「1. 一通り書く」「2. Excel に貼り付け」「3. 一括変換」みたいにやりたいから、Addlocがやはり必要。
            // → こんな感じで騙し騙し
            int a = 0;
            egen.Emit(_ => _.Addloc(a, default(int)));
            egen.Emit(_ => _.Stloc(a, 100));
            //egen.Emit(_ => _.Addloc(() => a, default(int)));
            //egen.Emit(_ => _.Stloc(() => a, 100));

            // ローカル変数に入れた情報にアクセスしたい場合はどうするのだ。
            // → こんな感じで騙し騙し
            var cachedAnonymousMethod = default(DynamicMethod);
            var gen = default(ILGenerator);
            var label27 = default(Label);
            egen.Emit(_ => _.Addloc(cachedAnonymousMethod, new DynamicMethod("cachedAnonymousMethod", typeof(string), new Type[] { typeof(string) }, true)));
            egen.Emit(_ => _.Addloc(gen, _.Ldloc(cachedAnonymousMethod).GetILGenerator()));
            egen.Emit(_ => _.Addloc(label27, _.Ldloc(gen).DefineLabel()));
            egen.Emit(_ => _.Ldloc(gen).Emit(System.Reflection.Emit.OpCodes.Brtrue_S, _.Ldloc(label27)));

            // 通常の Emit 処理混合できるよね、当然？
            // → こんな感じで騙し騙し
            egen.Emit(_ => _.Direct.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4_S, (sbyte)100));
            egen.Emit(_ => _.Direct.Emit(Mono.Cecil.Cil.OpCodes.Stloc, _.Locals(() => a)));
        }
    }


}
