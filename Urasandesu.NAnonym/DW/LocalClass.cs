/* 
 * File: LocalClass.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

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
using Urasandesu.NAnonym.Mixins.System.Reflection.Emit;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.ILTools.Impl.System.Reflection;

namespace Urasandesu.NAnonym.DW
{
    public abstract class LocalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UND$<>0__Cached";
        public static readonly string ClassNamePrefix = "UND$<>0__LocalClass";
        public LocalFieldInt DefineField(Expression<Func<int>> methodReference) { return new LocalFieldInt(this, methodReference); }
        public LocalField<T> DefineField<T>(Expression<Func<T>> methodReference) { return new LocalField<T>(this, methodReference); }
        protected sealed override void OnLoad(DependencyClassLoadParameter parameter)
        {
            OnLoadLocal((LocalClassLoadParameter)parameter);
        }

        protected virtual void OnLoadLocal(LocalClassLoadParameter parameter)
        {
        }
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

        public LocalImplementFunc<TBase, TResult> ImplementMethod<TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementFunc<TBase, TResult>(this, source);
        }

        public LocalImplementFunc<TBase, T, TResult> ImplementMethod<T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementFunc<TBase, T, TResult>(this, source);
        }

        public LocalImplementFunc<TBase, T1, T2, TResult> ImplementMethod<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementFunc<TBase, T1, T2, TResult>(this, source);
        }

        public LocalImplementFunc<TBase, T1, T2, T3, TResult> ImplementMethod<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementFunc<TBase, T1, T2, T3, TResult>(this, source);
        }

        public LocalImplementFunc<TBase, T1, T2, T3, T4, TResult> ImplementMethod<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementFunc<TBase, T1, T2, T3, T4, TResult>(this, source);
        }

        public LocalImplementAction<TBase> ImplementMethod(Expression<ActionReference<TBase>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementAction<TBase>(this, source);
        }

        public LocalImplementAction<TBase, T> ImplementMethod<T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementAction<TBase, T>(this, source);
        }

        public LocalImplementAction<TBase, T1, T2> ImplementMethod<T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementAction<TBase, T1, T2>(this, source);
        }

        public LocalImplementAction<TBase, T1, T2, T3> ImplementMethod<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementAction<TBase, T1, T2, T3>(this, source);
        }

        public LocalImplementAction<TBase, T1, T2, T3, T4> ImplementMethod<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalImplementAction<TBase, T1, T2, T3, T4>(this, source);
        }

        public LocalOverrideFunc<TBase, TResult> OverrideMethod<TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideFunc<TBase, TResult>(this, source);
        }

        public LocalOverrideFunc<TBase, T, TResult> OverrideMethod<T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideFunc<TBase, T, TResult>(this, source);
        }

        public LocalOverrideFunc<TBase, T1, T2, TResult> OverrideMethod<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideFunc<TBase, T1, T2, TResult>(this, source);
        }

        public LocalOverrideFunc<TBase, T1, T2, T3, TResult> OverrideMethod<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideFunc<TBase, T1, T2, T3, TResult>(this, source);
        }

        public LocalOverrideFunc<TBase, T1, T2, T3, T4, TResult> OverrideMethod<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideFunc<TBase, T1, T2, T3, T4, TResult>(this, source);
        }

        public LocalOverrideAction<TBase> OverrideMethod(Expression<ActionReference<TBase>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideAction<TBase>(this, source);
        }

        public LocalOverrideAction<TBase, T> OverrideMethod<T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideAction<TBase, T>(this, source);
        }

        public LocalOverrideAction<TBase, T1, T2> OverrideMethod<T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideAction<TBase, T1, T2>(this, source);
        }

        public LocalOverrideAction<TBase, T1, T2, T3> OverrideMethod<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideAction<TBase, T1, T2, T3>(this, source);
        }

        public LocalOverrideAction<TBase, T1, T2, T3, T4> OverrideMethod<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            var method = TypeSavable.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new LocalOverrideAction<TBase, T1, T2, T3, T4>(this, source);
        }



        public LocalPropertyGet<TBase, T> Property<T>(Func<T> propertyGet)
        {
            throw new NotImplementedException();
        }

        public LocalPropertySet<TBase, T> Property<T>(Action<T> propertySet)
        {
            throw new NotImplementedException();
        }

        protected override void OnLoadLocal(LocalClassLoadParameter parameter)
        {
            var localClassTypeGen = parameter.Assembly.Module.AddType("LocalClasses." + ClassNamePrefix + parameter.IncreaseClassNameSequence());
            // TODO: 元の型がどう、というわけではなく WeaveMethodInfo が何をしようとしているか、で型の追加もしくは更新を選択する必要がある。
            if (typeof(TBase).IsInterface)
            {
                localClassTypeGen.AddInterfaceImplementation(typeof(TBase));
            }
            else
            {
                localClassTypeGen.SetParent(typeof(TBase));
            }

            var constructorWeaver = new LocalConstructorWeaver(localClassTypeGen, FieldSet);
            constructorWeaver.Apply();

            var methodWeaver = new LocalMethodWeaver(constructorWeaver, MethodSet);
            methodWeaver.Apply();

            createdType = ((SRTypeGeneratorImpl)localClassTypeGen).Source.CreateType();
        }

        

        public TBase New()
        {
            return (TBase)createdType.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }
}

