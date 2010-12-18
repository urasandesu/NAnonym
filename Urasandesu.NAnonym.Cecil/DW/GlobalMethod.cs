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
    public abstract class GlobalMethod : DependencyMethod
    {
        public GlobalMethod(GlobalClass globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        protected DependencyClass HideBy(Delegate @delegate)
        {
            @class.MethodSet.Add(new WeaveMethodInfo(GlobalSetupModes.Hide, source, @delegate.Method, @delegate.GetType()));
            return @class;
        }

        protected DependencyClass BeforeRun(Delegate @delegate)
        {
            @class.MethodSet.Add(new WeaveMethodInfo(GlobalSetupModes.Before, source, @delegate.Method, @delegate.GetType()));
            return @class;
        }

        protected DependencyClass AfterRun(Delegate @delegate)
        {
            @class.MethodSet.Add(new WeaveMethodInfo(GlobalSetupModes.After, source, @delegate.Method, @delegate.GetType()));
            return @class;
        }
    }

    public class GlobalBeforeSource : DependencyBeforeSource
    {
        public GlobalBeforeSource(Type type, MethodBase method)
            : base(type, method)
        {
        }
    }

    public class GlobalAfterSource : DependencyBeforeSource
    {
        public GlobalAfterSource(Type type, MethodBase method)
            : base(type, method)
        {
        }
    }

    public class GlobalBeforeFunc<TBase, TResult> : GlobalMethod
    {
        public GlobalBeforeFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> Run(Action<GlobalBeforeSource> beforeHandler)
        {
            return (GlobalClass<TBase>)BeforeRun((Delegate)beforeHandler);
        }
    }

    public class GlobalBeforeFunc<TBase, T, TResult> : GlobalMethod
    {
        public GlobalBeforeFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> Run(Action<GlobalBeforeSource, T> beforeHandler)
        {
            return (GlobalClass<TBase>)BeforeRun((Delegate)beforeHandler);
        }
    }

    public class GlobalAfterFunc<TBase, TResult> : GlobalMethod
    {
        public GlobalAfterFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> Run(Action<GlobalAfterSource, TResult> afterHandler)
        {
            return (GlobalClass<TBase>)AfterRun((Delegate)afterHandler);
        }
    }

    public class GlobalAfterFunc<TBase, T, TResult> : GlobalMethod
    {
        public GlobalAfterFunc(GlobalClass<TBase> globalClass, MethodInfo source)
            : base(globalClass, source)
        {
        }

        public GlobalClass<TBase> Run(Action<GlobalAfterSource, T, TResult> afterHandler)
        {
            return (GlobalClass<TBase>)AfterRun((Delegate)afterHandler);
        }
    }

}

