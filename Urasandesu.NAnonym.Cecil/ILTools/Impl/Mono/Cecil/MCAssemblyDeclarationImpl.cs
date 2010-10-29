using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using System.IO;
using UN = Urasandesu.NAnonym;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCAssemblyDeclarationImpl : UN::ILTools.ManuallyDeserializable, UN::ILTools.IAssemblyDeclaration
    {
        [NonSerialized]
        AssemblyDefinition assemblyDef;
        string assemblyFullName;

        public MCAssemblyDeclarationImpl(AssemblyDefinition assemblyDef)
            : base(true)
        {
            Initialize(assemblyDef);
        }

        void Initialize(AssemblyDefinition assemblyDef)
        {
            this.assemblyDef = assemblyDef;
            assemblyFullName = assemblyDef.FullName;
        }

        internal AssemblyDefinition AssemblyDef { get { return assemblyDef; } }
        protected string AssemblyFullName { get { return assemblyFullName; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var assemblyDef = GlobalAssemblyResolver.Instance.Resolve(assemblyFullName);
            Initialize(assemblyDef);
        }
    }
}
