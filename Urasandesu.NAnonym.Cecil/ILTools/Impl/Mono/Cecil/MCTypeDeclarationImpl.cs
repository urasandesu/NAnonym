using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Mono.Cecil;
using Urasandesu.NAnonym.Cecil.ILTools.Mixins.Mono.Cecil;
using Urasandesu.NAnonym.ILTools;
using Urasandesu.NAnonym.Linq;
using SR = System.Reflection;

namespace Urasandesu.NAnonym.Cecil.ILTools.Impl.Mono.Cecil
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

        [NonSerialized]
        ReadOnlyCollection<IConstructorDeclaration> constructors;

        [NonSerialized]
        ReadOnlyCollection<IMethodDeclaration> methods;

        int lastMethodsCount = -1;

        [NonSerialized]
        ReadOnlyCollection<IFieldDeclaration> fields;
        
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
            moduleDecl = new MCModuleGeneratorImpl(typeRef.Module);
            baseTypeDecl = typeRef.Equivalent(typeof(object)) ? null : new MCTypeDeclarationImpl(typeDef.BaseType);
            fields = new ReadOnlyCollection<IFieldDeclaration>(typeDef.Fields.TransformEnumerateOnly(fieldDef => (IFieldDeclaration)new MCFieldGeneratorImpl(fieldDef)));
            InitializeMembers(this);
        }

        void InitializeMembers(MCTypeDeclarationImpl that)
        {
            if (lastMethodsCount < 0)
            {
                that.constructors = new ReadOnlyCollection<IConstructorDeclaration>(new IConstructorDeclaration[] { });
                that.methods = new ReadOnlyCollection<IMethodDeclaration>(new IMethodDeclaration[] { });
                lastMethodsCount = 0;
            }

            if (lastMethodsCount != typeDef.Methods.Count)
            {
                var constructors = typeDef.Methods.Where(methodDef => methodDef.Name == ".ctor").ToArray();
                that.constructors = new ReadOnlyCollection<IConstructorDeclaration>(
                    constructors.TransformEnumerateOnly(constructorDef => (IConstructorDeclaration)new MCConstructorGeneratorImpl(constructorDef)));

                var methods = typeDef.Methods.Where(methodDef => methodDef.Name != ".ctor").ToArray();
                that.methods = new ReadOnlyCollection<IMethodDeclaration>(
                    methods.TransformEnumerateOnly(methodDef => (IMethodDeclaration)new MCMethodGeneratorImpl(methodDef)));
            }
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

        public IModuleDeclaration Module { get { return moduleDecl; } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            // TODO: 本当は SR::BindingFlags.Default が正しい。修正。
            // MEMO: System.Object..ctor をそのまま参照させると、自身のコンストラクタ呼び出しに変換されてしまう？？
            if (typeRef.Equivalent(typeof(object)))
            {
                return new MCConstructorDeclarationImpl(typeRef.Module.Import(
                    typeof(object).GetConstructor(SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, null, types, null)));
            }
            else
            {
                return new MCConstructorDeclarationImpl(typeDef.GetConstructor(
                    SR::BindingFlags.Public | SR::BindingFlags.NonPublic | SR::BindingFlags.Instance, types));
            }
        }

        protected TypeDefinition TypeDef { get { return typeDef; } }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            var moduleDecl = (MCModuleDeclarationImpl)this.moduleDecl;
            moduleDecl.OnDeserialized(context);
            var moduleDef = (ModuleDefinition)moduleDecl.ModuleRef;
            var typeDef = moduleDef.Types.First(type => type.FullName == typeFullName);
            Initialize(typeDef);
            base.OnDeserializedManually(context);
        }

        public IFieldDeclaration[] GetFields(BindingFlags attr)
        {
            return typeDef.GetFields(attr).Select(fieldDef => (IFieldDeclaration)(MCFieldGeneratorImpl)fieldDef).ToArray();
        }

        public IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            var fieldDef = typeDef.GetFieldOrDefault(name, bindingAttr);
            return fieldDef == null ? null : new MCFieldGeneratorImpl(fieldDef);
        }

        public ReadOnlyCollection<IFieldDeclaration> Fields
        {
            get { return fields; }
        }

        public ReadOnlyCollection<IConstructorDeclaration> Constructors
        {
            get
            {
                InitializeMembers(this);
                return constructors;
            }
        }

        public ReadOnlyCollection<IMethodDeclaration> Methods
        {
            get 
            {
                InitializeMembers(this);
                return methods;
            }
        }

        public new Type Source
        {
            get { return typeDef.ToType(); }
        }
    }
}
