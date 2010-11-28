/* 
 * File: TypeMixin.cs
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

namespace Urasandesu.NAnonym.Cecil.Mixins.System
{
    public static class TypeMixin
    {
        public static TypeDefinition ToTypeDef(this Type type)
        {
            var assemblyDef = GlobalAssemblyResolver.Instance.Resolve(type.Assembly.FullName);
            return (TypeDefinition)assemblyDef.MainModule.LookupToken(type.MetadataToken);
        }

        public static GenericInstanceType ToTypeGen(this Type type)
        {
            var genericInstanceType = new GenericInstanceType(type.ToTypeDef());
            type.GetGenericArguments().Select(_ => _.ToTypeRef()).AddRangeTo(genericInstanceType.GenericArguments);
            return genericInstanceType;
        }

        public static ArrayType ToTypeArray(this Type type)
        {
            var arrayType = new ArrayType(type.GetElementType().ToTypeRef(), type.GetArrayRank());
            return arrayType;
        }

        public static TypeReference ToTypeRef(this Type type)
        {
            if (type.IsArray)
            {
                return type.ToTypeArray();
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                return type.ToTypeGen();
            }
            else
            {
                return type.ToTypeDef();
            }
        }
    }
}

