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
    class MCModuleGeneratorImpl : MCModuleDeclarationImpl, UN::ILTools.IModuleGenerator
    {
        [NonSerialized]
        ModuleDefinition moduleDef;

        //[NonSerialized]
        //bool deserialized;

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

        public new UN::ILTools.IAssemblyGenerator Assembly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        //[OnDeserialized]
        //internal new void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        base.OnDeserialized(context);
        //        Initialize((ModuleDefinition)ModuleRef);
        //    }
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            base.OnDeserializedManually(context);
            Initialize((ModuleDefinition)ModuleRef);
        }
    }
}
