/* 
 * File: MethodReferenceMixin.cs
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
using System.Reflection;
using Urasandesu.NAnonym.Cecil.Mixins.System.Collections.Generic;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class MethodReferenceMixin
    {
        public static bool Equivalent(this MethodReference x, MethodBase y)
        {
            bool equals = x.DeclaringType.Equivalent(y.DeclaringType);
            equals = equals && x.Name == y.Name;
            equals = equals && x.Parameters.Equivalent(y.GetParameters());
            return equals;
        }

        public static MethodReference DuplicateWithoutBody(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.DuplicateWithoutBody();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.DuplicateWithoutBody();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static MethodReference Duplicate(this MethodReference source)
        {
            var sourceDef = default(MethodDefinition);
            var sourceGen = default(GenericInstanceMethod);
            if ((sourceDef = source as MethodDefinition) != null)
            {
                return sourceDef.Duplicate();
            }
            else if ((sourceGen = source as GenericInstanceMethod) != null)
            {
                return sourceGen.Duplicate();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static MethodInfo ToMethodInfo(this MethodReference source)
        {
            return source.Resolve().ToMethodInfo();
        }

        public static ConstructorInfo ToConstructorInfo(this MethodReference methodRef)
        {
            return methodRef.Resolve().ToConstructorInfo();
        }
    }
}

