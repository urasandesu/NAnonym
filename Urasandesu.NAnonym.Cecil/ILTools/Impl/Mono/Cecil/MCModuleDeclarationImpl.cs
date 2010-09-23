using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleDeclarationImpl : UN::ILTools.DeserializableManually, UN::ILTools.IModuleDeclaration
    {
        [NonSerialized]
        ModuleReference moduleRef;
        string moduleName;

        UN::ILTools.IAssemblyDeclaration assemblyDecl;

        //[NonSerialized]
        //bool deserialized;

        public MCModuleDeclarationImpl(ModuleReference moduleRef)
            : base(true)
        {
            Initialize(moduleRef);
        }

        void Initialize(ModuleReference moduleRef)
        {
            this.moduleRef = moduleRef;
            moduleName = moduleRef.Name;
            assemblyDecl = new MCAssemblyDeclarationImpl(((ModuleDefinition)moduleRef).Assembly);
        }

        #region IModuleDeclaration メンバ

        public UN::ILTools.IAssemblyDeclaration Assembly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        internal ModuleReference ModuleRef { get { return moduleRef; } }
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
            var assemblyDef = assemblyDecl.AssemblyDef;
            Initialize(assemblyDef.Modules.First(module => module.Name == moduleName));
        }
    }
}
