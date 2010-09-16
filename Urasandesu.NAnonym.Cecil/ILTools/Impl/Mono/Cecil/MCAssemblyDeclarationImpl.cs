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
    class MCAssemblyDeclarationImpl : UN::ILTools.DeserializableManually, UN::ILTools.IAssemblyDeclaration
    {
        [NonSerialized]
        AssemblyDefinition assemblyDef;
        string assemblyFullName;

        //[NonSerialized]
        //bool deserialized;

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

        public static explicit operator MCAssemblyDeclarationImpl(AssemblyDefinition assemblyDef)
        {
            return new MCAssemblyDeclarationImpl(assemblyDef);
        }

        public static explicit operator AssemblyDefinition(MCAssemblyDeclarationImpl assemblyDecl)
        {
            return assemblyDecl.assemblyDef;
        }

        protected AssemblyDefinition AssemblyDef { get { return assemblyDef; } }
        protected string AssemblyFullName { get { return assemblyFullName; } }

        //[OnDeserialized]
        //internal void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        var assemblyDef = GlobalAssemblyResolver.Instance.Resolve(assemblyFullName);
        //        Initialize(assemblyDef);
        //    }
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var assemblyDef = GlobalAssemblyResolver.Instance.Resolve(assemblyFullName);
            Initialize(assemblyDef);
        }
    }
}
