using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil;
using SR = System.Reflection;
using System.Runtime.Serialization;
using System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.Mono.Cecil
{
    [Serializable]
    class MCTypeDeclarationImpl : MCMemberDeclarationImpl, ITypeDeclaration
    {
        [NonSerialized]
        TypeReference typeRef;

        [NonSerialized]
        TypeDefinition typeDef;

        [NonSerialized]
        ITypeDeclaration baseTypeDecl;

        string typeFullName;

        IModuleDeclaration moduleDecl;

        //[NonSerialized]
        //bool deserialized;
        
        public MCTypeDeclarationImpl(TypeReference typeRef)
            : base(typeRef)
        {
            Initialize(typeRef);
        }

        void Initialize(TypeReference typeRef)
        {
            this.typeRef = typeRef;
            typeDef = typeRef.Resolve();
            typeFullName = typeDef.FullName;
            moduleDecl = (MCModuleDeclarationImpl)typeRef.Module;
            baseTypeDecl = typeRef.Equivalent(typeof(object)) ? null : (MCTypeDeclarationImpl)typeDef.BaseType;
        }

        public static explicit operator MCTypeDeclarationImpl(TypeReference typeRef)
        {
            return new MCTypeDeclarationImpl(typeRef);
        }

        public static explicit operator TypeReference(MCTypeDeclarationImpl typeDecl)
        {
            return typeDecl.typeRef;
        }

        public string FullName
        {
            get { return typeRef.FullName; }
        }

        public string AssemblyQualifiedName
        {
            get { return typeRef.FullName + ", " + typeRef.Module.Assembly.FullName; }
        }

        public ITypeDeclaration BaseType
        {
            get { return baseTypeDecl; }
        }

        public IModuleDeclaration Module { get { throw new NotImplementedException(); } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            // TODO: 本当は SR::BindingFlags.Default が正しい。修正。
            // MEMO: System.Object..ctor をそのまま参照させると、自身のコンストラクタ呼び出しに変換されてしまう？？
            if (typeRef.Equivalent(typeof(object)))
            {
                return (MCConstructorDeclarationImpl)typeRef.Module.Import(
                    typeof(object).GetConstructor(SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, null, types, null));
            }
            else
            {
                return (MCConstructorDeclarationImpl)typeDef.GetConstructor(
                    SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, types);
            }
        }

        protected TypeDefinition TypeDef { get { return typeDef; } }

        //[OnDeserialized]
        //internal void OnDeserialized(StreamingContext context)
        //{
        //    if (!deserialized)
        //    {
        //        deserialized = true;
        //        var moduleDecl = (MCModuleDeclarationImpl)this.moduleDecl;
        //        moduleDecl.OnDeserialized(context);
        //        var moduleDef = (ModuleDefinition)(ModuleReference)moduleDecl;
        //        Initialize(moduleDef.Types.First(type => type.FullName == typeFullName));
        //    }
        //}

        //public override void OnDeserialization(object sender)
        //{
        //    //var moduleDecl = (MCModuleDeclarationImpl)this.moduleDecl;
        //    //moduleDecl.OnDeserialized(context);
        //    var moduleDef = (ModuleDefinition)(ModuleReference)moduleDecl;
        //    var typeDef = moduleDef.Types.First(type => type.FullName == typeFullName);
        //    Initialize(typeDef);
        //    base.OnDeserialization(typeDef);
        //}

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var moduleDecl = (MCModuleDeclarationImpl)this.moduleDecl;
            moduleDecl.OnDeserialized(context);
            var moduleDef = (ModuleDefinition)(ModuleReference)moduleDecl;
            var typeDef = moduleDef.Types.First(type => type.FullName == typeFullName);
            Initialize(typeDef);
            base.OnDeserializedManually(context);
        }

        #region ITypeDeclaration メンバ


        public IFieldDeclaration[] GetFields(BindingFlags attr)
        {
            return typeDef.GetFields(attr).Select(fieldDef => (IFieldDeclaration)(MCFieldGeneratorImpl)fieldDef).ToArray();
        }

        #endregion
    }

}
