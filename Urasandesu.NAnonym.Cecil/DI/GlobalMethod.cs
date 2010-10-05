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
    public abstract class GlobalMethod
    {
        readonly GlobalClass globalClass;
        readonly MethodInfo oldMethod;

        public GlobalMethod(GlobalClass globalClass, MethodInfo oldMethod)
        {
            this.globalClass = globalClass;
            this.oldMethod = oldMethod;
        }

        public GlobalClass IsReplacedBy(Delegate @delegate)
        {
            globalClass.TargetInfoSet.Add(new TargetInfo(SetupMode.Replace, oldMethod, @delegate.Method, @delegate.GetType()));
            return globalClass;
        }
    }

    public class GlobalFunc<TBase, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<TResult> newFunc)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newFunc);
        }
    }
    
    public class GlobalFunc<TBase, T, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T, TResult> newFunc)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newFunc);
        }
    }

    public class GlobalFunc<TBase, T1, T2, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, TResult> newFunc)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newFunc);
        }
    }

    public class GlobalFunc<TBase, T1, T2, T3, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, TResult> newFunc)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newFunc);
        }
    }

    public class GlobalFunc<TBase, T1, T2, T3, T4, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, T4, TResult> newFunc)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newFunc);
        }
    }

    public class GlobalAction<TBase> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action newAction)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newAction);
        }
    }

    public class GlobalAction<TBase, T> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T> newAction)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newAction);
        }
    }

    public class GlobalAction<TBase, T1, T2> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2> newAction)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newAction);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3> newAction)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newAction);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3, T4> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo oldMethod)
            : base(globalClass, oldMethod)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3, T4> newAction)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)newAction);
        }
    }
}
