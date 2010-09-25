using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Urasandesu.NAnonym.ILTools;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Urasandesu.NAnonym.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SR = System.Reflection;
using MC = Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools;
using TypeAnalyzer = Urasandesu.NAnonym.Cecil.ILTools.TypeAnalyzer;
using Urasandesu.NAnonym.DI;


namespace Urasandesu.NAnonym.Cecil.DI
{
    // HACK: あれ？ where とかいらなくね？もはやなんでも良い気がしてきた。
    public class GlobalMethod<TBase, T, TResult> where TBase : class
    {
        readonly GlobalClass<TBase> globalClass;
        readonly Func<T, TResult> func;

        public GlobalMethod(GlobalClass<TBase> globalClass, Func<T, TResult> func)
        {
            this.globalClass = globalClass;
            this.func = func;
        }


        public GlobalClass<TBase> Override(Func<TBase, Func<T, TResult>> method)
        {
            throw new NotImplementedException();
        }

        public GlobalClass<TBase> Instead(Expression<Func<TBase, Func<T, TResult>>> expression)
        {
            var method = DependencyUtil.ExtractMethod(expression);
            var targetMethod = typeof(TBase).GetMethod(method);
            globalClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Instead, targetMethod, func.Method));
            return globalClass;
        }
    }
}
