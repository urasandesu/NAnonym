using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRModuleDeclarationImpl : IModuleDeclaration
    {
        Module module;
        IAssemblyDeclaration assemblyDecl;
        public SRModuleDeclarationImpl(Module module)
        {
            this.module = module;
            var assemblyBuilder = ExportAssembly(module);
            if (assemblyBuilder != null)
            {
                assemblyDecl = new SRAssemblyGeneratorImpl(assemblyBuilder);
            }
        }

        static AssemblyBuilder ExportAssembly(Module module)
        {
            if (!(module is ModuleBuilder)) return null;

            var moduleBuilderType = typeof(ModuleBuilder);
            var m_assemblyBuilder = moduleBuilderType.GetField("m_assemblyBuilder", BindingFlags.NonPublic | BindingFlags.Instance);
            return m_assemblyBuilder == null ? default(AssemblyBuilder) : (AssemblyBuilder)m_assemblyBuilder.GetValue(module);
        }

        #region IModuleDeclaration メンバ

        public IAssemblyDeclaration Assembly
        {
            get { return assemblyDecl; }
        }

        #endregion

        #region IDeserializableManually メンバ

        public void OnDeserialized(StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
