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
using System.Collections.ObjectModel;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleDeclarationImpl : UN::ManuallyDeserializable, UNI::IModuleDeclaration
    {
        [NonSerialized]
        ModuleDefinition moduleDef;
        string moduleName;

        UNI::IAssemblyDeclaration assemblyDecl;

        int lastTypesCount = -1;

        ReadOnlyCollection<UNI::ITypeDeclaration> types;

        // HACK: Cecil については、トップダウンで構築する場合は、上位オブジェクトを入れる引数を持つオーバーロード、ボトムアップで構築する場合は、そのオブジェクトのみのオーバーロードで統一できそう。
        public MCModuleDeclarationImpl(ModuleReference moduleRef)
            : this(moduleRef, null)
        {
        }

        public MCModuleDeclarationImpl(ModuleReference moduleRef, UNI::IAssemblyDeclaration assemblyDecl)
            : base(true)
        {
            Initialize(moduleRef, assemblyDecl);
        }

        void Initialize(ModuleReference moduleRef, UNI::IAssemblyDeclaration assemblyDecl)
        {
            this.moduleDef = (ModuleDefinition)moduleRef;
            moduleName = moduleRef.Name;
            this.assemblyDecl = assemblyDecl != null ? assemblyDecl : new MCAssemblyGeneratorImpl(((ModuleDefinition)moduleRef).Assembly);
            InitializeMembersIfNecessary(this);
            this.types = new ReadOnlyCollection<UNI::ITypeDeclaration>(
                ((ModuleDefinition)moduleRef).Types.TransformEnumerateOnly(typeDef => (UNI::ITypeDeclaration)new MCTypeGeneratorImpl(typeDef, this)));
        }

        void InitializeMembersIfNecessary(MCModuleDeclarationImpl @this)
        {
            if (lastTypesCount < 0)
            {
                @this.types = new ReadOnlyCollection<UNI::ITypeDeclaration>(new UNI::ITypeDeclaration[] { });
                lastTypesCount = 0;
            }

            if (lastTypesCount !=  moduleDef.Types.Count)
            {
                var types = moduleDef.Types.Where(typeDef => typeDef.Name != "<Module>").ToArray();
                @this.types = new ReadOnlyCollection<UNI::ITypeDeclaration>(
                    types.TransformEnumerateOnly(typeDef => (UNI::ITypeDeclaration)new MCTypeGeneratorImpl(typeDef, this)));
            }
        }

        public UNI::IAssemblyDeclaration Assembly
        {
            get { return assemblyDecl; }
        }

        internal ModuleDefinition ModuleDef { get { return moduleDef; } }
        protected string ModuleName { get { return moduleName; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var assemblyDecl = (MCAssemblyDeclarationImpl)this.assemblyDecl;
            assemblyDecl.OnDeserialized(context);
            var assemblyDef = assemblyDecl.AssemblyDef;
            Initialize(assemblyDef.Modules.First(module => module.Name == moduleName), assemblyDecl);
        }

        public ReadOnlyCollection<UNI::ITypeDeclaration> Types
        {
            get 
            {
                InitializeMembersIfNecessary(this);
                return types; 
            }
        }

    }
}

