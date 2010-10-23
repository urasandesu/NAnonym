using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.ILTools.Mixins.System;
using MC = Mono.Cecil;
using System.Reflection.Emit;
using SRE = System.Reflection.Emit;
using UND = Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DI
{
    // GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UNCD$<>0__Cached";
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

        protected override DependencyClass OnRegister()
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




        protected override void OnLoad(DependencyClass modified)
        {
            // MEMO: ここで modified に来るのは、OnSetup() の戻り値なので、ここでは特に使う必要はない。
            var tbaseModuleDef = ModuleDefinition.ReadModule(new Uri(typeof(TBase).Assembly.CodeBase).LocalPath, new ReaderParameters() { ReadSymbols = true });
            var tbaseTypeDef = tbaseModuleDef.GetType(typeof(TBase).FullName);

            if (0 < TargetFieldInfoSet.Count)
            {
                var cachedConstructDef = new FieldDefinition(
                        CacheFieldPrefix + "Construct", MC::FieldAttributes.Private | MC::FieldAttributes.Static, tbaseModuleDef.Import(typeof(Action)));
                tbaseTypeDef.Fields.Add(cachedConstructDef);

                foreach (var constructorDef in tbaseTypeDef.Methods.Where(methodDef => methodDef.Name == ".ctor"))
                {
                    var firstInstruction = constructorDef.Body.Instructions[0];
                    constructorDef.ExpressBodyBefore(
                    gen =>
                    {
                        gen.Eval(_ => _.If(_.Ldsfld(_.Extract(cachedConstructDef.Name, typeof(Action))) == null));
                        {
                            var dynamicMethod = default(DynamicMethod);
                            gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod(
                                                                        "dynamicMethod",
                                                                        _.Expand(typeof(void)),
                                                                        _.Expand(new Type[] { typeof(TBase) }),
                                                                        typeof(TBase),
                                                                        true)));
                            var il = default(ILGenerator);
                            gen.Eval(_ => _.Addloc(il, dynamicMethod.GetILGenerator()));
                            var targetFieldDeclaringTypeDictionary = new Dictionary<Type, FieldDefinition>();
                            int targetFieldDeclaringTypeIndex = 0;
                            foreach (var targetFieldInfo in TargetFieldInfoSet)
                            {
                                var targetField = TypeSavable.GetFieldInfo(targetFieldInfo.Reference);
                                if (!targetFieldDeclaringTypeDictionary.ContainsKey(targetField.DeclaringType))
                                {
                                    var cachedTargetFieldDeclaringTypeDef = new FieldDefinition(
                                            CacheFieldPrefix + "TargetFieldDeclaringType" + targetFieldDeclaringTypeIndex++,
                                            MC::FieldAttributes.Private, tbaseModuleDef.Import(targetField.DeclaringType));
                                    tbaseTypeDef.Fields.Add(cachedTargetFieldDeclaringTypeDef);
                                    targetFieldDeclaringTypeDictionary.Add(targetField.DeclaringType, cachedTargetFieldDeclaringTypeDef);

                                    var targetFieldDeclaringTypeConstructor = default(ConstructorInfo);
                                    gen.Eval(_ => _.Addloc(targetFieldDeclaringTypeConstructor,
                                                           _.Expand(targetField.DeclaringType).GetConstructor(
                                                                                    BindingFlags.Public | BindingFlags.Instance,
                                                                                    null,
                                                                                    Type.EmptyTypes,
                                                                                    null)));

                                    gen.Eval(_ => _.Addloc(_.Extract<FieldInfo>(cachedTargetFieldDeclaringTypeDef.Name),
                                                           _.Expand(typeof(TBase)).GetField(
                                                                                    _.Expand(cachedTargetFieldDeclaringTypeDef.Name),
                                                                                    BindingFlags.Instance | BindingFlags.NonPublic)));
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Ldarg_0));
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Newobj, targetFieldDeclaringTypeConstructor));
                                    gen.Eval(_ => il.Emit(SRE::OpCodes.Stfld, _.Extract<FieldInfo>(cachedTargetFieldDeclaringTypeDef.Name)));
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
                            gen.Eval(_ => _.Stsfld(_.Extract<Action>(cachedConstructDef.Name),
                                                   (Action)dynamicMethod.CreateDelegate(typeof(Action), _.This())));
                        }
                        gen.Eval(_ => _.EndIf());
                        gen.Eval(_ => _.Ldsfld(_.Extract<Action>(cachedConstructDef.Name)).Invoke());
                    },
                    firstInstruction);
                }
            }


            int targetMethodInfoSetIndex = 0;
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                var cachedMethodDef = new FieldDefinition(
                                            CacheFieldPrefix + "Method" + targetMethodInfoSetIndex++, 
                                            MC::FieldAttributes.Private, 
                                            tbaseModuleDef.Import(targetMethodInfo.DelegateType));
                tbaseTypeDef.Fields.Add(cachedMethodDef);

                var methodInjection = GlobalMethodInjection.CreateInstance<TBase>(tbaseTypeDef, targetMethodInfo);
                var cachedSettingDef = tbaseTypeDef.Fields.FirstOrDefault(
                    field => field.FieldType.Resolve().FullName.Replace("/", "+") == targetMethodInfo.NewMethod.DeclaringType.FullName);
                methodInjection.Apply(cachedSettingDef, cachedMethodDef);
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

    public class SetupModes : UND::SetupModes
    {
        protected SetupModes() : base() { }

        public static readonly SetupMode Replace = new SetupMode();
        public static readonly SetupMode Before = new SetupMode();
        public static readonly SetupMode After = new SetupMode();
    }
}
