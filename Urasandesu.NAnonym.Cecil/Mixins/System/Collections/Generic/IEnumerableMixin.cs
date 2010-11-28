/* 
 * File: IEnumerableMixin.cs
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
using Mono.Cecil;
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.Mixins.System.Collections.Generic
{
    public static class IEnumerableMixin
    {
        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<Type> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(Type),
                (firstItem, secondItem) =>
                {
                    return firstItem.ParameterType.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterInfo> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterInfo),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }

        public static bool Equivalent(this IEnumerable<ParameterDefinition> first, IEnumerable<ParameterDefinition> second)
        {
            var comparer =
                Iterable.CreateEqualityComparerNullable(default(ParameterDefinition), default(ParameterDefinition),
                (firstItem, secondItem) =>
                {
                    return firstItem.Equivalent(secondItem);
                });
            return first.Equivalent(second, comparer);
        }
    }
}

