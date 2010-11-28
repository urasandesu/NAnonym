/* 
 * File: ModuleDefinitionMixin.cs
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
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil;

namespace Urasandesu.NAnonym.Cecil.Mixins.Mono.Cecil
{
    public static class ModuleDefinitionMixin
    {
        public static ModuleDefinition Duplicate(this ModuleDefinition source)
        {
            throw new NotImplementedException();
        }

        public static ITypeGenerator AddType(this ModuleDefinition moduleDef, string fullName)
        {
            Required.NotDefault(fullName, () => fullName);

            var periodLastIndex = fullName.LastIndexOf('.');
            var @namespace = periodLastIndex < 0 ? string.Empty : fullName.Substring(0, periodLastIndex);
            var name = periodLastIndex < 0 ? fullName : fullName.Substring(periodLastIndex + 1);
            var defaultTypeAttribute = TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.Public;
            var typeDef = new TypeDefinition(@namespace, name, defaultTypeAttribute, moduleDef.Import(typeof(object)));
            moduleDef.Types.Add(typeDef);

            return new MCTypeGeneratorImpl(typeDef);
        }

        public static ITypeGenerator ReadType(this ModuleDefinition moduleDef, string fullName)
        {
            return new MCTypeGeneratorImpl(moduleDef.GetType(fullName));
        }
    }
}

