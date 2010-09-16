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
    class MCAssemblyGeneratorImpl : MCAssemblyDeclarationImpl, UN::ILTools.IAssemblyGenerator
    {
        public MCAssemblyGeneratorImpl(AssemblyDefinition assemblyDef)
            : base(assemblyDef)
        {
        }

        public static explicit operator MCAssemblyGeneratorImpl(AssemblyDefinition assemblyDef)
        {
            return new MCAssemblyGeneratorImpl(assemblyDef);
        }

        public static explicit operator AssemblyDefinition(MCAssemblyGeneratorImpl assemblyGen)
        {
            return assemblyGen.AssemblyDef;
        }
    }
}
