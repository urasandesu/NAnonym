/* 
 * File: DelegateMixin.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class DelegateMixin
    {
        static class Cache<TDelegate> where TDelegate : class
        {
            public static readonly object ms_lockObj = new object();
            public static bool ms_ready;
            public static Func<object, IntPtr, TDelegate> ms_converter;
        }

        public static TDelegate Cast<TDelegate>(this Delegate source, Module associatedModule) where TDelegate : class
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (!typeof(TDelegate).IsSubclassOf(typeof(Delegate)))
                throw new ArgumentException("'TDelegate' must be a subclass of 'System.Delegate'.");

            if (!Cache<TDelegate>.ms_ready)
            {
                lock (Cache<TDelegate>.ms_lockObj)
                {
                    if (!Cache<TDelegate>.ms_ready)
                    {
                        var ctor = typeof(TDelegate).GetConstructors().First();

                        var converter = new DynamicMethod("Converter", typeof(TDelegate), new[] { typeof(object), typeof(IntPtr) }, associatedModule, true);
                        var gen = converter.GetILGenerator();
                        gen.Emit(OpCodes.Ldarg_0);
                        gen.Emit(OpCodes.Ldarg_1);
                        gen.Emit(OpCodes.Newobj, ctor);
                        gen.Emit(OpCodes.Ret);

                        Cache<TDelegate>.ms_converter = (Func<object, IntPtr, TDelegate>)converter.CreateDelegate(typeof(Func<object, IntPtr, TDelegate>));

                        Thread.MemoryBarrier();
                        Cache<TDelegate>.ms_ready = true;
                    }
                }
            }
            var @object = source.Target;
            var method = source.Method.GetFunctionPointer();
            return Cache<TDelegate>.ms_converter(@object, method);
        }
    }
}
