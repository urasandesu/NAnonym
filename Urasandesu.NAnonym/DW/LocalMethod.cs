/* 
 * File: LocalMethod.cs
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

namespace Urasandesu.NAnonym.DW
{
    public abstract class LocalMethod : DependencyMethod
    {
        public LocalMethod(LocalClass localClass, MethodInfo source)
            : base(localClass, source)
        {
        }
    }


    public class LocalFunc<TBase, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }

        public LocalClass<TBase> IsOverridedBy(FuncWithBase<T1, T2, TResult> destination)
        {
            return (LocalClass<TBase>)IsOverridedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalFunc<TBase, T1, T2, T3, T4, TResult> : LocalMethod
    {
        public LocalFunc(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Func<T1, T2, T3, T4, TResult> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2, T3> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }

    public class LocalAction<TBase, T1, T2, T3, T4> : LocalMethod
    {
        public LocalAction(LocalClass<TBase> localClass, MethodInfo source)
            : base(localClass, source)
        {
        }

        public LocalClass<TBase> IsImplementedBy(Action<T1, T2, T3, T4> destination)
        {
            return (LocalClass<TBase>)IsImplementedBy((Delegate)destination);
        }
    }
}

