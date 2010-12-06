/* 
 * File: GlobalMethod.cs
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
using System.Reflection;
using Urasandesu.NAnonym.DW;

namespace Urasandesu.NAnonym.Cecil.DW
{
    public abstract class GlobalMethod
    {
        readonly GlobalClass globalClass;
        readonly MethodInfo source;

        public GlobalMethod(GlobalClass globalClass, MethodInfo source)
        {
            this.globalClass = globalClass;
            this.source = source;
        }

        //public GlobalClass IsOverridedBy(Delegate @delegate)
        //{
        //    globalClass.MethodSet.Add(new WeaveMethodInfo(SetupModes.Override, source, @delegate.Method, @delegate.GetType()));
        //    return globalClass;
        //}

        public GlobalClass IsReplacedBy(Delegate @delegate)
        {
            globalClass.MethodSet.Add(new WeaveMethodInfo(GlobalSetupModes.Replace, source, @delegate.Method, @delegate.GetType()));
            return globalClass;
        }
    }

    public class GlobalFunc<TBase, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }
    
    public class GlobalFunc<TBase, T, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalFunc<TBase, T1, T2, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }

        public GlobalClass<TBase> IsReplacedBy(FuncWithBase<T1, T2, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }

        //public GlobalClass<TBase> IsOverridedBy(Func<T1, T2, TResult> destination)
        //{
        //    return (GlobalClass<TBase>)IsOverridedBy((Delegate)destination);
        //}
    }

    public class GlobalFunc<TBase, T1, T2, T3, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalFunc<TBase, T1, T2, T3, T4, TResult> : GlobalMethod
    {
        public GlobalFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Func<T1, T2, T3, T4, TResult> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }

    public class GlobalAction<TBase, T1, T2, T3, T4> : GlobalMethod
    {
        public GlobalAction(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> IsReplacedBy(Action<T1, T2, T3, T4> destination)
        {
            return (GlobalClass<TBase>)IsReplacedBy((Delegate)destination);
        }
    }
}

