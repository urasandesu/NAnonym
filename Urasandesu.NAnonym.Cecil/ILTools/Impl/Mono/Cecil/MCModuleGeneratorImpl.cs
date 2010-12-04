/* 
 * File: MCModuleGeneratorImpl.cs
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
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleGeneratorImpl : MCModuleDeclarationImpl, IModuleGenerator
    {
        [NonSerialized]
        ModuleDefinition moduleDef;

        ReadOnlyCollection<ITypeGenerator> types;

        public MCModuleGeneratorImpl(ModuleDefinition moduleDef)
            : base(moduleDef)
        {
            Initialize(moduleDef);
        }

        public MCModuleGeneratorImpl(ModuleDefinition moduleDef, IAssemblyDeclaration assemblyDecl)
            : base(moduleDef, assemblyDecl)
        {
            Initialize(moduleDef);
        }

        void Initialize(ModuleDefinition moduleDef)
        {
            this.moduleDef = moduleDef;
            types = new ReadOnlyCollection<ITypeGenerator>(base.Types.TransformEnumerateOnly(typeDecl => (ITypeGenerator)typeDecl));
        }

        public new IAssemblyGenerator Assembly
        {
            get { return base.Assembly as IAssemblyGenerator; }
        }

        public ITypeGenerator AddType(string fullName, SR::TypeAttributes attr, Type parent)
        {
            int dotIndex = fullName.LastIndexOf(".");
            string @namespace = dotIndex < 0 ? string.Empty : fullName.Substring(0, dotIndex);
            string name = dotIndex < 0 ? fullName : fullName.Substring(dotIndex + 1);
            var typeDef = new TypeDefinition(@namespace, name, (TypeAttributes)attr, moduleDef.Import(parent));
            moduleDef.Types.Add(typeDef);
            return new MCTypeGeneratorImpl(typeDef);
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            base.OnDeserializedManually(context);
            Initialize(ModuleDef);
        }

        public new ReadOnlyCollection<ITypeGenerator> Types
        {
            get { return types; }
        }
    }
}

