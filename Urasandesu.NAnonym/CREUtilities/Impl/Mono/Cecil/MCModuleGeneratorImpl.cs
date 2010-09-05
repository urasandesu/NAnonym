using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.CREUtilities.Impl.Mono.Cecil
{
    [Serializable]
    class MCModuleGeneratorImpl : MCModuleDeclarationImpl, IModuleGenerator
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

        public static explicit operator ModuleDefinition(MCModuleGeneratorImpl moduleGen)
        {
            return moduleGen.moduleDef;
        }

        public static explicit operator MCModuleGeneratorImpl(ModuleDefinition moduleDef)
        {
            return new MCModuleGeneratorImpl(moduleDef);
        }

        #region IModuleGenerator メンバ

        public new IAssemblyGenerator Assembly
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
