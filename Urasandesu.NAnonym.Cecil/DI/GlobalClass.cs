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

namespace Urasandesu.NAnonym.Cecil.DI
{
    // GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClass : MarshalByRefObject
    {
        public static readonly string CacheFieldPrefix = "UNCD$<>0__Cached";

        internal HashSet<TargetMethodInfo> TargetMethodInfoSet { get; private set; }
        GlobalClass modified;

        public GlobalClass()
        {
            TargetMethodInfoSet = new HashSet<TargetMethodInfo>();
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

            var declaringType = TypeAnalyzer.SearchManuallyGenerated(setupper.Method.DeclaringType);

            var cachedConstructDef = new FieldDefinition(
                    CacheFieldPrefix + "Construct", MC::FieldAttributes.Private | MC::FieldAttributes.Static, tbaseModuleDef.Import(typeof(Action)));
            tbaseTypeDef.Fields.Add(cachedConstructDef);

            var cachedSettingDef = new FieldDefinition(
                    CacheFieldPrefix + "Setting", MC::FieldAttributes.Private, tbaseModuleDef.Import(typeof(GlobalClass)));
            tbaseTypeDef.Fields.Add(cachedSettingDef);

            foreach (var constructorDef in tbaseTypeDef.Methods.Where(methodDef => methodDef.Name == ".ctor"))
            {
                var firstInstruction = constructorDef.Body.Instructions[0];
                constructorDef.ExpressBodyBefore(
                gen =>
                {
                    gen.Eval(_ => _.If(_.Ldsfld(_.Extract(cachedConstructDef.Name, typeof(Action))) == null));
                    var dynamicMethod = default(DynamicMethod);
                    gen.Eval(_ => _.Addloc(dynamicMethod, new DynamicMethod(
                                                                "dynamicMethod",
                                                                _.Expand(typeof(void)),
                                                                _.Expand(new Type[] { typeof(TBase) }),
                                                                typeof(TBase),
                                                                true)));
                    var il = default(ILGenerator);
                    gen.Eval(_ => _.Addloc(il, dynamicMethod.GetILGenerator()));
                    var settingConstructor = default(ConstructorInfo);
                    gen.Eval(_ => _.Addloc(settingConstructor, _.Expand(declaringType).GetConstructor(
                                                                            BindingFlags.Public | BindingFlags.Instance,
                                                                            null,
                                                                            Type.EmptyTypes,
                                                                            null)));
                    var settingField = default(FieldInfo);
                    gen.Eval(_ => _.Addloc(settingField, _.Expand(typeof(TBase)).GetField(_.Expand(cachedSettingDef.Name), BindingFlags.Instance | BindingFlags.NonPublic)));
                    gen.Eval(_ => il.Emit(OpCodes.Ldarg_0));
                    gen.Eval(_ => il.Emit(OpCodes.Newobj, settingConstructor));
                    gen.Eval(_ => il.Emit(OpCodes.Stfld, settingField));
                    var settingRegisterMethod = default(MethodInfo);
                    gen.Eval(_ => _.Addloc(settingRegisterMethod, _.Expand(declaringType).GetMethod(
                                                                            "Register",
                                                                            BindingFlags.NonPublic | BindingFlags.Instance,
                                                                            null,
                                                                            Type.EmptyTypes,
                                                                            null)));
                    gen.Eval(_ => il.Emit(OpCodes.Ldarg_0));
                    gen.Eval(_ => il.Emit(OpCodes.Ldfld, settingField));
                    gen.Eval(_ => il.Emit(OpCodes.Callvirt, settingRegisterMethod));
                    gen.Eval(_ => il.Emit(OpCodes.Ret));
                    gen.Eval(_ => _.Stsfld(_.Extract<Action>(cachedConstructDef.Name), (Action)dynamicMethod.CreateDelegate(typeof(Action), _.This())));
                    gen.Eval(_ => _.EndIf());
                    gen.Eval(_ => _.Ldsfld(_.Extract<Action>(cachedConstructDef.Name)).Invoke());
                },
                firstInstruction);
            }


            int targetInfoSetIndex = 0;
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                var cachedMethodDef = new FieldDefinition(
                    CacheFieldPrefix + "Method" + targetInfoSetIndex++, MC::FieldAttributes.Private, tbaseModuleDef.Import(targetMethodInfo.DelegateType));
                tbaseTypeDef.Fields.Add(cachedMethodDef);

                var methodInjection = GlobalMethodInjection.CreateInstance<TBase>(tbaseTypeDef, targetMethodInfo);
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

}
