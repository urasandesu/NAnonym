/* 
 * File: GlobalClass.cs
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
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Mixins.System;
using MC = Mono.Cecil;
using System.Reflection.Emit;
using SRE = System.Reflection.Emit;
using UND = Urasandesu.NAnonym.DW;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.Mixins.Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.DW
{
    // GlobalClass が Generic Type 持っちゃうから、共通で引き回せるような口用。
    public abstract class GlobalClass : DependencyClass
    {
        public static readonly string CacheFieldPrefix = "UNCD$<>0__Cached";
        protected internal abstract string CodeBase { get; }
        protected internal abstract string Location { get; }

        public GlobalFieldInt Field(Expression<Func<int>> methodReference) { return new GlobalFieldInt(this, methodReference); }
        public GlobalField<T> Field<T>(Expression<Func<T>> methodReference) { return new GlobalField<T>(this, methodReference); }
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

        public GlobalFunc<TBase, TResult> Method<TResult>(Expression<FuncReference<TBase, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase,TResult>(this, source);
        }

        public GlobalFunc<TBase, T, TResult> Method<T, TResult>(Expression<FuncReference<TBase, T, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T, TResult>(this, source);
        }

        public GlobalFunc<TBase, T1, T2, TResult> Method<T1, T2, TResult>(Expression<FuncReference<TBase, T1, T2, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, TResult>(this, source);
        }

        public GlobalFunc<TBase, T1, T2, T3, TResult> Method<T1, T2, T3, TResult>(Expression<FuncReference<TBase, T1, T2, T3, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, T3, TResult>(this, source);
        }

        public GlobalFunc<TBase, T1, T2, T3, T4, TResult> Method<T1, T2, T3, T4, TResult>(Expression<FuncReference<TBase, T1, T2, T3, T4, TResult>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalFunc<TBase, T1, T2, T3, T4, TResult>(this, source);
        }

        public GlobalAction<TBase> Method(Expression<ActionReference<TBase>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase>(this, source);
        }

        public GlobalAction<TBase, T> Method<T>(Expression<ActionReference<TBase, T>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T>(this, source);
        }

        public GlobalAction<TBase, T1, T2> Method<T1, T2>(Expression<ActionReference<TBase, T1, T2>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2>(this, source);
        }

        public GlobalAction<TBase, T1, T2, T3> Method<T1, T2, T3>(Expression<ActionReference<TBase, T1, T2, T3>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2, T3>(this, source);
        }

        public GlobalAction<TBase, T1, T2, T3, T4> Method<T1, T2, T3, T4>(Expression<ActionReference<TBase, T1, T2, T3, T4>> methodReference)
        {
            var method = DependencyUtil.ExtractMethod(methodReference);
            var source = typeof(TBase).GetMethod(method);
            return new GlobalAction<TBase, T1, T2, T3, T4>(this, source);
        }




        protected override void OnLoad(DependencyClass modified)
        {
            // MEMO: ここで modified に来るのは、OnSetup() の戻り値なので、ここでは特に使う必要はない。
            var localPath = new Uri(typeof(TBase).Assembly.CodeBase).LocalPath;

            var globalClassModuleDef = ModuleDefinition.ReadModule(localPath, new ReaderParameters() { ReadSymbols = true });
            var globalClassTypeGen = globalClassModuleDef.ReadType(typeof(TBase).FullName);

            var constructorWeaver = new GlobalConstructorWeaver(globalClassTypeGen, FieldSet);
            constructorWeaver.Apply();

            var methodWeaver = new GlobalMethodWeaver(constructorWeaver, MethodSet);
            methodWeaver.Apply();

            globalClassModuleDef.Write(localPath, new WriterParameters() { WriteSymbols = true });
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

