using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;
using UN = Urasandesu.NAnonym;
using Urasandesu.NAnonym.ILTools;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleGeneratorImpl : MCModuleDeclarationImpl, IModuleGenerator
    {
        [NonSerialized]
        ModuleDefinition moduleDef;

        public MCModuleGeneratorImpl(ModuleDefinition moduleDef)
            : base(moduleDef)
        {
            Initialize(moduleDef);
        }

        void Initialize(ModuleDefinition moduleDef)
        {
            this.moduleDef = moduleDef;
        }

        #region IModuleGenerator メンバ

        public new IAssemblyGenerator Assembly
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeGenerator AddType(string fullName, SR::TypeAttributes attr, Type parent)
        {
            int dotIndex = fullName.LastIndexOf(".");
            string @namespace = dotIndex < 0 ? string.Empty : fullName.Substring(0, dotIndex);
            string name = dotIndex < 0 ? fullName : fullName.Substring(dotIndex + 1);
            var typeDef = new TypeDefinition(@namespace, name, (TypeAttributes)attr, moduleDef.Import(parent));
            moduleDef.Types.Add(typeDef);
            return new MCTypeGeneratorImpl(typeDef);
        }

        #endregion

        protected override void OnDeserializedManually(StreamingContext context)
        {
            base.OnDeserializedManually(context);
            Initialize((ModuleDefinition)ModuleRef);
        }
    }
}
