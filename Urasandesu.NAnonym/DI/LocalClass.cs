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
            var localClassAssemblyName = new AssemblyName("LocalClasses");
            localClassAssemblyName.Version = new Version(1, 0, 0, 0);
            var localClassAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(localClassAssemblyName, AssemblyBuilderAccess.RunAndSave);
            var localClassModuleBuilder = localClassAssemblyBuilder.DefineDynamicModule("LocalClasses", "LocalClasses.dll");
            var localClassTypeBuilder = localClassModuleBuilder.DefineType("LocalClasses.LocalClassType");
            if (typeof(TBase).IsInterface)
            {
                localClassTypeBuilder.AddInterfaceImplementation(typeof(TBase));
            }
            else
            {
                throw new NotImplementedException();
            }

            var targetFieldDeclaringTypeDictionary = new Dictionary<Type, FieldBuilder>();
            if (0 < TargetFieldInfoSet.Count)
            {
                var cachedConstructBuilder = localClassTypeBuilder.DefineField(
                    CacheFieldPrefix + "Construct", typeof(Action), FieldAttributes.Private | FieldAttributes.Static);

                var targetFieldDeclaringTypeUsedDictionary = new Dictionary<Type, bool>();
                int targetFieldDeclaringTypeIndex = 0;
                foreach (var targetFieldInfo in TargetFieldInfoSet)
                {
                    var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                    if (!targetFieldDeclaringTypeDictionary.ContainsKey(targetField.DeclaringType))
                    {
                        var cachedTargetFieldDeclaringTypeBuilder = localClassTypeBuilder.DefineField(
                                CacheFieldPrefix + "TargetFieldDeclaringType" + targetFieldDeclaringTypeIndex++, targetField.DeclaringType, FieldAttributes.Private);
                        targetFieldDeclaringTypeDictionary.Add(targetField.DeclaringType, cachedTargetFieldDeclaringTypeBuilder);
                        targetFieldDeclaringTypeUsedDictionary.Add(targetField.DeclaringType, false);
                    }
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
                    gen.Eval(_ => _.If(_.Ldsfld(_.Extract(cachedConstructBuilder.Name, typeof(Action))) == null));
                    {
                        var dynamicMethod = default(DynamicMethod);
                        gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod(
                                                                    "dynamicMethod",
                                                                    _.Expand(typeof(void)),
                                                                    new Type[] { _.Expand(localClassTypeBuilder) },
                                                                    _.Expand(localClassTypeBuilder),
                                                                    true)));
                        var il = default(ILGenerator);
                        gen.Eval(_ => _.Addloc(il, dynamicMethod.GetILGenerator()));
                        foreach (var targetFieldInfo in TargetFieldInfoSet)
                        {
                            var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                            if (!targetFieldDeclaringTypeUsedDictionary[targetField.DeclaringType])
                            {
                                targetFieldDeclaringTypeUsedDictionary[targetField.DeclaringType] = true;

                                var targetFieldDeclaringTypeConstructor = default(ConstructorInfo);
                                gen.Eval(_ => _.Addloc(targetFieldDeclaringTypeConstructor,
                                                       _.Expand(targetField.DeclaringType).GetConstructor(
                                                                                BindingFlags.Public | BindingFlags.Instance,
                                                                                null,
                                                                                Type.EmptyTypes,
                                                                                null)));

                                gen.Eval(_ => _.Addloc(_.Extract<FieldInfo>(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name),
                                                       _.Expand(localClassTypeBuilder).GetField(
                                                                                _.Expand(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name),
                                                                                BindingFlags.Instance | BindingFlags.NonPublic)));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, targetFieldDeclaringTypeConstructor));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, _.Extract<FieldInfo>(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name)));
                            }

                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, _.Extract<FieldInfo>(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name)));
                            var targetFieldActual = default(FieldInfo);
                            gen.Eval(_ => _.Addloc(targetFieldActual,
                                                   _.Expand(targetField.DeclaringType).GetField(
                                                                                _.Expand(targetField.Name),
                                                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));

                            var macro = new ExpressiveMethodBodyGeneratorMacro(gen);
                            macro.EvalEmitDirectives(TypeSavable.GetName(() => il), gen.ToDirectives(targetFieldInfo.Expression));

                            gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, targetFieldActual));
                        }
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                        gen.Eval(_ => _.Stsfld(_.Extract<Action>(cachedConstructBuilder.Name),
                                               (Action)dynamicMethod.CreateDelegate(typeof(Action), _.This())));
                    }
                    gen.Eval(_ => _.EndIf());
                    gen.Eval(_ => _.Ldsfld(_.Extract<Action>(cachedConstructBuilder.Name)).Invoke());
                    gen.Eval(_ => _.Base());
                },
                new FieldBuilder[] { cachedConstructBuilder }.Concat(targetFieldDeclaringTypeDictionary.Values).ToArray());
            }




            int targetMethodInfoSetIndex = 0;
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                var cachedMethodBuilder =
                    localClassTypeBuilder.DefineField(
                        CacheFieldPrefix + "Method" + targetMethodInfoSetIndex++,
                        targetMethodInfo.DelegateType,
                        FieldAttributes.Private);

                var methodInjection = LocalMethodInjection.CreateInstance<TBase>(localClassTypeBuilder, targetMethodInfo);
                var cachedSettingBuilder = targetFieldDeclaringTypeDictionary.ContainsKey(targetMethodInfo.NewMethod.DeclaringType) ? 
                                                targetFieldDeclaringTypeDictionary[targetMethodInfo.NewMethod.DeclaringType] : 
                                                default(FieldBuilder);
                methodInjection.Apply(cachedSettingBuilder, cachedMethodBuilder);
            }
            createdType = localClassTypeBuilder.CreateType();
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}
