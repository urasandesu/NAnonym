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
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.ILTools.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.DI
{

    public abstract class LocalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UND$<>0__Cached";
        public LocalFieldInt Field(Expression<Func<int>> methodReference) { return new LocalFieldInt(this, methodReference); }
        public LocalField<T> Field<T>(Expression<Func<T>> methodReference) { return new LocalField<T>(this, methodReference); }
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

        public LocalFunc<TBase, TResult> Method<TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, TResult>(this, source);
        }

        public LocalFunc<TBase, T, TResult> Method<T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T, TResult>(this, source);
        }

        public LocalFunc<TBase, T1, T2, TResult> Method<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, TResult>(this, source);
        }

        public LocalFunc<TBase, T1, T2, T3, TResult> Method<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, T3, TResult>(this, source);
        }

        public LocalFunc<TBase, T1, T2, T3, T4, TResult> Method<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalFunc<TBase, T1, T2, T3, T4, TResult>(this, source);
        }

        public LocalAction<TBase> Method(Expression<ActionReference<TBase>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase>(this, source);
        }

        public LocalAction<TBase, T> Method<T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T>(this, source);
        }

        public LocalAction<TBase, T1, T2> Method<T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2>(this, source);
        }

        public LocalAction<TBase, T1, T2, T3> Method<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2, T3>(this, source);
        }

        public LocalAction<TBase, T1, T2, T3, T4> Method<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalAction<TBase, T1, T2, T3, T4>(this, source);
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
            var localClassTypeGen = localClassModuleBuilder.AddType("LocalClasses.LocalClassType");
            if (typeof(TBase).IsInterface)
            {
                localClassTypeGen.AddInterfaceImplementation(typeof(TBase));
            }
            else
            {
                localClassTypeGen.SetParent(typeof(TBase));
            }
            


            var constructorInjection = new LocalConstructorInjection(localClassTypeGen, FieldSet);
            constructorInjection.Apply();

            var methodInjection = new LocalMethodInjection(constructorInjection, MethodSet);
            methodInjection.Apply();

            createdType = ((SRTypeGeneratorImpl)localClassTypeGen).Source.CreateType();
            //localClassAssemblyBuilder.Save("LocalClasses.dll");
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}
