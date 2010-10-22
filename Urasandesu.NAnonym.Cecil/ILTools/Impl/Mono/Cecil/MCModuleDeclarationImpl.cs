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
    class MCModuleDeclarationImpl : UNI::DeserializableManually, UNI::IModuleDeclaration
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

        #region IModuleDeclaration メンバ

        public UNI::IAssemblyDeclaration Assembly
        {
            get { return assemblyGen; }
        }

        #endregion

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
