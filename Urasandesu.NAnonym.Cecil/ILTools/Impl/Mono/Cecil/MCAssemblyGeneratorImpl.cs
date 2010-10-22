using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using UNI = Urasandesu.NAnonym.ILTools;
using System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCAssemblyGeneratorImpl : MCAssemblyDeclarationImpl, UNI::IAssemblyGenerator
    {
        public MCAssemblyGeneratorImpl(AssemblyDefinition assemblyDef)
            : base(assemblyDef)
        {
        }

        #region IAssemblyGenerator メンバ

        public UNI::IModuleGenerator AddModule(string name)
        {
            var moduleDef = ModuleDefinition.CreateModule(name, ModuleKind.Dll);
            AssemblyDef.Modules.Add(moduleDef);
            return new MCModuleGeneratorImpl(moduleDef);
        }

        public UNI::IAssemblyGenerator CreateInstance(AssemblyName name)
        {
            var assemblyNameDef = new AssemblyNameDefinition(name.Name, name.Version);
            var assemblyDef = AssemblyDefinition.CreateAssembly(assemblyNameDef, name.Name, ModuleKind.Dll);
            return new MCAssemblyGeneratorImpl(assemblyDef);
        }

        #endregion
    }
}
