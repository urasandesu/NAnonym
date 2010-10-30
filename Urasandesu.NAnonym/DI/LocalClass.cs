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
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.ILTools.Mixins.System.Reflection.Emit;

namespace Urasandesu.NAnonym.DI
{

    public abstract class LocalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UND$<>0__Cached";
        public LocalFieldInt Field(Expression<Func<int>> reference) { return new LocalFieldInt(this, reference); }
        public LocalField<T> Field<T>(Expression<Func<T>> reference) { return new LocalField<T>(this, reference); }
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
                    gen.Eval(_ => _.If(_.Ld(_.X(cachedConstructBuilder.Name)) == null));
                    {
                        var dynamicMethod = default(DynamicMethod);
                        gen.Eval(_ => _.St(dynamicMethod).As(new DynamicMethod(
                                                                    "dynamicMethod",
                                                                    typeof(void),
                                                                    new Type[] { _.X(localClassTypeBuilder) },
                                                                    _.X(localClassTypeBuilder),
                                                                    true)));
                        var il = default(ILGenerator);
                        gen.Eval(_ => _.St(il).As(dynamicMethod.GetILGenerator()));
                        foreach (var targetFieldInfo in TargetFieldInfoSet)
                        {
                            var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                            if (!targetFieldDeclaringTypeUsedDictionary[targetField.DeclaringType])
                            {
                                targetFieldDeclaringTypeUsedDictionary[targetField.DeclaringType] = true;

                                var targetFieldDeclaringTypeConstructor = default(ConstructorInfo);
                                gen.Eval(_ => _.St(targetFieldDeclaringTypeConstructor).As(
                                                       _.X(targetField.DeclaringType).GetConstructor(
                                                                                BindingFlags.Public | BindingFlags.Instance,
                                                                                null,
                                                                                Type.EmptyTypes,
                                                                                null)));

                                gen.Eval(_ => _.St(_.X(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name)).As(
                                                       _.X(localClassTypeBuilder).GetField(
                                                                                _.X(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name),
                                                                                BindingFlags.Instance | BindingFlags.NonPublic)));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, targetFieldDeclaringTypeConstructor));
                                gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, _.Ld<FieldInfo>(_.X(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name))));
                            }

                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                            gen.Eval(_ => il.Emit(SRE::OpCodes.Ldfld, _.Ld<FieldInfo>(_.X(targetFieldDeclaringTypeDictionary[targetField.DeclaringType].Name))));
                            var targetFieldActual = default(FieldInfo);
                            gen.Eval(_ => _.St(targetFieldActual).As(
                                                   _.X(targetField.DeclaringType).GetField(
                                                                                _.X(targetField.Name),
                                                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)));

                            var macro = new ExpressiveMethodBodyGeneratorMacro(gen);
                            macro.EvalEmitDirectives(TypeSavable.GetName(() => il), gen.ToDirectives(targetFieldInfo.Expression));

                            gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, targetFieldActual));
                        }
                        gen.Eval(_ => il.Emit(SRE::OpCodes.Ret));
                        gen.Eval(_ => _.St(_.X(cachedConstructBuilder.Name)).As(dynamicMethod.CreateDelegate(typeof(Action), _.This())));
                    }
                    gen.Eval(_ => _.EndIf());
                    gen.Eval(_ => _.Ld<Action>(_.X(cachedConstructBuilder.Name)).Invoke());
                    gen.Eval(_ => _.Base());
                },
                new FieldBuilder[] { cachedConstructBuilder }.Concat(targetFieldDeclaringTypeDictionary.Values).ToArray());
            }



            var methodInjection = new LocalMethodInjection(localClassTypeBuilder, targetFieldDeclaringTypeDictionary);
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                methodInjection.Apply(targetMethodInfo);
            }

            createdType = localClassTypeBuilder.CreateType();
            localClassAssemblyBuilder.Save("LocalClasses.dll");
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}
