/* 
 * File: ValueTypeMixin.cs
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
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class ValueTypeMixin
    {
        static Dictionary<Type, Func<ValueType, ValueType>> ms_cloneCache = new Dictionary<Type, Func<ValueType, ValueType>>();

        static ValueTypeMixin()
        {
            Clone(default(DateTime));
            Clone(default(Boolean));
            Clone(default(Byte));
            Clone(default(Char));
            Clone(default(Decimal));
            Clone(default(Double));
            Clone(default(Guid));
            Clone(default(Int16));
            Clone(default(Int32));
            Clone(default(Int64));
            Clone(default(IntPtr));
            Clone(default(SByte));
            Clone(default(Single));
            Clone(default(TimeSpan));
            Clone(default(UInt16));
            Clone(default(UInt32));
            Clone(default(UInt64));
            Clone(default(UIntPtr));
        }

        public static ValueType Clone(this ValueType source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            lock (ms_cloneCache)
            {
                var sourceType = source.GetType();
                if (!ms_cloneCache.ContainsKey(sourceType))
                {
                    var cloneMethod = new DynamicMethod("Clone", typeof(ValueType), new Type[] { typeof(ValueType) }, true);
                    var gen = cloneMethod.GetILGenerator();
                    var local = gen.DeclareLocal(sourceType);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Unbox_Any, sourceType);
                    gen.Emit(OpCodes.Stloc_0);
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Box, sourceType);
                    gen.Emit(OpCodes.Ret);
                    var clone = (Func<ValueType, ValueType>)cloneMethod.CreateDelegate(typeof(Func<ValueType, ValueType>));
                    ms_cloneCache.Add(sourceType, clone);
                }
                return ms_cloneCache[sourceType](source);
            }
        }
    }
}
