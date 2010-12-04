/* 
 * File: PortableScopeRawData.cs
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
using Urasandesu.NAnonym.Linq;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.ILTools
{
    [Serializable]
    public sealed class PortableScopeRawData
    {
        public PortableScopeRawData(string typeAssemblyQualifiedName, string methodName, string[] parameterTypeFullNames)
        {
            TypeAssemblyQualifiedName = typeAssemblyQualifiedName;
            MethodName = methodName;
            ParameterTypeFullNames = parameterTypeFullNames;
        }

        public PortableScopeRawData(IMethodBaseDeclaration methodDecl)
            : this(methodDecl.DeclaringType.AssemblyQualifiedName,
                   methodDecl.Name,
                   methodDecl.Parameters.Select(parameter => parameter.ParameterType.FullName).ToArray())
        {
        }

        public string TypeAssemblyQualifiedName { get; private set; }
        public string MethodName { get; private set; }
        public string[] ParameterTypeFullNames { get; private set; }

        public static bool operator ==(PortableScopeRawData x, PortableScopeRawData y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PortableScopeRawData x, PortableScopeRawData y)
        {
            return !(x == y);
        }

        public bool Equals(PortableScopeRawData that)
        {
            bool equals = TypeAssemblyQualifiedName == that.TypeAssemblyQualifiedName;
            equals = equals && MethodName == that.MethodName;
            equals = equals && ParameterTypeFullNames.Equivalent(that.ParameterTypeFullNames);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is PortableScopeRawData)) return false;

            var that = (PortableScopeRawData)obj;

            return Equals(that);
        }

        public override int GetHashCode()
        {
            int typeFullNameHashCode = TypeAssemblyQualifiedName.NullableGetHashCode();
            int methodNameHashCode = MethodName.NullableGetHashCode();
            int parameterTypeFullNamesHashCode = ParameterTypeFullNames.GetAggregatedHashCodeOrDefault();

            return typeFullNameHashCode ^ methodNameHashCode ^ parameterTypeFullNamesHashCode;
        }
    }
}

