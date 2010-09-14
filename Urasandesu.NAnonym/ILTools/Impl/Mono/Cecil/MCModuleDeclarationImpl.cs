using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleDeclarationImpl : DeserializableManually, IModuleDeclaration
    {
        [NonSerialized]
        ModuleReference moduleRef;
        string moduleName;

        IAssemblyDeclaration assemblyDecl;

        //[NonSerialized]
        //bool deserialized;

        public MCModuleDeclarationImpl(ModuleReference moduleRef)
            : base(true)
        {
            Initialize(moduleRef);
        }

        public static explicit operator MCModuleDeclarationImpl(ModuleReference moduleRef)
        {
            return new MCModuleDeclarationImpl(moduleRef);
        }

        public static explicit operator ModuleReference(MCModuleDeclarationImpl moduleDecl)
        {
            return moduleDecl.moduleRef;
        }

        void Initialize(ModuleReference moduleRef)
        {
            this.moduleRef = moduleRef;
            moduleName = moduleRef.Name;
            assemblyDecl = (MCAssemblyDeclarationImpl)((ModuleDefinition)moduleRef).Assembly;
        }

        #region IModuleDeclaration メンバ

        public IAssemblyDeclaration Assembly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        protected ModuleReference ModuleRef { get { return moduleRef; } }
        protected string ModuleName { get { return moduleName; } }

        //[OnDeserialized]
        //internal void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        var assemblyDecl = (MCAssemblyDeclarationImpl)this.assemblyDecl;
        //        assemblyDecl.OnDeserialized(context);
        //        var assemblyDef = (AssemblyDefinition)assemblyDecl;
        //        Initialize(assemblyDef.Modules.First(module => module.Name == moduleName));
        //    }
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var assemblyDecl = (MCAssemblyDeclarationImpl)this.assemblyDecl;
            assemblyDecl.OnDeserialized(context);
            var assemblyDef = (AssemblyDefinition)assemblyDecl;
            Initialize(assemblyDef.Modules.First(module => module.Name == moduleName));
        }
    }
}
