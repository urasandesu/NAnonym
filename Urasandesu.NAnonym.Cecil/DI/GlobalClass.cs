using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DI;
using Urasandesu.NAnonym.Mixins.System;
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

        public GlobalFieldInt Field(Expression<Func<int>> reference) { return new GlobalFieldInt(this, reference); }
        public GlobalField<T> Field<T>(Expression<Func<T>> reference) { return new GlobalField<T>(this, reference); }
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

            var fieldsForDeclaringType = new Dictionary<Type, FieldDefinition>();

            var constructorInjection = new GlobalConstructorInjection(tbaseTypeDef, TargetFieldInfoSet, fieldsForDeclaringType);
            constructorInjection.Apply();


            var methodInjection = new GlobalMethodInjection(tbaseTypeDef);
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                methodInjection.Apply(targetMethodInfo);
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
