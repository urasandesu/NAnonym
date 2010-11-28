/* 
 * File: MCModuleDeclarationImpl.cs
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
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleDeclarationImpl : UNI::ManuallyDeserializable, UNI::IModuleDeclaration
    {
        [NonSerialized]
        ModuleReference moduleRef;
        string moduleName;

        UNI::IAssemblyGenerator assemblyGen;

        public MCModuleDeclarationImpl(ModuleReference moduleRef)
            : base(true)
        {
            Initialize(moduleRef);
        }

        void Initialize(ModuleReference moduleRef)
        {
            this.moduleRef = moduleRef;
            moduleName = moduleRef.Name;
            assemblyGen = new MCAssemblyGeneratorImpl(((ModuleDefinition)moduleRef).Assembly);
        }

        public UNI::IAssemblyDeclaration Assembly
        {
            get { return assemblyGen; }
        }

        internal ModuleReference ModuleRef { get { return moduleRef; } }
        protected string ModuleName { get { return moduleName; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var assemblyDecl = (MCAssemblyDeclarationImpl)this.assemblyGen;
            assemblyDecl.OnDeserialized(context);
            var assemblyDef = assemblyDecl.AssemblyDef;
            Initialize(assemblyDef.Modules.First(module => module.Name == moduleName));
        }
    }
}

