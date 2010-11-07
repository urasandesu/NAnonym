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


            var fieldsForDeclaringType = new Dictionary<Type, FieldBuilder>();

            var constructorInjection = new LocalConstructorInjection(localClassTypeBuilder, TargetFieldInfoSet, fieldsForDeclaringType);
            constructorInjection.Apply();


            // 最終的には constructorInjection を食わせる。
            var methodInjection = new LocalMethodInjection(localClassTypeBuilder, fieldsForDeclaringType);
            foreach (var targetMethodInfo in TargetMethodInfoSet)
            {
                methodInjection.Apply(targetMethodInfo);
            }

            createdType = localClassTypeBuilder.CreateType();
            //localClassAssemblyBuilder.Save("LocalClasses.dll");
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}
