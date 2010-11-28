/* 
 * File: ParameterDefinitionMixin.cs
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

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class ParameterDefinitionMixin
    {
        public static bool Equivalent(this ParameterDefinition x, ParameterInfo y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static bool Equivalent(this ParameterDefinition x, ParameterDefinition y)
        {
            return x.Name == y.Name && x.ParameterType.Equivalent(y.ParameterType);
        }

        public static ParameterDefinition Duplicate(this ParameterDefinition source)
        {
            var destination = new ParameterDefinition(source.Name, source.Attributes, source.ParameterType);
            return destination;
        }

        public static ParameterInfo ToParameterInfo(this ParameterDefinition source)
        {
            throw new NotImplementedException();
        }
    }
}

